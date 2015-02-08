using UnityEngine;
using System.Collections;

public class IslandContainer : MonoBehaviour {
	public IslandID islandId;
	public Island island;
	public string islandSceneName;
	public MembraneShell atmosphere;
	public Vector3 spawnOffset;
	public bool spawnOnStart = false; // TODO this should be handled in main menu.

	void Start()
	{
		if (spawnOnStart)
		{
			StartCoroutine(LevelHandler.Instance.LoadIsland(islandSceneName, this));
			IslandLanded();
		}
	}

	private void MembraneBroken(MembraneShell brokenMembrane)
	{
		// Handle breaking of the island's atmosphere.
		if (brokenMembrane != null && brokenMembrane == atmosphere)
		{
			// TODO: How should player parenting be handled?
			Globals.Instance.player1.transform.parent = transform.parent;
			Globals.Instance.player2.transform.parent = transform.parent;

			if (island == null)
			{
				// Unload other islands.
				LevelHandler.Instance.UnloadIslands();

				// Load the target island.
				StartCoroutine(LevelHandler.Instance.LoadIsland(islandSceneName, this));
			}
			else
			{
				// TODO: load ring
			}
			
		}
	}

	private void IslandLanded()
	{
		if (island != null)
		{
			// TODO: regenerate atmosphere if not already intact
			// TODO: unload ring
		}
	}


}
