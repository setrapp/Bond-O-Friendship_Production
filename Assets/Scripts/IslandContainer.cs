using UnityEngine;
using System.Collections;

public class IslandContainer : MonoBehaviour {
	public EtherRing parentRing;
	public IslandID islandId;
	public Island island;
	[HideInInspector]
	public bool islandLoading = false;
	public string islandSceneName;
	public Renderer editorPlaceholder;
	public MembraneShell atmosphere;
	public Vector3 spawnOffset;
	public bool spawnOnStart = false; // TODO this should be handled in main menu.
	private GameObject landedPlayer = null;
	//private bool playersLanded = false;
	private bool waitingToIsolate = false;

	void Start()
	{
		if (spawnOnStart)
		{
			waitingToIsolate = true;
			StartCoroutine(LevelHandler.Instance.LoadIsland(islandSceneName, this));
		}

		if (editorPlaceholder != null)
		{
			editorPlaceholder.gameObject.SetActive(false);
		}
	}

	public void GenerateAtmosphere()
	{
		if (atmosphere != null)
		{
			atmosphere.CreateShell();
		}
	}

	private void MembraneBreaking(MembraneShell BreakingMembrane)
	{
		// Handle breaking of the island's atmosphere.
		if (BreakingMembrane != null && BreakingMembrane == atmosphere)
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

				Globals.Instance.visibilityDepthMaskNeeded = false;
				if (DepthMaskHolder.Instance != null)
				{
					Destroy(DepthMaskHolder.Instance.gameObject);
				}
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
		island.levelHelper.landingEnabledObjects.ToggleObjects(false);
	}

	private void IsolateIsland()
	{
		GenerateAtmosphere();
		//TODO uncomment.
		LevelHandler.Instance.UnloadEtherRing(parentRing, this);
		if (island != null)
		{
			island.levelHelper.landingEnabledObjects.ToggleObjects(true);
		}
	}

	private void IslandLoaded(Island createdIsland)
	{
		if (createdIsland != null && createdIsland.islandId == islandId)
		{
			createdIsland.transform.parent = transform;
			createdIsland.transform.localPosition = Vector3.zero + spawnOffset;
			island = createdIsland;
			createdIsland.container = this;
			islandLoading = false;
			
			PlayersEstablish playersEstablish = createdIsland.GetComponentInChildren<PlayersEstablish>();
			if (playersEstablish != null)
			{
				playersEstablish.PlacePlayers();
			}

			if (waitingToIsolate)
			{
				IsolateIsland();
			}

			if (spawnOnStart && CameraColorFade.Instance != null)
			{
				CameraColorFade.Instance.JumpToColor(createdIsland.backgroundColor);
			}
		}
	}
}
