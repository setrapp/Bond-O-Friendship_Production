using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Island : MonoBehaviour {
	public IslandID islandId;
	public IslandContainer container;
	public LevelHelper levelHelper;

	void Start()
	{
		if (levelHelper == null)
		{
			levelHelper = GetComponentInChildren<LevelHelper>();
			if (levelHelper == null)
			{
				Debug.LogError("Level Helper not attached to " + ((container != null) ? container.name + "'s island" : "an island. Please add one as child of the island level."));
			}
		}
	}
}

public enum IslandID
{
	NONE = 0,
	TUTORIAL,
	HARMONY_A,
	INTIMACY_A,
	ASYMMETRY_A
};