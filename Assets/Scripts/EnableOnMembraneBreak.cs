using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnableOnMembraneBreak : MonoBehaviour {

	public MembraneWall membraneWall;
	public bool toEnabled = true;
	public List<GameObject> enablees;

	private void MembraneBroken(MembraneWall brokenMembrane)
	{
		if (brokenMembrane != null && brokenMembrane == membraneWall)
		{
			for (int i = 0; i < enablees.Count; i++)
			{
				if (enablees[i] != null)
				{
					enablees[i].SetActive(toEnabled);
				}
			}
		}
	}
}
