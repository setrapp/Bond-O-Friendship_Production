using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bond : MonoBehaviour {
	public BondAttachment attachment1;
	public BondAttachment attachment2;
	public GameObject linkPrefab;
	public List<BondLink> links;
	public GameObject bondPullPrefab;
	private SpringJoint pullSpring1;
	private SpringJoint pullSpring2;
	private float bondLength;
	public float BondLength
	{
		get
		{
			if (!lengthFresh)
			{
				bondLength = 0;
				for(int i = 1; i < links.Count; i++)
				{
					bondLength += (links[i].transform.position - links[i - 1].transform.position).magnitude;
				}
				lengthFresh = true;
			}
			return bondLength;
		}
	}
	private bool lengthFresh = false;
	public BondStats stats;
	public bool forceFullDetail = false;
	public float fullDetailAddDistance = -1;
	public float fullDetailRemoveDistance = -1;
	public float currentDetail = 1;
	private bool disablingLinks = false;
	[SerializeField]
	public List<GameObject> fluffsHeld;
	private float fluffRequestTime;

	protected virtual void Start()
	{
		fullDetailAddDistance = stats.addLinkDistance;
		fullDetailRemoveDistance = stats.removeLinkDistance;
	}

	protected virtual void Update()
	{
		lengthFresh = false;
		
		currentDetail = SetLevelOfDetail();
		bool atSparseDetail = currentDetail <= stats.sparseDetailFactor;
		float frameTime = Time.time;

		if (stats.pullApartMaxFactor > 0)
		{
			if (pullSpring1 == null || pullSpring2 == null)
			{
				CreatePullers();
			}
		}
		else
		{
			if (pullSpring1 != null || pullSpring2 != null)
			{
				DestroyPullers();
			}
		}

		if (attachment1.attachee != null || attachment2.attachee != null)
		{
			StartCoroutine(UpdateBondCount(atSparseDetail));

			bool isCountEven = links.Count % 2 == 0;

			// Ensure that links with unconnected joints are not applying spring forces.
			for (int i = 0; i < links.Count; i++)
			{
				if (links[i].jointToNeighbor != null && links[i].jointToNeighbor.connectedBody == null)
				{
					links[i].jointToNeighbor.spring = 0;
				}
				if (links[i].jointToAttachment != null && links[i].jointToAttachment.connectedBody == null)
				{
					links[i].jointToAttachment.spring = 0;
				}
			}

			// Direct, scale, and place link colliders to cover the surface of the bond.
			if (!stats.manualLinks && !atSparseDetail)
			{
				Vector3 linkDir = Vector3.zero;
				Vector3 linkScalePrev = Vector3.zero;
				Vector3 linkScaleNext = Vector3.zero;
				for (int i = 1; i < links.Count - 1; i++)
				{
					linkDir = Vector3.zero;
					linkScalePrev = links[i].toPreviousCollider.size;
					linkScaleNext = links[i].toNextCollider.size;
					linkDir = links[i + 1].transform.position - links[i - 1].transform.position;
					float magFromPrevious = (links[i].transform.position - links[i - 1].transform.position).magnitude;
					float magToNext = (links[i + 1].transform.position - links[i].transform.position).magnitude;
					linkScalePrev.y = magFromPrevious;
					linkScaleNext.y = magToNext;
					links[i].toPreviousCollider.center = new Vector3(0, -linkScalePrev.y / 2, 0);
					links[i].toNextCollider.center = new Vector3(0, linkScaleNext.y / 2, 0);
					links[i].toPreviousCollider.size = linkScalePrev;
					links[i].toNextCollider.size = linkScaleNext;
					links[i].toPreviousCollider.transform.up = links[i].transform.position - links[i - 1].transform.position;
					links[i].toNextCollider.transform.up = links[i + 1].transform.position - links[i].transform.position;
					links[i].transform.up = linkDir;
				}
			}

			// Base the width of the bond on how much has been drained beyond the partners' capacity.
			float warningDistance = stats.maxDistance * stats.relativeWarningDistance;
			float actualMidWidth = (stats.relativeWarningDistance > 0) ? stats.midWidth * Mathf.Clamp(1 - (BondLength - warningDistance) / (stats.maxDistance - warningDistance), 0, 1) : stats.midWidth;

			// If stretching enough request fluffs to extend.
			if (stats.relativeRequestDistance >= 0 && stats.maxDistance * stats.relativeRequestDistance < BondLength && Time.time - fluffRequestTime >= stats.fluffRequestDelay)
			{
				if (stats.maxFluffCapacity > fluffsHeld.Count)
				{
					if (Random.Range(0.0f, 1.0f) < 0.5f)
					{
						attachment1.attachee.RequestFluff(this);
					}
					else
					{
						attachment2.attachee.RequestFluff(this);
					}
					fluffRequestTime = Time.time;
				}
			}

			// Place attachment points for each partner.
			if (!stats.manualAttachment1)
			{
				attachment1.position = attachment1.attachee.transform.position + attachment1.attachee.transform.TransformDirection(attachment1.offset);
			}
			if (!stats.manualAttachment2)
			{
				attachment2.position = attachment2.attachee.transform.position + attachment2.attachee.transform.TransformDirection(attachment2.offset);
			}

			// Place attachment points with attached characters.
			if (links.Count > 1)
			{
				if (attachment1.attachedLink != null)
				{
					attachment1.attachedLink.transform.position = attachment1.position;
				}
				if (attachment2.attachedLink != null)
				{
					attachment2.attachedLink.transform.position = attachment2.position;
				}
				
			}

			if (pullSpring1 != null && pullSpring2 != null)
			{
				float pullSpringDist = 1;
				if (stats.maxDistance >= 0)
				{
					pullSpringDist = Mathf.Max((stats.maxDistance * stats.pullApartMaxFactor) - bondLength, 0);
				}

				Vector3 betweenAttachments = (attachment2.position - attachment1.position).normalized;
				pullSpring1.transform.position = attachment1.position - (betweenAttachments * pullSpringDist);
				pullSpring2.transform.position = attachment2.position + (betweenAttachments * pullSpringDist);
			}

			// Ensure smooth transition between the two lines at the center.
			if (!isCountEven && links.Count > 2)
			{
				Vector3 betweenMids = links[(links.Count/2)+1].transform.position - links[(links.Count/2)-1].transform.position;
				links[links.Count/2].transform.position = links[(links.Count/2)-1].transform.position + (betweenMids/2);
			}

			// Draw lines between bond points.
			if (!atSparseDetail)
			{
				RenderBond(actualMidWidth, isCountEven);
			}
			else
			{
				attachment1.lineRenderer.SetVertexCount(0);
				attachment2.lineRenderer.SetVertexCount(0);
			}

			// Disconnect if too far apart.
			if (stats.maxDistance > 0 && BondLength >= stats.maxDistance)
			{
				BreakBond();
			}
		}
	}

	private IEnumerator UpdateBondCount(bool atSparseDetail)
	{
		bool isCountEven = links.Count % 2 == 0;

		// Mainting desired length of links by adding and removing.
		if (!stats.disableColliders)
		{
			if (!stats.manualLinks)
			{
				if (links.Count < 4)
				{
					AddLink();
				}
				else
				{
					int linksCheckedOnFrame = 0;
					bool bondChanged = false;
					float sqrAddDist = Mathf.Pow(stats.addLinkDistance, 2);
					float sqrRemoveDist = Mathf.Pow(stats.removeLinkDistance, 2);
					if (stats.addLinkDistance >= 0)
					{
						for (int i = 1; i < links.Count - 2; i++)
						{
							float sqrDist = (links[i + 1].transform.position - links[i].transform.position).sqrMagnitude;
							if (sqrDist > sqrAddDist)
							{
								AddLink(i + 1, false);
								bondChanged = true;
							}

							if (atSparseDetail && linksCheckedOnFrame >= stats.sparseDetailLinksCheck)
							{
								linksCheckedOnFrame = 0;
								yield return null;
							}
						}
					}
					if (stats.removeLinkDistance >= 0)
					{
						for (int i = 1; i < links.Count - 2; i++)
						{
							float sqrDist = (links[i + 1].transform.position - links[i - 1].transform.position).sqrMagnitude;
							if (sqrDist < sqrRemoveDist)
							{
								RemoveLink(i, false);
								bondChanged = true;
							}

							if (atSparseDetail && linksCheckedOnFrame >= stats.sparseDetailLinksCheck)
							{
								linksCheckedOnFrame = 0;
								yield return null;
							}
						}
					}
					if (bondChanged)
					{
						WeightJoints();
					}
				}
			}
		}
		else
		{
			for (int i = 0; i < links.Count; i++)
			{
				if (links[i].toNextCollider != null)
				{
					links[i].toNextCollider.enabled = false;
				}
				if (links[i].toPreviousCollider != null)
				{
					links[i].toPreviousCollider.enabled = false;
				}
			}
		}

		if (links.Count > 0 && attachment1.fluffPullTarget != null && attachment2.fluffPullTarget != null)
		{
			if (stats.fluffPullLinks < 0 || links.Count < stats.fluffPullLinks * 2 + 1)
			{
				attachment1.fluffPullTarget.transform.position = attachment2.fluffPullTarget.transform.position = links[links.Count / 2].transform.position;
			}
			else
			{
				attachment1.fluffPullTarget.transform.position = links[stats.fluffPullLinks].transform.position;
				attachment2.fluffPullTarget.transform.position = links[(links.Count - 1) - stats.fluffPullLinks].transform.position;
			}
		}
	}

	public virtual void RenderBond(float actualMidWidth, bool isCountEven)
	{
		if (links.Count < 2)
		{
			attachment1.lineRenderer.SetVertexCount(0);
			attachment2.lineRenderer.SetVertexCount(0);
			return;
		}

		attachment1.lineRenderer.SetVertexCount(links.Count / 2 + 1);
		for (int i = 0; i < links.Count / 2; i++)
		{
			attachment1.lineRenderer.SetPosition(i, links[i].transform.position);
		}
		attachment2.lineRenderer.SetVertexCount(links.Count / 2 + 1);
		for (int i = 1; i < links.Count / 2 + 1; i++)
		{
			if (isCountEven)
			{
				attachment2.lineRenderer.SetPosition(i, links[i + links.Count / 2 - 1].transform.position);
			}
			else
			{
				attachment2.lineRenderer.SetPosition(i, links[i + links.Count / 2].transform.position);
			}
		}

		if (isCountEven)
		{
			Vector3 fakeMidpoint = (links[links.Count / 2].transform.position + links[links.Count / 2 - 1].transform.position) / 2;
			attachment1.lineRenderer.SetPosition(links.Count / 2, fakeMidpoint);
			attachment2.lineRenderer.SetPosition(0, fakeMidpoint);
		}
		else
		{
			attachment1.lineRenderer.SetPosition(links.Count / 2, links[links.Count / 2].transform.position);
			attachment2.lineRenderer.SetPosition(0, links[links.Count / 2].transform.position);
		}

		attachment1.lineRenderer.SetWidth(stats.endsWidth, actualMidWidth);
		attachment2.lineRenderer.SetWidth(actualMidWidth, stats.endsWidth);
	}

	public virtual void BreakBond(bool quickDestroy = false)
	{
		if (fluffsHeld.Count > 0)
		{
			float linksPerFluffSpawn = links.Count / (fluffsHeld.Count + 2);
			for (int i = 0; i < fluffsHeld.Count; i++)
			{
				int linkToSpawnAt = (int)((i + 1) * linksPerFluffSpawn);
				if (linkToSpawnAt > 0 && linkToSpawnAt < links.Count)
				{
					if(fluffsHeld[i] != null && links[i] != null)
					{
						fluffsHeld[i].SetActive(true);
						fluffsHeld[i].transform.position = links[linkToSpawnAt].transform.position;
						Fluff fluff = fluffsHeld[i].GetComponent<Fluff>();
						if (fluff != null)
						{
							fluff.soleAttractor = null;
							fluff.nonAttractTime = 0;
							fluff.attractable = true;
							fluff.mover.externalSpeedMultiplier = 1.0f;
						}

						SpringJoint fluffSpring = fluff.GetComponent<SpringJoint>();
						if(fluffSpring != null)
						{
							Destroy(fluffSpring);
						}
					}
					
				}
			}
		}

		BondAttachable attachee1 = attachment1.attachee;
		BondAttachable attachee2 = attachment2.attachee;

		if (attachee1 != null)	{ attachee1.bonds.Remove(this); }
		if (attachee2 != null)	{ attachee2.bonds.Remove(this); }

		Globals.Instance.BondBroken(this);

		BondBreaking();
		if (attachee1 != null) { attachee1.SendMessage("BondBroken", attachee2, SendMessageOptions.DontRequireReceiver); }
		if (attachee2 != null) { attachee2.SendMessage("BondBroken", attachee1, SendMessageOptions.DontRequireReceiver); }

		if (this != null && gameObject != null)
		{
			Destroy(gameObject);
		}
	}

	public void AttachPartners(BondAttachable attachee1, Vector3 attachPoint1, BondAttachable attachee2, Vector3 attachPoint2)
	{
		attachment1.attachee = attachee1;
		attachment1.attachedLink = links[0];
		attachment1.position = attachPoint1;
		attachment1.offset = attachee1.transform.InverseTransformDirection(attachPoint1 - attachee1.transform.position);

		attachment2.attachee = attachee2;
		attachment2.attachedLink = links[1];
		attachment2.position = attachPoint2;
		attachment2.offset = attachee2.transform.InverseTransformDirection(attachPoint2 - attachee2.transform.position);

		Color color1 = attachment1.attachee.attachmentColor;
		Color color2 = attachment2.attachee.attachmentColor;
		Color midColor = color1 + color2;
		midColor.a = (color1.a + color2.a) / 2;
		attachment1.lineRenderer.SetColors(color1, midColor);
		attachment2.lineRenderer.SetColors(midColor, color2);

		attachment1.attachedLink.transform.position = attachment1.position;
		attachment2.attachedLink.transform.position = attachment2.position;

		if (attachment1.attachedLink.jointToNeighbor != null)
		{
			attachment1.attachedLink.jointToNeighbor.connectedBody = attachment2.attachedLink.body;
		}
		if (attachment2.attachedLink.jointToNeighbor != null)
		{
			attachment2.attachedLink.jointToNeighbor.connectedBody = attachment1.attachedLink.body;
		}

		WeightJoints();

		attachee1.SendMessage("BondMade", attachee2, SendMessageOptions.DontRequireReceiver);
		attachee2.SendMessage("BondMade", attachee1, SendMessageOptions.DontRequireReceiver);

		Globals.Instance.BondFormed(this);

		BondForming();
	}

	public void ReplacePartner(BondAttachable partnerToReplace, BondAttachable replacement)
	{
		if (replacement == partnerToReplace)
		{
			return;
		}

		if (partnerToReplace == attachment1.attachee)
		{
			attachment1.attachee.bonds.Remove(this);
			attachment1.attachee = replacement;
			replacement.bonds.Add(this);
			attachment1.attachee.SendMessage("BondMade", attachment2.attachee, SendMessageOptions.DontRequireReceiver);
		}
		else if (partnerToReplace == attachment2.attachee)
		{
			attachment2.attachee.bonds.Remove(this);
			attachment2.attachee = replacement;
			replacement.bonds.Add(this);
			attachment2.attachee.SendMessage("BondMade", attachment1.attachee, SendMessageOptions.DontRequireReceiver);
		}
	}

	public void RemovePartner(BondAttachable partner)
	{
		if (partner == attachment1.attachee)
		{
			attachment1.attachee = null;
		}
		else if (partner == attachment2.attachee)
		{
			attachment2.attachee = null;
		}
	}

	private void AddLink(int index = -1, bool weightJoints = true)
	{

		// Create new link in bond at the given index, or default to center.
		if (index < 0)
		{
			index = links.Count / 2;
		}
		Vector3 midpoint = (links[index].transform.position + links[index - 1].transform.position) / 2;
		BondLink newLink = ((GameObject)Instantiate(linkPrefab, midpoint, Quaternion.identity)).GetComponent<BondLink>();
		newLink.transform.parent = transform;
		links.Insert(index, newLink);

		// Connect the new link to its neighbors.
		BondLink previousLink = links[index - 1];
		BondLink nextLink = links[index + 1];
		newLink.linkPrevious = previousLink;
		newLink.linkNext = nextLink;

		if (previousLink != null)
		{
			previousLink.linkNext = newLink;
		}
		if (nextLink != null)
		{
			nextLink.linkPrevious = newLink;
		}

		if (newLink.jointToNeighbor != null)
		{
			newLink.jointToNeighbor.spring = 0;
		}

		if (newLink.jointToAttachment != null)
		{
			newLink.jointToAttachment.spring = 0;
		}

		if (weightJoints)
		{
			WeightJoints();
		}
		LinkAdded(newLink);
	}

	private void RemoveLink(int index, bool weightJoints = true)
	{
		// Remove link in bond at the given index, or default to center.
		if (index < 0)
		{
			index = links.Count / 2;
		}
		BondLink previousLink = links[index - 1];
		BondLink nextLink = links[index + 1];
		if (previousLink != null)
		{
			previousLink.linkNext = nextLink;
		}
		if (nextLink != null)
		{
			nextLink.linkPrevious = previousLink;
		}
		LinkRemoved(links[index]);
		Destroy(links[index].gameObject);
		links.RemoveAt(index);

		if (previousLink.jointToNeighbor != null)
		{
			previousLink.jointToNeighbor.spring = 0;
		}
		if (previousLink.jointToAttachment != null)
		{
			previousLink.jointToAttachment.spring = 0;
		}

		if (nextLink.jointToNeighbor != null)
		{
			nextLink.jointToNeighbor.spring = 0;
		}
		if (nextLink.jointToAttachment != null)
		{
			nextLink.jointToAttachment.spring = 0;
		}

		if (weightJoints)
		{
			WeightJoints();
		}
	}

	private void WeightJoints()
	{
		// Set order level of each link.
		int halfCount = (links.Count % 2 == 0) ? links.Count / 2 : links.Count / 2 + 1;
		for (int i = 0; i < halfCount; i++)
		{
			links[i].orderLevel = links[links.Count-(1+i)].orderLevel = i;
		}

		// Set mass and drag of links.
		if (stats.linkMass >= 0)
		{
			for (int i = 0; i < links.Count; i++)
			{
				links[i].body.mass = stats.linkMass;
			}
		}
		if (stats.linkDrag >= 0)
		{
			for (int i = 0; i < links.Count; i++)
			{
				links[i].body.drag = stats.linkDrag;
			}
		}

		// Weight the strength of joints based on where links and neighbors exist in hierarchy.
		if (links.Count > 2)
		{
			int startIndex = 0;
			int endIndex = links.Count - 1;
			for (int i = startIndex; i <= endIndex; i++)
			{
				if (i < links.Count / 2)
				{
					links[i].jointToAttachment.connectedBody = attachment1.attachee.body;
				}
				else
				{
					links[i].jointToAttachment.connectedBody = attachment2.attachee.body;
				}
				links[i].jointToAttachment.spring = 0;

				if (links[i].jointToNeighbor != null)
				{
					links[i].jointToNeighbor.connectedBody = null;
					links[i].jointToNeighbor.spring = 0;
					links[i].jointToNeighbor.damper = stats.springDamper;

					BondLink jointedLink = null;
					if (links[i].linkPrevious != null && links[i].linkPrevious.orderLevel > links[i].orderLevel)
					{
						jointedLink = links[i].linkPrevious;
					}
					else if (links[i].linkNext != null && links[i].linkNext.orderLevel > links[i].orderLevel)
					{
						jointedLink = links[i].linkNext;
					}

					if (jointedLink != null && jointedLink.body != null)
					{
						links[i].jointToNeighbor.connectedBody = jointedLink.body;
						links[i].jointToNeighbor.spring = stats.springForce;
					}
				}
			}

			// Attach near end links to attachments to allow pulling.
			if (pullSpring1 != null && pullSpring2 != null)
			{
				pullSpring1.spring = stats.attachSpring1;
				pullSpring2.spring = stats.attachSpring2;
			}
			else
			{
				links[1].jointToAttachment.spring = stats.attachSpring1;
				links[links.Count - 2].jointToAttachment.spring = stats.attachSpring2;
			}
		}
	}

	private void CreatePullers()
	{
		if (bondPullPrefab != null)
		{
			pullSpring1 = ((GameObject)Instantiate(bondPullPrefab, attachment1.position, Quaternion.identity)).GetComponent<SpringJoint>();
			pullSpring1.transform.parent = transform;
			pullSpring1.connectedBody = attachment1.attachee.body;
			pullSpring2 = ((GameObject)Instantiate(bondPullPrefab, attachment1.position, Quaternion.identity)).GetComponent<SpringJoint>();
			pullSpring2.transform.parent = transform;
			pullSpring2.connectedBody = attachment2.attachee.body;
		}

		WeightJoints();
	}

	private void DestroyPullers()
	{
		if (pullSpring1 != null)
		{
			Destroy(pullSpring1.gameObject);
			pullSpring1 = null;
		}
		if (pullSpring2 != null)
		{
			Destroy(pullSpring2.gameObject);
			pullSpring2 = null;
		}

		WeightJoints();
	}

	public bool AddFluff(Fluff fluff)
	{
		if (fluff == null || (stats.maxFluffCapacity >= 0 && fluffsHeld.Count >= stats.maxFluffCapacity))
		{
			return false;
		}

		if (stats.maxDistance >= 0 && stats.extensionPerFluff >= 0)
		{
			stats.maxDistance += stats.extensionPerFluff;
		}

		fluffsHeld.Add(fluff.gameObject);

		fluff.PopFluff(0.5f, -1, true);

		return true;
	}

	public Vector3 NearestPoint(Vector3 checkPoint)
	{
		BondLink nearestLink;
		return NearestPoint(checkPoint, out nearestLink);
	}

	public Vector3 NearestPoint(Vector3 checkPoint, out BondLink nearestLink)
	{
		nearestLink = null;

		if (links.Count < 1)
		{
			Debug.LogError("Attempting to find nearest point on bond with no links.");
			return Vector3.zero;
		}
		if (links.Count == 1)
		{
			nearestLink = links[0];
			return links[0].transform.position;
		}

		int nearIndex = 0;
		Vector3 nearPos = links[nearIndex].transform.position;
		float nearSqrDist = (checkPoint - nearPos).sqrMagnitude;

		// Find the nearest joint.
		for (int i = 1; i < links.Count; i++)
		{
			float sqrDist = (checkPoint - links[i].transform.position).sqrMagnitude;
			if (sqrDist < nearSqrDist)
			{
				nearSqrDist = sqrDist;
				nearIndex = i;
				nearPos = links[nearIndex].transform.position;
			}
		}

		// Find the position of the neighbor nearest to the check point.
		Vector3 neighborPos;
		if (nearIndex == 0)
		{
			neighborPos = links[nearIndex + 1].transform.position;
		}
		else if (nearIndex == links.Count - 1)
		{
			neighborPos = links[nearIndex - 1].transform.position;
		}
		else
		{
			if ((checkPoint - links[nearIndex + 1].transform.position).sqrMagnitude < (checkPoint - links[nearIndex - 1].transform.position).sqrMagnitude)
			{
				neighborPos = links[nearIndex + 1].transform.position;
			}
			else
			{
				neighborPos = links[nearIndex - 1].transform.position;
			}
		}

		// Find the nearest point on the line between the nearest joint and its neighbor.
		Vector3 toCheckOnLine = Helper.ProjectVector(neighborPos - nearPos, checkPoint - nearPos);
		Vector3 nearestPos = nearPos + toCheckOnLine;
		if (Vector3.Dot(nearestPos - nearPos, neighborPos - nearPos) < 0)
		{
			nearestPos = nearPos;
		}

		// Store the nearest link to the check point;
		nearestLink = links[nearIndex];

		return nearestPos;
	}

	public BondAttachable OtherPartner(BondAttachable partner)
	{
		if (attachment1.attachee == partner)
		{
			return attachment2.attachee;
		}
		else if (attachment2.attachee == partner)
		{
			return attachment1.attachee;
		}
		return null;
	}

	protected virtual float SetLevelOfDetail()
	{
		if (forceFullDetail || (stats.fullDetailDistance < 0 || stats.sparseDetailDistance < stats.fullDetailDistance || stats.sparseDetailFactor <= 0))
		{
			ApplyLevelOfDetail(1);
			return 1;
		}

		GameObject player1 = Globals.Instance.player1.gameObject;
		GameObject player2 = Globals.Instance.player2.gameObject;
		float player1Dist = (NearestPoint(player1.transform.position) - player1.transform.position).magnitude;
		float player2Dist = (NearestPoint(player2.transform.position) - player2.transform.position).magnitude;
		float nearestDist = (player1Dist < player2Dist) ? player1Dist : player2Dist;
		float distFraction = Mathf.Clamp((nearestDist - stats.fullDetailDistance) / (stats.sparseDetailDistance - stats.fullDetailDistance), 0, 1);
		float detailFraction = 1 - ((1 - stats.sparseDetailFactor) * distFraction);
		ApplyLevelOfDetail(detailFraction);
		return detailFraction;
	}

	private void ApplyLevelOfDetail(float detailFraction)
	{
		if (links.Count < 2)
		{
			return;
		}

		float maxLinkDistance = BondLength / 4;
		stats.addLinkDistance = Mathf.Min(fullDetailAddDistance / detailFraction, maxLinkDistance);
		stats.removeLinkDistance = Mathf.Min(fullDetailRemoveDistance / detailFraction, maxLinkDistance / 2);
		for (int i = 0; i < links.Count; i++)
		{
			if (!links[i].broken)
			{
				links[i].jointToNeighbor.spring = stats.springForce * detailFraction;
			}
		}

		if (pullSpring1 != null && pullSpring2 != null)
		{
			pullSpring1.spring = stats.attachSpring1 * detailFraction;
			pullSpring2.spring = stats.attachSpring2 * detailFraction;
		}
		else
		{
			links[1].jointToAttachment.spring = stats.attachSpring1 * detailFraction;
			links[links.Count - 2].jointToAttachment.spring = stats.attachSpring2 * detailFraction;
		}
	}
	
	// Hooks for subclasses.
	protected virtual void BondForming() {}
	protected virtual void BondBreaking(bool quickDestroy = false) { }
	protected virtual void LinkAdded(BondLink addedLink) {}
	protected virtual void LinkRemoved(BondLink removedLink) {}
}

[System.Serializable]
public class BondAttachment
{
	public BondAttachable attachee;
	public BondLink attachedLink;
	public Vector3 position;
	public Vector3 offset;
	public LineRenderer lineRenderer;
	public Rigidbody fluffPullTarget;
}

[System.Serializable]
public class BondStats
{
	public float attachSpring1 = 0;
	public float attachSpring2 = 0;
	public float linkMass = -1;
	public float linkDrag = -1;
	public float pullApartMaxFactor;
	public float maxDistance = 25;
	public float relativeWarningDistance = 0.5f;
	public float endsWidth = 0.02f;
	public float midWidth = 0.5f;
	public float addLinkDistance = 0.5f;
	public float removeLinkDistance = 0.3f;
	public float springForce = 5000;
	public float springDamper = 5;
	[Header("Fluff Boosting")]
	public float relativeRequestDistance = -1;
	public float fluffRequestDelay = -1;
	public float fluffPullForce = -1;
	public int fluffPullLinks = -1;
	public float maxFluffCapacity = -1;
	public float extensionPerFluff = -1;
	[Header("Manual Controls")]
	public bool manualAttachment1 = false;
	public bool manualAttachment2 = false;
	public bool manualLinks = false;
	public bool disableColliders = false;
	[Header("Level of Detail")]
	public float fullDetailDistance = -1;
	public float sparseDetailDistance = -1;
	public float sparseDetailFactor = -1;
	public float sparseDetailLinksCheck = 1;

	public void Overwrite(BondStats replacement, bool fullOverwrite = false)
	{
		if (replacement == null)
		{
			return;
		}

		if (fullOverwrite || replacement.attachSpring1 >= 0)			{	this.attachSpring1 = replacement.attachSpring1;						}
		if (fullOverwrite || replacement.attachSpring2 >= 0)			{	this.attachSpring2 = replacement.attachSpring2;						}
		if (fullOverwrite || replacement.linkMass >= 0)					{	this.linkMass = replacement.linkMass;								}
		if (fullOverwrite || replacement.linkDrag >= 0)					{	this.linkDrag = replacement.linkDrag;								}
		if (fullOverwrite || replacement.pullApartMaxFactor >= 0)		{	this.pullApartMaxFactor = replacement.pullApartMaxFactor;			}
		if (fullOverwrite || replacement.maxDistance >= 0)				{	this.maxDistance = replacement.maxDistance;							}
		if (fullOverwrite || replacement.relativeWarningDistance >= 0)	{	this.relativeWarningDistance = replacement.relativeWarningDistance;	}
		if (fullOverwrite || replacement.endsWidth >= 0)				{	this.endsWidth = replacement.endsWidth;								}
		if (fullOverwrite || replacement.midWidth >= 0)					{	this.midWidth = replacement.midWidth;								}
		if (fullOverwrite || replacement.addLinkDistance >= 0)			{	this.addLinkDistance = replacement.addLinkDistance;					}
		if (fullOverwrite || replacement.removeLinkDistance >= 0)		{	this.removeLinkDistance = replacement.removeLinkDistance;			}
		if (fullOverwrite || replacement.springForce >= 0)				{	this.springForce = replacement.springForce;							}
		if (fullOverwrite || replacement.springDamper >= 0)				{	this.springDamper = replacement.springDamper;						}
		if (fullOverwrite || replacement.relativeRequestDistance >= 0)	{	this.relativeRequestDistance = replacement.relativeRequestDistance;	}
		if (fullOverwrite || replacement.fluffRequestDelay >= 0)		{	this.fluffRequestDelay = replacement.fluffRequestDelay;				}
		if (fullOverwrite || replacement.fluffPullForce >= 0)			{	this.fluffPullForce = replacement.fluffPullForce;					}
		if (fullOverwrite || replacement.fluffPullLinks >= 0)			{	this.fluffPullLinks = replacement.fluffPullLinks;					}
		if (fullOverwrite || replacement.maxFluffCapacity >= 0)			{	this.maxFluffCapacity = replacement.maxFluffCapacity;				}
		if (fullOverwrite || replacement.extensionPerFluff >= 0)		{	this.extensionPerFluff = replacement.extensionPerFluff;				}
		manualAttachment1 = replacement.manualAttachment1;
		manualAttachment2 = replacement.manualAttachment2;
		if (fullOverwrite || replacement.fullDetailDistance >= 0)		{	this.fullDetailDistance = replacement.fullDetailDistance;			}
		if (fullOverwrite || replacement.sparseDetailDistance >= 0)		{	this.sparseDetailDistance = replacement.sparseDetailDistance;		}
		if (fullOverwrite || replacement.sparseDetailFactor >= 0)		{	this.sparseDetailFactor = replacement.sparseDetailFactor;			}
	}
}
