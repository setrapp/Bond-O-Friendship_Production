using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConvertToStreamOrb : MonoBehaviour {

	public List<StreamReaction> requiredReactions;
	public List<SpawnBud> budsToSpawn;
	private bool spawned = false;

	void Update()
	{
		if (!spawned)
		{
			bool allDone = true;
			for (int i = 0; i < requiredReactions.Count; i++)
			{
				if (requiredReactions[i].reactionProgress < 1)
				{
					allDone = false;
				}
			}

			if (allDone)
			{
				for (int i = 0; i < requiredReactions.Count; i++)
				{
					requiredReactions[i].transform.parent.GetComponent<Renderer>().enabled = false;
				}
				for (int i = 0; i < budsToSpawn.Count; i++)
				{
					budsToSpawn[i].SpawnOrb();
				}
				spawned = true;
			}
		}
	}
}
