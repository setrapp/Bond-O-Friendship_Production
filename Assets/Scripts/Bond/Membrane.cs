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

	protected override void Start()
	{
		base.Start();
		fullDetailShapingForce = extraStats.defaultShapingForce;
		fullDetailSmoothForce = extraStats.smoothForce;
	}

	protected override void Update()
	{
		base.Update();

		for (int i = 1; i < links.Count - 1; i++)
		{
			MembraneLink membraneLink = links[i] as MembraneLink;
			ApplyShaping(membraneLink);
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

	public override void BreakBond()
	{
		base.BreakBond();
	}

	protected override void BondForming()
	{
		base.BondForming();

		if (endpointSpring < 0)
		{
			endpointSpring = internalBondStats.stats.attachSpring1;
		}

		// Setup endpoint links.
		MembraneLink startLink = links[0] as MembraneLink;
		if (startLink != null)
		{
			startLink.membrane = this;
			startPosition1 = startLink.transform.position;
		}
		MembraneLink endLink = links[links.Count-1] as MembraneLink;
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

	protected override void BondBreaking()
	{
		base.BondBreaking();
		if (membranePrevious != null)
		{
			if (membranePrevious.membranePrevious == this)
			{
				membranePrevious.membranePrevious = null;
			}
			if (membranePrevious.membraneNext == this)
			{
				membranePrevious.membraneNext = null;
			}
			Membrane breakMembrane = membranePrevious;
			membranePrevious = null;
			if (breakMembrane.extraStats.breakWithNeighbors)
			{
				breakMembrane.BreakBond();
			}
		}
		if (membraneNext != null)
		{
			if (membraneNext.membranePrevious == this)
			{
				membraneNext.membranePrevious = null;
			}
			if (membraneNext.membraneNext == this)
			{
				membraneNext.membraneNext = null;
			}
			Membrane breakMembrane = membraneNext;
			membraneNext = null;
			if (breakMembrane.extraStats.breakWithNeighbors)
			{
				breakMembrane.BreakBond();
			}
		}

		if (extraStats.breakDestroyAttachments)
		{
			if (attachment1 != null && attachment1.attachee != null)
			{
				Destroy(attachment1.attachee.gameObject);
			}
			if (attachment2 != null && attachment2.attachee != null)
			{
				Destroy(attachment2.attachee.gameObject);
			}
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
		bool bonded = false;
		if (attachment1FauxLink != null && attachment1FauxLink.bondAttachable != null && attachment1FauxLink.bondAttachable.IsBondMade(partner))
		{
			bonded = true;
		}
		else if (attachment2FauxLink != null && attachment2FauxLink.bondAttachable != null && attachment2FauxLink.bondAttachable.IsBondMade(partner))
		{
			bonded = true;
		}
		for (int i = 0; i < links.Count && !bonded; i++)
		{
			MembraneLink membraneLink = links[i] as MembraneLink;
			if (membraneLink != null && membraneLink.bondAttachable != null && membraneLink.bondAttachable.IsBondMade(partner))
			{
				bonded = true;
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
			if(membranePrevious != null && !ignoreMembranes.Contains(membranePrevious))
			{
				previousBondMade = membranePrevious.IsBondMade(partner, ignoreMembranes);
			}
			if (membraneNext != null && !ignoreMembranes.Contains(membraneNext))
			{
				nextBondMade = membraneNext.IsBondMade(partner, ignoreMembranes);
			}
			bonded = bonded || previousBondMade || nextBondMade;
		}

		return bonded;
	}

	public void BreakInnerBond(BondAttachable partner, List<Membrane> ignoreMembranes = null)
	{
		if (attachment1FauxLink != null && attachment1FauxLink.bondAttachable != null && attachment1FauxLink.bondAttachable.IsBondMade(partner))
		{
			attachment1FauxLink.bondAttachable.BreakBound(partner);
		}
		if (attachment2FauxLink != null && attachment2FauxLink.bondAttachable != null && attachment2FauxLink.bondAttachable.IsBondMade(partner))
		{
			attachment2FauxLink.bondAttachable.BreakBound(partner);
		}
		for (int i = 0; i < links.Count; i++)
		{
			MembraneLink membraneLink = links[i] as MembraneLink;
			if (membraneLink != null && membraneLink.bondAttachable != null && membraneLink.bondAttachable.IsBondMade(partner))
			{
				membraneLink.bondAttachable.BreakBound(partner);
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
		if (membranePrevious != null && links.Count > 2 && membranePrevious.links.Count > 2)
		{
			// Pull the first endpoint of this membrane and the last endpoint of the previous towards an average position that smooths transition between the two.
			Vector3 thisNearEndPos = links[0].linkNext.transform.position;
			Vector3 prevNearEndPos = membranePrevious.links[membranePrevious.links.Count - 1].linkPrevious.transform.position;
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
		BondLink thisEnd = links[0];
		BondLink prevEnd = membranePrevious.links[membranePrevious.links.Count - 1];
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
	public bool breakWithNeighbors = true;
	public bool breakDestroyAttachments = true;
	public bool considerNeighborBonds = true;
	public float smoothForce = 10;

	public MembraneStats(MembraneStats original)
	{
		this.defaultShapingForce = original.defaultShapingForce;
		this.bondOnContact = original.bondOnContact;
		this.bondOnFluff = original.bondOnFluff;
		this.breakWithNeighbors = original.breakWithNeighbors;
		this.considerNeighborBonds = original.considerNeighborBonds;
		this.smoothForce = original.smoothForce;
	}

	public void Overwrite(MembraneStats replacement, bool fullOverwrite = false)
	{
		if (replacement == null)
		{
			return;
		}

		if (fullOverwrite || replacement.defaultShapingForce >= 0)	{	this.defaultShapingForce = replacement.defaultShapingForce;	}
		this.bondOnContact = replacement.bondOnContact;
		this.bondOnFluff = replacement.bondOnFluff;
		this.breakWithNeighbors = replacement.breakWithNeighbors;
		this.considerNeighborBonds = replacement.considerNeighborBonds;
		if (fullOverwrite || replacement.smoothForce >= 0)				{	this.smoothForce = replacement.smoothForce;				}
	}
}
