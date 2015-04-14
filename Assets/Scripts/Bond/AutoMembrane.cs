using UnityEngine;
using System.Collections;

public class AutoMembrane : AutoBond {
	public MembraneStatsHolder membraneOverrideStats;
	public BondStatsHolder internalBondStats;
	public GameObject shapingPointContainer;
	public AutoMembrane neighborPrevious;
	public AutoMembrane neighborNext;
	private Membrane membranePrevious;
	private Membrane membraneNext;

	public override Bond CreateBond()
	{
		base.CreateBond();

		Membrane createdMembrane = createdBond as Membrane;
		if (createdMembrane != null)
		{
			// Override extra membrane stats.
			if (membraneOverrideStats == null)
			{
				membraneOverrideStats = GetComponent<MembraneStatsHolder>();
			}
			if (membraneOverrideStats != null && membraneOverrideStats.stats != null)
			{
				createdMembrane.extraStats.Overwrite(membraneOverrideStats.stats, true);
			}

			// Override membrane's internal bonding stats.
			if (internalBondStats != null && internalBondStats.stats != null)
			{
				createdMembrane.internalBondStats.stats.Overwrite(internalBondStats.stats, true);
			}

			// Add shaping points to membrane.
			if (shapingPointContainer != null)
			{
				for (int i = 0; i < shapingPointContainer.transform.childCount; i++)
				{
					Transform additionalObject = shapingPointContainer.transform.GetChild(i);
					ShapingPoint additionalPoint = additionalObject.GetComponent<ShapingPoint>();
					if (additionalPoint != null)
					{
						createdMembrane.shapingPoints.Add(additionalPoint);
					}
				}
			}

			// Attach neighboring membranes to created membrane.
			if (membranePrevious != null)
			{
				createdMembrane.membranePrevious = membranePrevious;
				membranePrevious = null;
			}
			if (membraneNext != null)
			{
				createdMembrane.membraneNext = membraneNext;
				membraneNext = null;
			}

			// Attach created membrane to neighboring membranes.
			if (neighborPrevious != null && neighborPrevious != this)
			{
				neighborPrevious.AttachMembraneNext(createdMembrane);
			}
			if (neighborNext != null && neighborNext != this)
			{
				neighborNext.AttachMembranePrevious(createdMembrane);
			}
		}

		return createdMembrane;
	}

	public void AttachMembranePrevious(Membrane previous)
	{
		Membrane createdMembrane = createdBond as Membrane;
		if (createdMembrane != null)
		{
			createdMembrane.membranePrevious = previous;
		}
		else
		{
			membranePrevious = previous;
		}
	}

	public void AttachMembraneNext(Membrane next)
	{
		Membrane createdMembrane = createdBond as Membrane;
		if (createdMembrane != null)
		{
			createdMembrane.membraneNext = next;
		}
		else
		{
			membraneNext = next;
		}
	}
}
