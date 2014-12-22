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
			breakMembrane.BreakBond();
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
			breakMembrane.BreakBond();
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

	public bool IsBondMade(BondAttachable partner)
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
		return bonded;
	}

	public void BreakInnerBond(BondAttachable partner)
	{
		for (int i = 0; i < links.Count; i++)
		{
			MembraneLink membraneLink = links[i] as MembraneLink;
			if (membraneLink != null && membraneLink.bondAttachable != null && membraneLink.bondAttachable.IsBondMade(partner))
			{
				membraneLink.bondAttachable.BreakBound(partner);
			}
		}
	}

	private ShapingPoint NearestShapingPoint(MembraneLink link)
	{
		ShapingPoint nearPoint = null;

		if (shapingPoints.Count > 0)
		{
			nearPoint = shapingPoints[0];
			float nearSqrDist = (nearPoint.transform.position - link.transform.position).sqrMagnitude;
			for (int i = 1; i < shapingPoints.Count; i++)
			{
				float sqrDist = (shapingPoints[i].transform.position - link.transform.position).sqrMagnitude;
				if (sqrDist < nearSqrDist)
				{
					nearSqrDist = sqrDist;

					nearPoint = shapingPoints[i];
				}
			}
		}
		
		return nearPoint;
	}

	private void ApplyShaping(MembraneLink membraneLink)
	{
		if (membraneLink != null)
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
}

[System.Serializable]
public class MembraneStats
{
	public float defaultShapingForce = 5;
}
