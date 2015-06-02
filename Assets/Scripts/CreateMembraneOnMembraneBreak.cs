using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateMembraneOnMembraneBreak : MonoBehaviour {

	public MembraneWall membraneWallTrigger;
	public List<MembraneWall> membranesToCreate;

	private void MembraneBroken(MembraneWall brokenMembrane)
	{
		if (brokenMembrane != null && brokenMembrane == membraneWallTrigger)
		{
			for (int i = 0; i < membranesToCreate.Count; i++)
			{
				if (membranesToCreate[i] != null && membranesToCreate[i].membraneCreator != null && membranesToCreate[i].membraneCreator.createdBond == null)
				{
					membranesToCreate[i].CreateWall();
				}
			}
		}
	}
}
