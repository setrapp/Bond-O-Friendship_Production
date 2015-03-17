using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Membrane : Bond {
	public Color attachmentColor;
	public BondStatsHolder internalBondStats;
	public bool preferNewBonds = false;
	public List<ShapingPoint> shapingPoints;
	public MembraneStats extraStats;
	public Membrane membranePrevious;
	public Membrane membraneNext;
	[HideInInspector]
	public Vector3 startPosition1;
	[HideInInspector]
	public Vector3 startPosition2;
	public LineRenderer smoothCornerLine1;
	public LineRenderer smoothCornerLine2;
	public MembraneLink attachment1FauxLink;
	public MembraneLink attachment2FauxLink;
	public float endpointSpring = -1;
	private FixedJoint jointToPrevious;
	public float fullDetailShapingForce;
	public float fullDetailSmoothForce;
	private bool deconstructing = false;
	public bool Breaking { get { return deconstructing; } }
	private List<MembraneLink> breakLinks;
	private List<LineRenderer> breakLines;
	private bool hidingShaping = false;

	protected override void Start()
	{
		base.Start();
		fullDetailShapingForce = extraStats.defaultShapingForce;
		fullDetailSmoothForce = extraStats.smoothForce;
		breakLinks = new List<MembraneLink>();
	}

	protected override void Update()
	{
		base.Update();
		/*TODO not sure any of this is usable*/
		/*if (currentDetail <= stats.sparseDetailFactor && !deconstructing)
		{
			if (!hidingShaping)
			{
				for (int i = 0; i < links.Count; i++)
				{
					MembraneLink link = links[i] as MembraneLink;
					for (int j = 0; j < link.jointsShaping.Length; j++)
					{
						if (link.jointsShaping[j] != null)
						{
							link.jointsShaping[j].spring = 0;
							//link.jointToNeighbor.spring = 0;
							//link.jointToAttachment.spring = 0;
						}
						link.body.velocity = Vector3.zero;
						//link.body.isKinematic = true;
					}
				}
				hidingShaping = true;
			}
			extraStats.smoothForce = 0;
			//return;
		}
		else if (hidingShaping)
		{
			for (int i = 0; i < links.Count; i++)
			{
				MembraneLink link = links[i] as MembraneLink;
				for (int j = 0; j < link.jointsShaping.Length; j++)
				{
					if (link.jointsShaping[j] != null && link.jointsShaping[j].connectedBody != null)
					{
						ShapingPoint shapingPont = link.jointsShaping[j].connectedBody.GetComponent<ShapingPoint>();
						if (shapingPont != null && shapingPont.shapingForce >= 0)
						{
							link.jointsShaping[j].spring = shapingPont.shapingForce;
						}
						else
						{
							link.jointsShaping[j].spring = extraStats.defaultShapingForce;
						}
						//link.jointToNeighbor.spring = stats.springForce;
						//link.body.isKinematic = false;
						link.body.velocity = Vector3.zero;
					}

				}
			}
			if (links.Count > 3)
			{
				//links[1].jointToAttachment.spring = stats.attachSpring1;
				//links[links.Count - 2].jointToAttachment.spring = stats.attachSpring2;
			}
			hidingShaping = false;
		}*/

		if (!deconstructing)
		{
			for (int i = 1; i < links.Count - 1; i++)
			{
				MembraneLink membraneLink = links[i] as MembraneLink;
				ApplyShaping(membraneLink);
			}
		}

		if (extraStats.smoothForce > 0)
		{
			SmoothToNeighbors();
		}

		// Hide smoothing line if no smoothing will be down.
		if (extraStats.smoothForce <= 0 || membranePrevious == null)
		{
			smoothCornerLine1.SetVertexCount(0);
			smoothCornerLine2.SetVertexCount(0);
		}
	}

	public override void BreakBond(bool quickDestroy = false)
	{
		if (!quickDestroy)
		{
			if (!deconstructing)
			{
				List<Membrane> ignoreNeighbors = new List<Membrane>();
				ignoreNeighbors.Add(membranePrevious);
				ignoreNeighbors.Add(membraneNext);

				// Check for bonds with the players on this membrane only (ignoring neighbors).
				MembraneLink bondedLink;
				if (IsBondMade(out bondedLink, Globals.Instance.player1.character.bondAttachable, ignoreNeighbors))
				{
					breakLinks.Add(bondedLink);
				}
				if (IsBondMade(out bondedLink, Globals.Instance.player2.character.bondAttachable, ignoreNeighbors))
				{
					breakLinks.Add(bondedLink);
				}
			}
			BreakMembrane(quickDestroy);
		}
		else
		{
			if (transform.parent != null)
			{
				transform.parent.SendMessage("MembraneBreaking", this, SendMessageOptions.DontRequireReceiver);
			}
			base.BreakBond(true);
		}
	}

	public void BreakMembrane(bool quickDestroy = false)
	{
		if (transform.parent != null && !deconstructing)
		{
			transform.parent.SendMessage("MembraneBreaking", this, SendMessageOptions.DontRequireReceiver);
		}

		if (extraStats.breakDelay < 0)
		{
			quickDestroy = true;
		}

		if (quickDestroy)
		{
			FinalizeBreak();
			return;
		}

		List<Membrane> ignoreNeighbors = new List<Membrane>();
		ignoreNeighbors.Add(membranePrevious);
		ignoreNeighbors.Add(membraneNext);
		BreakInnerBond(null, ignoreNeighbors);

		if (!deconstructing)
		{
			deconstructing = true;
			stats.manualLinks = true;
			float breakDelay = (extraStats.breakDelay == 0) ? 0.001f : extraStats.breakDelay;
			if (gameObject.activeInHierarchy)
			{
				StartCoroutine(DeconstructMembrane(breakDelay, false));
			}
		}

		if (extraStats.considerNeighborBonds)
		{
			List<Membrane> ignoreThis = new List<Membrane>();
			ignoreThis.Add(this);
			if (membranePrevious != null)
			{
				MembraneLink prevLinkBreak;
				membranePrevious.IsBondMade(out prevLinkBreak, null, ignoreThis);
				if (prevLinkBreak != null && prevLinkBreak.membrane != null)
				{
					prevLinkBreak.membrane.breakLinks.Add(prevLinkBreak);
					prevLinkBreak.membrane.BreakMembrane(quickDestroy);
				}
			}
		}
	}

	public void BreakMembraneWithNeighbor(Membrane brokenNeighbor, bool quickDestroy = false)
	{
		if (brokenNeighbor != membranePrevious && brokenNeighbor != membraneNext)
		{
			return;
		}

		if (brokenNeighbor == membranePrevious)
		{
			breakLinks.Add(links[0] as MembraneLink);
		}
		if (brokenNeighbor == membraneNext)
		{
			breakLinks.Add(links[links.Count - 1] as MembraneLink);
		}
		
		BreakMembrane(quickDestroy);
	}

	private IEnumerator DeconstructMembrane(float destroyInterval = 0, bool quickDestroy = false)
	{
		if (!quickDestroy)
		{
			// Prepare neighbors to fill be broken while this is deconstructing.
			/*if (extraStats.considerNeighborBonds)
			{
				if (membranePrevious != null)
				{
					membranePrevious.forceFullDetail = true;
				}
				if (membraneNext != null)
				{
					membraneNext.forceFullDetail = true;
				}
			}*/

			// Prepare to draw lines between each break, requiring one more line than break.
			breakLines = new List<LineRenderer>();
			breakLines.Add(attachment1.lineRenderer);
			breakLines.Add(attachment2.lineRenderer);
			for (int i = 2; i < breakLinks.Count + 1; i++)
			{
				GameObject newLineObject = (GameObject)Instantiate(breakLines[0].gameObject);
				LineRenderer newLineRenderer = newLineObject.GetComponent<LineRenderer>();
				newLineRenderer.transform.parent = breakLines[0].transform.parent;
				newLineRenderer.SetVertexCount(0);
				breakLines.Add(newLineRenderer);
			}

			// If no other break points have been stored, start breaking from the center.
			if (breakLinks.Count < 1)
			{
				breakLinks.Add(links[links.Count / 2] as MembraneLink);
			}

			// If an odd number of links exist, prepare to destroy the one closest to the breaking point.
			List<MembraneLink> linksToDestroy = new List<MembraneLink>();

			// Destroy successive pairs, spanning out from break, of links over time.
			int linkDestroyCount = 0;
			while (links.Count > 0)
			{
				// Move link about to break to final breaking stage.
				while (breakLinks.Count > 0)
				{
					linksToDestroy.Add(breakLinks[0]);
					breakLinks.RemoveAt(0);
				}

				// Create list of links to break after the upcoming group is broken.
				for (int i = 0; i < linksToDestroy.Count; i++)
				{
					if (linksToDestroy[i].linkPrevious != null)
					{
						breakLinks.Add(linksToDestroy[i].linkPrevious as MembraneLink);
					}
					if (linksToDestroy[i].linkNext != null)
					{
						breakLinks.Add(linksToDestroy[i].linkNext as MembraneLink);
					}
				}

				for (int i = 0; i < breakLinks.Count; i++)
				{
					if (breakLinks[i].toPreviousCollider != null)
					{
						breakLinks[i].toPreviousCollider.enabled = false;
					}
					if (breakLinks[i].toNextCollider != null)
					{
						breakLinks[i].toNextCollider.enabled = false;
					}
				}

				// After releasing and regaining control, destroy all links at the final break stage.
				yield return new WaitForSeconds(destroyInterval);
				while (linksToDestroy.Count > 0)
				{
					MembraneLink destroyeeLink = linksToDestroy[0];
					linksToDestroy.RemoveAt(0);
					DestroyLink(destroyeeLink);
				}

				linkDestroyCount++;
			}
		}

		// After all links have been broken finalize breaking procedure and destroy membrane.
		FinalizeBreak();
	}

	private void FinalizeBreak()
	{
		BondBreaking();

		// Drop reference of membrane from the bonded attachments.
		BondAttachable attachee1 = attachment1.attachee;
		BondAttachable attachee2 = attachment2.attachee;
		if (attachee1 != null) { attachee1.bonds.Remove(this); }
		if (attachee2 != null) { attachee2.bonds.Remove(this); }
		if (attachee1 != null) { attachee1.SendMessage("BondBroken", attachee2, SendMessageOptions.DontRequireReceiver); }
		if (attachee2 != null) { attachee2.SendMessage("BondBroken", attachee1, SendMessageOptions.DontRequireReceiver); }

		if (this != null && gameObject != null)
		{
			Destroy(gameObject);
		}
	}

	private void DestroyLink(MembraneLink linkToDestroy)
	{
		int indexToDestroy = links.IndexOf(linkToDestroy);
		if (linkToDestroy != null && indexToDestroy >= 0 && indexToDestroy < links.Count)
		{
			// Remove the neighboring links' references to the link about to be destroyed.
			if (linkToDestroy.linkPrevious != null)
			{
				if (linkToDestroy.linkPrevious.jointToNeighbor != null && linkToDestroy.linkPrevious.jointToNeighbor.connectedBody == linkToDestroy.body)
				{
					linkToDestroy.linkPrevious.jointToNeighbor.connectedBody = null;
					linkToDestroy.linkPrevious.jointToNeighbor.spring = 0;
				}
				linkToDestroy.linkPrevious.linkNext = null;
			}
			if (linkToDestroy.linkNext != null)
			{
				if (linkToDestroy.linkNext.jointToNeighbor != null && linkToDestroy.linkNext.jointToNeighbor.connectedBody == linkToDestroy.body)
				{
					linkToDestroy.linkNext.jointToNeighbor.connectedBody = null;
					linkToDestroy.linkNext.jointToNeighbor.spring = 0;
				}
				linkToDestroy.linkNext.linkPrevious = null;
			}

			bool destroyingStartLink = (linkToDestroy == attachment1.attachedLink);
			bool destroyingEndLink = (linkToDestroy == attachment2.attachedLink);

			Destroy(linkToDestroy.gameObject);
			links.RemoveAt(indexToDestroy);

			// If an end has been reached, begin breaking the neighboring memebrane.
			if (destroyingStartLink)
			{
				attachment1.attachedLink = null;
				smoothCornerLine1.SetVertexCount(0);
				smoothCornerLine2.SetVertexCount(0);
				BreakNeighbor(membranePrevious);
			}
			else if (destroyingEndLink)
			{
				attachment2.attachedLink = null;
				BreakNeighbor(membraneNext);
			}
			
		}
	}

	public override void RenderBond(float actualMidWidth, bool isCountEven)
	{
		// If not in the process of breaking, render lines the usual way.
		if (!deconstructing || breakLines == null || breakLines.Count == 0)
		{
			base.RenderBond(actualMidWidth, isCountEven);
			return;
		}

		// Remove all lines if one or fewer links exists.
		if (links.Count < 2)
		{
			for (int i = 0; i < breakLines.Count; i++)
			{
				breakLines[0].SetVertexCount(0);
			}
			return;
		}

		// Determine how many links are covered in each line, from break to break.
		int[] lineSizes = new int[breakLines.Count];
		int[] startingIndices = new int[breakLines.Count];
		startingIndices[0] = 0;
		for (int i = 0; i < breakLines.Count; i++)
		{
			breakLines[i].SetVertexCount(0);
			bool breakFound = false;
			for (int j = startingIndices[i]; j < links.Count && !breakFound; j++)
			{
				// If the next link does not exist, either a break or an endpoint has been found, so end line.
				if (links[j].linkNext == null)
				{
					lineSizes[i] = (j - startingIndices[i]) + 1;
					breakLines[i].SetVertexCount(lineSizes[i]);
					if (i < startingIndices.Length - 1)
					{
						startingIndices[i + 1] = j + 1;
					}
					breakFound = true;
				}
			}
		}

		// Draw lines at link positions, leaving broken links unrendered.
		for (int i = 0; i < breakLines.Count; i++)
		{
			for(int j = 0; j < lineSizes[i]; j++)
			{
				Vector3 vertPos = links[startingIndices[i] + j].transform.position;
				breakLines[i].SetPosition(j, vertPos);
			}
			breakLines[i].SetWidth(stats.endsWidth, stats.endsWidth);
		}
	}

	protected override void BondForming()
	{
		base.BondForming();

		if (endpointSpring < 0)
		{
			endpointSpring = internalBondStats.stats.attachSpring1;
		}

		// Setup endpoint links.
		MembraneLink startLink = attachment1.attachedLink as MembraneLink;
		if (startLink != null)
		{
			startLink.membrane = this;
			startPosition1 = startLink.transform.position;
		}
		MembraneLink endLink = attachment2.attachedLink as MembraneLink;
		if (endLink != null)
		{
			endLink.membrane = this;
			startPosition2 = endLink.transform.position;
		}

		// Setup faux links on attachees to allow them to be interacted with in place of endpoint links.
		attachment1FauxLink = attachment1.attachee.GetComponent<MembraneLink>();
		if (attachment1FauxLink != null)
		{
			attachment1FauxLink.membrane = this;
			if (attachment1FauxLink.bondAttachable != null)
			{
				attachment1FauxLink.bondAttachable.attachmentColor = attachmentColor;
				attachment1FauxLink.bondAttachable.bondOverrideStats = attachment1FauxLink.gameObject.GetComponent<BondStatsHolder>();
				attachment1FauxLink.bondAttachable.bondOverrideStats.stats.Overwrite(internalBondStats.stats, true);
				attachment1FauxLink.bondAttachable.bondOverrideStats.stats.attachSpring1 = endpointSpring;
			}
		}
		attachment2FauxLink = attachment2.attachee.GetComponent<MembraneLink>();
		if (attachment2FauxLink != null)
		{
			attachment2FauxLink.membrane = this;
			if (attachment2FauxLink.bondAttachable != null)
			{
				attachment2FauxLink.bondAttachable.attachmentColor = attachmentColor;
				attachment2FauxLink.bondAttachable.bondOverrideStats = attachment2FauxLink.gameObject.GetComponent<BondStatsHolder>();
				attachment2FauxLink.bondAttachable.bondOverrideStats.stats.Overwrite(internalBondStats.stats, true);
				attachment2FauxLink.bondAttachable.bondOverrideStats.stats.attachSpring1 = endpointSpring;
			}
		}
	}

	protected override void BondBreaking(bool quickDestroy = false)
	{
		base.BondBreaking();
		BreakNeighbor(membranePrevious, quickDestroy);
		BreakNeighbor(membraneNext, quickDestroy);

		// If desired, destroy attachments, unless they are used by neighbors.
		if (extraStats.breakDestroyAttachments)
		{
			if (attachment1 != null && attachment1.attachee != null)
			{
				if ((membranePrevious == null || (membranePrevious.attachment1.attachee != attachment1.attachee && membranePrevious.attachment2.attachee != attachment1.attachee)) &&
					(membraneNext == null || (membraneNext.attachment1.attachee != attachment1.attachee && membraneNext.attachment2.attachee != attachment1.attachee)))
				{
					Destroy(attachment1.attachee.gameObject);
				}
			}
			if (attachment2 != null && attachment2.attachee != null)
			{
				if ((membranePrevious == null || (membranePrevious.attachment1.attachee != attachment2.attachee && membranePrevious.attachment2.attachee != attachment2.attachee)) &&
					(membraneNext == null || (membraneNext.attachment2.attachee != attachment1.attachee && membraneNext.attachment2.attachee != attachment2.attachee)))
				{
					Destroy(attachment2.attachee.gameObject);
				}
			}
		}

		membranePrevious = membraneNext = null;

		if (transform.parent != null)
		{
			transform.parent.SendMessage("MembraneBroken", this, SendMessageOptions.DontRequireReceiver);
		}
	}

	private void BreakNeighbor(Membrane neighbor, bool quickDestroy = false)
	{
		if (neighbor == null || (neighbor != membranePrevious && neighbor != membraneNext))
		{
			return;
		}

		if (neighbor == membranePrevious)
		{
			membranePrevious = null;
		}
		if (neighbor == membraneNext)
		{
			membranePrevious = null;
		}

		if (neighbor.extraStats.breakWithNeighbors)
		{
			neighbor.BreakMembraneWithNeighbor(this, quickDestroy);
		}

		if (neighbor.membranePrevious == this)
		{
			neighbor.membranePrevious = null;
		}
		if (neighbor.membraneNext == this)
		{
			neighbor.membraneNext = null;
		}
	}

	protected override void LinkAdded(BondLink addedLink)
	{
		base.LinkAdded(addedLink);
		MembraneLink addedMembraneLink = addedLink as MembraneLink;
		if (addedMembraneLink != null)
		{
			addedMembraneLink.membrane = this;
			if (addedMembraneLink.bondAttachable != null)
			{
				addedMembraneLink.bondAttachable.attachmentColor = attachmentColor;
				addedMembraneLink.bondAttachable.bondOverrideStats = internalBondStats;
				ApplyShaping(addedMembraneLink);
			}
		}
	}
	
	public BondAttachable FindLinkAttachable(MembraneLink link)
	{
		if (link == null)
		{
			return null;
		}

		BondAttachable linkAttachable = link.bondAttachable;
		if (linkAttachable == null)
		{
			if (link == links[0] && attachment1FauxLink != null)
			{
				linkAttachable = attachment1FauxLink.bondAttachable;
			}
			else if (link == links[links.Count-1] && attachment2FauxLink != null)
			{
				linkAttachable = attachment2FauxLink.bondAttachable;
			}
		}

		return linkAttachable;
	}

	public bool IsBondMade(BondAttachable partner = null, List<Membrane> ignoreMembranes = null)
	{
		MembraneLink bondedLink;
		return IsBondMade(out bondedLink, partner, ignoreMembranes);
	}

	public bool IsBondMade(out MembraneLink bondedLink, BondAttachable partner = null, List<Membrane> ignoreMembranes = null)
	{
		bondedLink = null;
		bool bonded = false;
		if (attachment1FauxLink != null && attachment1FauxLink.bondAttachable != null && attachment1FauxLink.bondAttachable.IsBondMade(partner))
		{
			bondedLink = links[0] as MembraneLink;
			bonded = true;
		}
		else if (attachment2FauxLink != null && attachment2FauxLink.bondAttachable != null && attachment2FauxLink.bondAttachable.IsBondMade(partner))
		{
			bondedLink = links[links.Count - 1] as MembraneLink;
			bonded = true;
		}
		else
		{
			for (int i = 0; i < links.Count && !bonded; i++)
			{
				MembraneLink membraneLink = links[i] as MembraneLink;
				if (membraneLink != null && membraneLink.bondAttachable != null && membraneLink.bondAttachable.IsBondMade(partner))
				{
					bondedLink = links[i] as MembraneLink;
					bonded = true;
				}
			}
		}
		

		if (!bonded && extraStats.considerNeighborBonds)
		{
			if (ignoreMembranes == null)
			{
				ignoreMembranes = new List<Membrane>();
			}
			ignoreMembranes.Add(this);

			bool previousBondMade = false, nextBondMade = false;
			if (membranePrevious != null && !ignoreMembranes.Contains(membranePrevious))
			{
				previousBondMade = membranePrevious.IsBondMade(out bondedLink, partner, ignoreMembranes);
			}
			if (membraneNext != null && !ignoreMembranes.Contains(membraneNext))
			{
				nextBondMade = membraneNext.IsBondMade(out bondedLink, partner, ignoreMembranes);
			}
			bonded = bonded || previousBondMade || nextBondMade;
		}

		return bonded;
	}

	public GameObject[] BondedObjectsWithTag(string targetTag, List<GameObject> knownBondedObjects = null, List<Membrane> ignoreMembranes = null)
	{
		GameObject[] bondedObjects;
		if (knownBondedObjects == null)
		{
			knownBondedObjects = new List<GameObject>();
		}

		// Find all link bonds with objects with the given tag.
		for (int i = 0; i < links.Count; i++)
		{
			MembraneLink link = links[i] as MembraneLink;
			BondAttachable linkAttachable = FindLinkAttachable(link);
			if (linkAttachable != null)
			{
				for (int j = 0; j < linkAttachable.bonds.Count; j++)
				{
					GameObject partner = linkAttachable.bonds[j].OtherPartner(linkAttachable).gameObject;
					if (partner.tag == targetTag)
					{
						knownBondedObjects.Add(partner);
					}
				}
			}
			
		}

		// If desired, check bonds of neighbors.
		if (extraStats.considerNeighborBonds)
		{
			if (ignoreMembranes == null)
			{
				ignoreMembranes = new List<Membrane>();
			}
			ignoreMembranes.Add(this);

			if (membranePrevious != null && !ignoreMembranes.Contains(membranePrevious))
			{
				membranePrevious.BondedObjectsWithTag(targetTag, knownBondedObjects, ignoreMembranes);
			}
			if (membraneNext != null && !ignoreMembranes.Contains(membraneNext))
			{
				membraneNext.BondedObjectsWithTag(targetTag, knownBondedObjects, ignoreMembranes);
			}
		}

		// Store bonded object for output.
		bondedObjects = new GameObject[knownBondedObjects.Count];
		for (int i = 0; i < bondedObjects.Length; i++)
		{
			bondedObjects[i] = knownBondedObjects[i];
		}

		return bondedObjects;
	}

	public void BreakInnerBond(BondAttachable partner, List<Membrane> ignoreMembranes = null)
	{
		if (attachment1FauxLink != null && attachment1FauxLink.bondAttachable != null && attachment1FauxLink.bondAttachable.IsBondMade(partner))
		{
			attachment1FauxLink.bondAttachable.BreakBond(partner);
		}
		if (attachment2FauxLink != null && attachment2FauxLink.bondAttachable != null && attachment2FauxLink.bondAttachable.IsBondMade(partner))
		{
			attachment2FauxLink.bondAttachable.BreakBond(partner);
		}
		for (int i = 0; i < links.Count; i++)
		{
			MembraneLink membraneLink = links[i] as MembraneLink;
			if (membraneLink != null && membraneLink.bondAttachable != null && membraneLink.bondAttachable.IsBondMade(partner))
			{
				membraneLink.bondAttachable.BreakBond(partner);
			}
		}

		if (extraStats.considerNeighborBonds)
		{
			if (ignoreMembranes == null)
			{
				ignoreMembranes = new List<Membrane>();
			}
			ignoreMembranes.Add(this);

			if (membranePrevious != null && !ignoreMembranes.Contains(membranePrevious))
			{
				membranePrevious.BreakInnerBond(partner, ignoreMembranes);
			}
			if (membraneNext != null && !ignoreMembranes.Contains(membraneNext))
			{
				membraneNext.BreakInnerBond(partner, ignoreMembranes);
			}
		}
	}

	private ShapingPoint[] NearestShapingPoints(MembraneLink link)
	{
		ShapingPoint[] nearPoints = new ShapingPoint[3]{null, null, null};

		if (shapingPoints.Count > 0)
		{
			int[] nearIndex = new int[3]{0, -1, -1};
			nearPoints[0] = shapingPoints[nearIndex[0]];
			float[] nearSqrDist = new float[3]{(nearPoints[0].transform.position - link.transform.position).sqrMagnitude, -1, -1};
			for (int i = 1; i < shapingPoints.Count; i++)
			{
				float sqrDist = (shapingPoints[i].transform.position - link.transform.position).sqrMagnitude;
				// Get the nearest shaping point, only using the attachment points if no others exist.
				int beatsIndex = -1;
				if (sqrDist < nearSqrDist[2] || (i > 1 && nearIndex[2] <= 1))
				{
					beatsIndex = 2;
					if (sqrDist < nearSqrDist[1] || (i > 1 && nearIndex[1] <= 1))
					{
						beatsIndex = 1;
						if (sqrDist < nearSqrDist[0] || (i > 1 && nearIndex[0] <= 1))
						{
							beatsIndex = 0;
						}
					}
				}

				if (beatsIndex >= 0)
				{
					for(int j = nearPoints.Length - 2; j >= beatsIndex; j--)
					{
						nearIndex[j + 1] = nearIndex[j];
						nearSqrDist[j + 1] = nearSqrDist[j];
						nearPoints[j + 1] = nearPoints[j];
					}

					nearIndex[beatsIndex] = i;
					nearSqrDist[beatsIndex] = sqrDist;
					nearPoints[beatsIndex] = shapingPoints[i];
				}
			}
		}
		return nearPoints;
	}

	private void ApplyShaping(MembraneLink membraneLink)
	{
		if (membraneLink != null && membraneLink.jointsShaping != null)
		{
			ShapingPoint[] shapingPoints = NearestShapingPoints(membraneLink);
			if (shapingPoints != null && shapingPoints[0] != null)
			{
				if (shapingPoints[1] == null)
				{
					shapingPoints[1] = shapingPoints[0];
				}
				if (shapingPoints[2] == null)
				{
					shapingPoints[2] = shapingPoints[0];
				}

				for (int i = 0; i < shapingPoints.Length && i < membraneLink.jointsShaping.Length; i++)
				{
					Rigidbody connectedBody = shapingPoints[i].body;
					float shapingForce = extraStats.defaultShapingForce;
					if (connectedBody == null || (connectedBody == links[0].body || connectedBody == links[links.Count - 1].body))
					{
						shapingForce = 0;
					}
					else if (shapingPoints[i].lodShapingForce >= 0)
					{
						shapingForce = shapingPoints[i].lodShapingForce;
					}

					shapingForce /= Mathf.Pow(2, (i + 1) / 2);

					membraneLink.jointsShaping[i].connectedBody = connectedBody;
					membraneLink.jointsShaping[i].spring = shapingForce;
				}

				for (int i = shapingPoints.Length; i < membraneLink.jointsShaping.Length; i++)
				{
					membraneLink.jointsShaping[i].connectedBody = null;
					membraneLink.jointsShaping[i].spring = 0;
				}
			}
			else
			{
				for (int i = 0; i < membraneLink.jointsShaping.Length; i++)
				{
					membraneLink.jointsShaping[i].connectedBody = null;
					membraneLink.jointsShaping[i].spring = 0;
				}
			}
		}
	}

	private void SmoothToNeighbors()
	{
		if (membranePrevious != null && (attachment1.attachedLink != null && attachment1.attachedLink.linkNext != null) &&
			(membranePrevious.attachment2.attachedLink != null && membranePrevious.attachment2.attachedLink.linkPrevious != null))
		{
			// Pull the first endpoint of this membrane and the last endpoint of the previous towards an average position that smooths transition between the two.
			Vector3 thisNearEndPos = attachment1.attachedLink.linkNext.transform.position;
			Vector3 prevNearEndPos = membranePrevious.attachment2.attachedLink.linkPrevious.transform.position;
			Vector3 desiredSmoothPos = (thisNearEndPos + prevNearEndPos) / 2;
			attachment1.attachee.body.AddForce((desiredSmoothPos - attachment1.position).normalized * extraStats.smoothForce);
			membranePrevious.attachment2.attachee.body.AddForce((desiredSmoothPos - membranePrevious.attachment2.position).normalized * membranePrevious.extraStats.smoothForce);

			// Pull the endpoints back towards their starting positions when accounting for level of detail.
			attachment1.attachee.body.AddForce((startPosition1 - attachment1.position) * (fullDetailSmoothForce - extraStats.smoothForce));
			membranePrevious.attachment2.attachee.body.AddForce((membranePrevious.startPosition2 - membranePrevious.attachment2.position) * (membranePrevious.fullDetailSmoothForce - membranePrevious.extraStats.smoothForce));

			// Ensure that the endpoints stay connected.
			if (jointToPrevious == null)
			{
				jointToPrevious = attachment1.attachee.gameObject.AddComponent<FixedJoint>();
				jointToPrevious.connectedBody = membranePrevious.attachment2.attachee.body;
			}

			// Draw a line between the endpoints to hide the gap between ending corners.
			DrawLineFromPrevious();
		}
	}

	private void DrawLineFromPrevious()
	{
		if (membranePrevious == null)
		{
			return;
		}

		// Find the needed end points and their adjacent links on each membrane.
		BondLink thisEnd = attachment1.attachedLink;
		BondLink prevEnd = membranePrevious.attachment2.attachedLink;
		Vector3 thisNearEndPos = thisEnd.linkNext.transform.position;
		Vector3 prevNearEndPos = prevEnd.linkPrevious.transform.position;

		// Compute the directions from the adjacent links to the endpoints.
		Vector3 thisEndDir = (thisEnd.transform.position - thisNearEndPos).normalized;
		Vector3 prevEndDir = (prevEnd.transform.position - prevNearEndPos).normalized;

		// Find the needed corners of this membrane's line.
		Vector3 thisEndPerp = Vector3.Cross(thisEndDir, Vector3.forward) * (stats.endsWidth / 2);
		Vector3 thisEndCorner1 = thisEnd.transform.position + thisEndPerp;
		Vector3 thisEndCorner2 = thisEnd.transform.position - thisEndPerp;

		// Find the needed corners of previous membrane's line.
		Vector3 prevEndPerp = Vector3.Cross(prevEndDir, Vector3.forward) * (membranePrevious.stats.endsWidth / 2);
		Vector3 prevEndCorner1 = prevEnd.transform.position - prevEndPerp;
		Vector3 prevEndCorner2 = prevEnd.transform.position + prevEndPerp;

		// Prepare the line drawing positions and widths.
		Vector3 smoothLineStart1 = (prevEndCorner1 + thisEndCorner1) / 2;
		Vector3 smoothLineStart2 = (prevEndCorner2 + thisEndCorner2) / 2;
		Vector3 smoothLineMidpoint = (prevEnd.transform.position + thisEnd.transform.position) / 2;
		float smoothLineWidth1 = (prevEndCorner1 - thisEndCorner1).magnitude;
		float smoothLineWidth2 = (prevEndCorner2 - thisEndCorner2).magnitude;

		// Draw a line from the space between the first two corners to the space between the endpoints.
		smoothCornerLine1.SetVertexCount(2);
		smoothCornerLine1.SetPosition(0, smoothLineStart1);
		smoothCornerLine1.SetPosition(1, smoothLineMidpoint);
		smoothCornerLine1.SetColors(membranePrevious.attachmentColor, attachmentColor);
		smoothCornerLine1.SetWidth(smoothLineWidth1, smoothLineWidth1);

		// Draw a line from the space between the second two corners to the space between the endpoints.
		smoothCornerLine2.SetVertexCount(2);
		smoothCornerLine2.SetPosition(0, smoothLineStart2);
		smoothCornerLine2.SetPosition(1, smoothLineMidpoint);
		smoothCornerLine2.SetColors(membranePrevious.attachmentColor, attachmentColor);
		smoothCornerLine2.SetWidth(smoothLineWidth1, smoothLineWidth2);
	}

	public Vector3 NearestNeighboredPoint(Vector3 checkPoint)
	{
		MembraneLink nearestLink;
		return NearestNeighboredPoint(checkPoint, out nearestLink);
	}

	public Vector3 NearestNeighboredPoint(Vector3 checkPoint, out MembraneLink nearestLink)
	{
		BondLink nearestBondLink;
		Vector3 nearestPoint = base.NearestPoint(checkPoint, out nearestBondLink);
		float nearestSqrDist = (nearestPoint - checkPoint).sqrMagnitude;

		// Check if preious neighbor is closer to the checked point.
		if (membranePrevious != null)
		{
			BondLink nearestLinkPrevious;
			Vector3 nearestPointPrevious = membranePrevious.NearestPoint(checkPoint, out nearestLinkPrevious);
			float previousSqrDist = (nearestPointPrevious - checkPoint).sqrMagnitude;
			if (previousSqrDist < nearestSqrDist)
			{
				nearestPoint = nearestPointPrevious;
				nearestSqrDist = previousSqrDist;
				nearestBondLink = nearestLinkPrevious;
			}
		}

		// Check if preious neighbor is closer to the checked point.
		if (membraneNext != null)
		{
			BondLink nearestLinkNext;
			Vector3 nearestPointNext = membraneNext.NearestPoint(checkPoint, out nearestLinkNext);
			float nextSqrDist = (nearestPointNext - checkPoint).sqrMagnitude;
			if (nextSqrDist < nearestSqrDist)
			{
				nearestPoint = nearestPointNext;
				nearestSqrDist = nextSqrDist;
				nearestBondLink = nearestLinkNext;
			}
		}

		nearestLink = nearestBondLink as MembraneLink;

		return nearestPoint;
	}

	protected override float SetLevelOfDetail()
	{
		float detailFraction = base.SetLevelOfDetail();
		extraStats.smoothForce = fullDetailSmoothForce * detailFraction;
		return detailFraction;
	}
}



[System.Serializable]
public class MembraneStats
{
	public float defaultShapingForce = 5;
	public bool bondOnContact = true;
	public bool bondOnFluff = true;
	public LayerMask ignoreBondingLayers;
	public bool breakWithNeighbors = true;
	public bool breakDestroyAttachments = true;
	public bool considerNeighborBonds = true;
	public float smoothForce = 10;
	public float breakDelay = 0;

	public MembraneStats(MembraneStats original)
	{
		this.defaultShapingForce = original.defaultShapingForce;
		this.bondOnContact = original.bondOnContact;
		this.bondOnFluff = original.bondOnFluff;
		this.ignoreBondingLayers = original.ignoreBondingLayers;
		this.breakWithNeighbors = original.breakWithNeighbors;
		this.breakDestroyAttachments = original.breakDestroyAttachments;
		this.considerNeighborBonds = original.considerNeighborBonds;
		this.smoothForce = original.smoothForce;
		this.breakDelay = original.breakDelay;
	}

	public void Overwrite(MembraneStats replacement, bool fullOverwrite = false)
	{
		if (replacement == null)
		{
			return;
		}

		if (fullOverwrite || replacement.defaultShapingForce >= 0)		{	this.defaultShapingForce = replacement.defaultShapingForce;		}
		this.bondOnContact = replacement.bondOnContact;
		this.bondOnFluff = replacement.bondOnFluff;
		this.ignoreBondingLayers = replacement.ignoreBondingLayers;
		this.breakWithNeighbors = replacement.breakWithNeighbors;
		this.breakDestroyAttachments = replacement.breakDestroyAttachments;
		this.considerNeighborBonds = replacement.considerNeighborBonds;
		if (fullOverwrite || replacement.smoothForce >= 0)				{	this.smoothForce = replacement.smoothForce;						}
		if (fullOverwrite || replacement.breakDelay >= 0)				{	this.breakDelay = replacement.breakDelay;						}
	}
}
