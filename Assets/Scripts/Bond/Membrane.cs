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

	protected override void PostUpdate()
	{
		base.PostUpdate();
		for (int i = 1; i < links.Count - 1; i++)
		{
			MembraneLink membraneLink = links[i] as MembraneLink;
			ApplyShaping(membraneLink);
		}

		if (extraStats.smoothWithNeighbors)
		{
			SmoothToNeighbors();
		}
	}

	protected override void BondForming()
	{
		base.BondForming();
		/*MembraneLink membraneLinkStart = links[0] as MembraneLink;
		MembraneLink membraneLinkEnd = links[1] as MembraneLink;
		if (membraneLinkStart != null)
		{
			mem//add pullable bonds to either starting links or attachements
		}*/
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

	public bool IsBondMade(BondAttachable partner, List<Membrane> ignoreMembranes = null)
	{
		bool bonded = false;
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

	private ShapingPoint NearestShapingPoint(MembraneLink link)
	{
		ShapingPoint nearPoint = null;

		if (shapingPoints.Count > 0)
		{
			int nearIndex = 0;
			nearPoint = shapingPoints[nearIndex];
			float nearSqrDist = (nearPoint.transform.position - link.transform.position).sqrMagnitude;
			for (int i = 1; i < shapingPoints.Count; i++)
			{
				float sqrDist = (shapingPoints[i].transform.position - link.transform.position).sqrMagnitude;
				// Get the nearest shaping point, only using the attachment points if no others exist.
				if (sqrDist < nearSqrDist || (i > 1 && nearIndex <= 1))
				{
					nearIndex = i;
					nearSqrDist = sqrDist;
					nearPoint = shapingPoints[nearIndex];
				}
			}
		}
		
		return nearPoint;
	}

	private void ApplyShaping(MembraneLink membraneLink)
	{
		if (membraneLink != null && membraneLink.jointShaping != null)
		{
			ShapingPoint shapingPoint = NearestShapingPoint(membraneLink);
			if (shapingPoint != null)
			{
				membraneLink.jointShaping.connectedBody = shapingPoint.body;
				float shapingForce = extraStats.defaultShapingForce;
				if (shapingPoint.body == null || shapingPoint.body == links[0].body || shapingPoint.body == links[links.Count - 1].body)
				{
					shapingForce = 0;
				}
				else if (shapingPoint.shapingForce >= 0)
				{
					shapingForce = shapingPoint.shapingForce;
				}
				membraneLink.jointShaping.spring = shapingForce;
			}
			else
			{
				membraneLink.jointShaping.connectedBody = null;
				membraneLink.jointShaping.spring = 0;
			}
		}
	}

	private void SmoothToNeighbors()
	{
		if (membranePrevious != null && links.Count > 2 && membranePrevious.links.Count > 2)
		{
			attachment1.attachee.transform.position = (links[0].jointNext.connectedBody.transform.position + membranePrevious.links[membranePrevious.links.Count - 1].jointPrevious.connectedBody.transform.position) / 2;
		}
		if (membraneNext != null && links.Count > 2 && membraneNext.links.Count > 2)
		{
			attachment2.attachee.transform.position = (links[links.Count - 1].jointPrevious.connectedBody.transform.position + membraneNext.links[0].jointNext.connectedBody.transform.position) / 2;
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
		if (membranePrevious != null)
		{
			Gizmos.DrawLine(links[0].jointNext.connectedBody.transform.position, (links[0].jointNext.connectedBody.transform.position + membranePrevious.links[membranePrevious.links.Count - 1].jointPrevious.connectedBody.transform.position) / 2);
		}
		if (membraneNext != null)
		{
			Gizmos.DrawLine(links[links.Count - 1].jointPrevious.connectedBody.transform.position, (links[links.Count - 1].jointPrevious.connectedBody.transform.position + membraneNext.links[0].jointNext.connectedBody.transform.position) / 2);
		}
		
	}
}



[System.Serializable]
public class MembraneStats
{
	public float defaultShapingForce = 5;
	public bool breakWithNeighbors = true;
	public bool considerNeighborBonds = true;
	[HideInInspector]
	public bool smoothWithNeighbors = false;
}
