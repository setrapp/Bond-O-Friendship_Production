using UnityEngine;
using System.Collections;

public class IslandContainer : MonoBehaviour {
	public EtherRing parentRing;
	public IslandID islandId;
	public Island island;
	[HideInInspector]
	public bool islandLoading = false;
	public string islandSceneName;
	public MembraneShell atmosphere;
	public Vector3 spawnOffset;
	public bool spawnOnStart = false; // TODO this should be handled in main menu.
	private GameObject landedPlayer = null;
	private bool playersLanded = false;

	void Start()
	{
		if (spawnOnStart)
		{
			StartCoroutine(LevelHandler.Instance.LoadIsland(islandSceneName, this));
			IsolateIsland();
		}
	}

	public void GenerateAtmosphere()
	{
		if (atmosphere != null)
		{
			atmosphere.CreateShell();
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
				if (!islandLoading)
				{
					// Unload other islands and generate atmospheres.
					LevelHandler.Instance.UnloadIslands();
					LevelHandler.Instance.GenerateIslandAtmospheres(parentRing, this);

					// Load the target island.
					StartCoroutine(LevelHandler.Instance.LoadIsland(islandSceneName, this));
					islandLoading = true;
				}
			}
			else
			{
				// Load the contents of the ether ring that surrounds this island.
				LevelHandler.Instance.LoadEtherRing(parentRing, this);
			}
			
		}
	}

	private void IslandLanded(GameObject landingPlayer)
	{
		if (landedPlayer != null && landingPlayer != null && landingPlayer != landedPlayer)
		{
			IsolateIsland();
			landedPlayer = null;
		}
		else if (landingPlayer != null)
		{
			landedPlayer = landingPlayer;
		}
	}

	private void IslandUnlanded(GameObject unlandingPlayer)
	{
		if (unlandingPlayer != null && unlandingPlayer == landedPlayer)
		{
			landedPlayer = null;
		}
	}

	private void IsolateIsland()
	{
		GenerateAtmosphere();
		LevelHandler.Instance.UnloadEtherRing(parentRing, this);
	}
}
