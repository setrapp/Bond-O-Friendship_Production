using UnityEngine;
using System.Collections;

public class AutoMembrane : AutoBond {
	public GameObject shapingPointContainer;
	public MembraneStatsHolder membraneOverrideStats;

	protected override void CreateBond()
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
				createdMembrane.extraStats = membraneOverrideStats.stats;
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
		}
	}
}
