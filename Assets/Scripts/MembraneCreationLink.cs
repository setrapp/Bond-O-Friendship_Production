using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MembraneCreationLink : MonoBehaviour {

	public MembraneWall membraneWallTrigger;
	public List<MembraneWall> linkedMembranes;
	public bool createLinkedOnBreak = true;
	public IslandContainer linkedIslandContainer;

	private void MembraneWallBreaking(MembraneWall brokenMembrane)
	{
		if (brokenMembrane != null && brokenMembrane == membraneWallTrigger && createLinkedOnBreak)
		{
			CreateMembranes();
		}
	}

	public void CreateMembranes()
	{
		for (int i = 0; i < linkedMembranes.Count; i++)
		{
			if (linkedMembranes[i] != null && linkedMembranes[i].membraneCreator != null && linkedMembranes[i].membraneCreator.createdBond == null)
			{
				linkedMembranes[i].CreateWall();
			}
		}
	}

	public void BreakMembranes()
	{
		for (int i = 0; i < linkedMembranes.Count; i++)
		{
			if (linkedMembranes[i] != null && linkedMembranes[i].membraneCreator != null && linkedMembranes[i].membraneCreator.createdBond != null)
			{
				linkedMembranes[i].membraneCreator.createdBond.BreakBond();
			}
		}
	}
}
