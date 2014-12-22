using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bond : MonoBehaviour {
	public BondAttachment attachment1;
	public BondAttachment attachment2;
	public GameObject linkPrefab;
	public List<BondLink> links;
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

	void Update()
	{
		lengthFresh = false;

		if (attachment1.attachee != null || attachment2.attachee != null)
		{
			bool isCountEven = links.Count % 2 == 0;

			// Round the count of links down to an odd number.
			int oddLinkCount = (links.Count % 2 == 0) ? links.Count - 1 : links.Count;

			// If the bond is too short to require the current number of bonds, remove some. (Attempt to keep count odd)
			if (BondLength < stats.removeLinkDistance * oddLinkCount)
			{
				// Maintain the end points.
				if (links.Count > 2)
				{
					RemoveLink();
					// If the link count was not even before, it is now, so remove another to stay odd.
					if (links.Count > 2 && !isCountEven)
					{
						RemoveLink();
					}
				}
			}
			// If the bond length requires more bonds, create some.
			else if (BondLength > stats.addLinkDistance * (links.Count + 1))
			{
				AddLink();
				if (links.Count % 2 == 0)
				{
					AddLink();
				}
			}
			isCountEven = links.Count % 2 == 0;


			// Direct, scale, and place link colliders to cover the surface of the bond.
			Vector3 linkDir = Vector3.zero;
			Vector3 linkScale = Vector3.zero;
			for (int i = 0; i < links.Count; i++)
			{
				linkDir = Vector3.zero;
				linkScale = links[i].transform.localScale;
				if (i == 0)
				{
					linkDir = links[i + 1].transform.position - links[i].transform.position;
					linkScale.x = (links[i + 1].transform.position - links[i].transform.position).magnitude;
					links[i].linkCollider.center = new Vector3(0, 0.5f, 0);
				}
				else if (i == links.Count - 1)
				{
					linkDir = links[i].transform.position - links[i - 1].transform.position;
					linkScale.x = (links[i].transform.position - links[i - 1].transform.position).magnitude;
					links[i].linkCollider.center = new Vector3(0, -0.5f, 0);
				}
				else
				{
					linkDir = links[i + 1].transform.position - links[i - 1].transform.position;
					float magFromPrevious = (links[i].transform.position - links[i - 1].transform.position).magnitude;
					float magToNext = (links[i + 1].transform.position - links[i].transform.position).magnitude;
					linkScale.x = magFromPrevious / 4 + magToNext / 4;
					links[i].linkCollider.center = new Vector3(0, (magFromPrevious - magToNext) / 2, 0);
				}
				links[i].transform.up = linkDir;
				links[i].linkCollider.size = linkScale;
			}

			// Base the width of the bond on how much has been drained beyond the partners' capacity.
			float warningDistance = stats.maxDistance * stats.relativeWarningDistance;
			float actualMidWidth = stats.midWidth * Mathf.Clamp(1 - ((BondLength - warningDistance) / (stats.maxDistance - warningDistance)), 0, 1);

			// Place attachment points for each partner.
			Vector3 betweenPartners = (attachment2.position - attachment1.position).normalized;
			attachment1.position = attachment1.attachee.transform.position + attachment1.attachee.transform.TransformDirection(attachment1.offset);
			attachment2.position = attachment2.attachee.transform.position + attachment2.attachee.transform.TransformDirection(attachment2.offset);

			// Place attachment points with attached characters.
			links[0].transform.position = attachment1.position;
			links[links.Count - 1].transform.position = attachment2.position;

			// Ensure smooth transition between the two lines at the center.
			if (!isCountEven && links.Count > 2)
			{
				Vector3 betweenMids = links[(links.Count/2)+1].transform.position - links[(links.Count/2)-1].transform.position;
				links[links.Count/2].transform.position = links[(links.Count/2)-1].transform.position + (betweenMids/2);
			}

			// Draw lines between bond points.
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
				Vector3 fakeMidpoint = (links[links.Count / 2].transform.position + links[links.Count / 2 -1].transform.position) / 2;
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

			// Disconnect if too far apart.
			if (actualMidWidth <= 0)
			{
				BreakBond();

			}
		}
	}
	public void BreakBond()
	{
		BondAttachable attachee1 = attachment1.attachee;
		BondAttachable attachee2 = attachment2.attachee;
		if (attachee1 != null)
		{
			attachee1.bonds.Remove(this);
			attachee1.SendMessage("BondBroken", attachee2, SendMessageOptions.DontRequireReceiver);
		}
		if (attachee2 != null)
		{
			attachee2.bonds.Remove(this);
			attachee2.SendMessage("BondBroken", attachee1, SendMessageOptions.DontRequireReceiver);
		}
		if (this != null && gameObject != null)
		{
			Destroy(gameObject);
		}
	}

	public void AttachPartners(BondAttachable attachee1, Vector3 attachPoint1, BondAttachable attachee2, Vector3 attachPoint2)
	{
		Vector3 betweenPartners = (attachee2.transform.position - attachee1.transform.position).normalized;

		attachment1.attachee = attachee1;
		attachment1.position = attachPoint1;
		attachment1.offset = attachee1.transform.InverseTransformDirection(attachPoint1 - attachee1.transform.position);

		attachment2.attachee = attachee2;
		attachment2.position = attachPoint2;
		attachment2.offset = attachee2.transform.InverseTransformDirection(attachPoint2 - attachee2.transform.position);

		Color color1 = attachment1.attachee.attachmentColor;
		Color color2 = attachment2.attachee.attachmentColor;
		Color midColor = color1 + color2;
		midColor.a = (color1.a + color2.a) / 2;
		attachment1.lineRenderer.SetColors(color1, midColor);
		attachment2.lineRenderer.SetColors(midColor, color2);

		links[0].transform.position = attachment1.position;
		links[1].transform.position = attachment2.position;

		WeightJoints();

		attachee1.SendMessage("BondMade", attachee2, SendMessageOptions.DontRequireReceiver);
		attachee2.SendMessage("BondMade", attachee1, SendMessageOptions.DontRequireReceiver);
	}

	private void AddLink()
	{
		// Create new link in bond and add it to the center of the list.
		Vector3 midpoint = (links[links.Count / 2].transform.position + links[links.Count / 2 - 1].transform.position) / 2;
		BondLink newLink = ((GameObject)Instantiate(linkPrefab, midpoint, Quaternion.identity)).GetComponent<BondLink>();
		int index = links.Count / 2;
		newLink.transform.parent = transform;
		newLink.orderLevel = links.Count / 2;
		links.Insert(index, newLink);

		// Connect the new link to its neighbors.
		BondLink previousLink = links[index - 1];
		BondLink nextLink = links[index + 1];
		newLink.jointPrevious.connectedBody = previousLink.body;
		newLink.jointNext.connectedBody = nextLink.body;
		if (previousLink.jointNext != null)
		{
			previousLink.jointNext.connectedBody = newLink.body;
		}
		if (nextLink.jointNext != null)
		{
			nextLink.jointPrevious.connectedBody = newLink.body;
		}

		//RepositionLinks();
		WeightJoints();
	}

	private void RemoveLink()
	{
		// Remove the center link and connect its remaining neighbors together.
		int index = links.Count / 2;
		BondLink previousLink = links[index - 1];
		BondLink nextLink = links[index + 1];
		if (previousLink.jointNext != null)
		{
			previousLink.jointNext.connectedBody = nextLink.body;
		}
		if (nextLink.jointNext != null)
		{
			nextLink.jointPrevious.connectedBody = previousLink.body;
		}
		Destroy(links[index].gameObject);
		links.RemoveAt(index);

		WeightJoints();
	}

	private void WeightJoints()
	{
		// Weight the strength of joints based on where links and neighbors exist in hierarchy.
		for (int i = 1; i < links.Count - 1; i++)
		{
			int prevLinkOrderLevel = links[i - 1].orderLevel;
			int nextLinkOrderLevel = (i < links.Count - 1) ? links[i + 1].orderLevel : -1;

			if (i < links.Count / 2)
			{
				links[i].jointToAttachment.connectedBody = attachment1.attachee.body;
			}
			else
			{
				links[i].jointToAttachment.connectedBody = attachment2.attachee.body;
			}
			links[i].jointToAttachment.spring = 0;

			if (links[i].jointPrevious != null)
			{
				if (links[i].orderLevel < prevLinkOrderLevel)
				{
					links[i].jointPrevious.spring = stats.downOrderSpring;
					links[i].jointPrevious.damper = stats.downOrderDamper;
				}
				else if (links[i].orderLevel == prevLinkOrderLevel)
				{
					links[i].jointPrevious.spring = stats.upOrderSpring / 2;
					links[i].jointPrevious.damper = stats.upOrderDamper / 2;
				}
				else
				{
					links[i].jointPrevious.spring = stats.upOrderSpring;
					links[i].jointPrevious.damper = stats.upOrderDamper;
				}
			}
			if (links[i].jointNext != null)
			{
				if (links[i].orderLevel < nextLinkOrderLevel)
				{
					links[i].jointNext.spring = stats.downOrderSpring;
					links[i].jointNext.damper = stats.downOrderDamper;
				}
				else if (links[i].orderLevel == nextLinkOrderLevel)
				{
					links[i].jointNext.spring = stats.upOrderSpring / 2;
					links[i].jointNext.damper = stats.upOrderDamper / 2;
				}
				else
				{
					links[i].jointNext.spring = stats.upOrderSpring;
					links[i].jointNext.damper = stats.upOrderDamper;
				}
			}
		}

		if (links.Count > 2)
		{
			links[1].jointToAttachment.spring = stats.attachSpring1;
			links[links.Count-2].jointToAttachment.spring = stats.attachSpring2;
		}
	}

	public Vector3 NearestPoint(Vector3 checkPoint)
	{
		if (links.Count < 2)
		{
			return Vector3.zero;
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

		return nearestPos;
	}

	public BondAttachable OtherAttachee(BondAttachable attachee)
	{
		if (attachment1.attachee == attachee)
		{
			return attachment2.attachee;
		}
		else if (attachment2.attachee == attachee)
		{
			return attachment1.attachee;
		}
		return null;
	}
}

[System.Serializable]
public class BondAttachment
{
	public BondAttachable attachee;
	public Vector3 position;
	public Vector3 offset;
	public LineRenderer lineRenderer;
}

[System.Serializable]
public class BondStats
{
	public float attachSpring1 = 0;
	public float attachSpring2 = 0;
	public float maxDistance = 25;
	public float relativeWarningDistance = 0.5f;
	public float endsWidth = 0.02f;
	public float midWidth = 0.5f;
	public float addLinkDistance = 0.5f;
	public float removeLinkDistance = 0.3f;
	public float upOrderSpring = 3000;
	public float upOrderDamper = 5;
	public float downOrderSpring = 2000;
	public float downOrderDamper = 0;
}
