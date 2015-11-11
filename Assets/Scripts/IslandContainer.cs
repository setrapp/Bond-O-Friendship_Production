using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IslandContainer : MonoBehaviour {
	public IslandID islandId;
	public Island island;
	public Island tempIsland;
	[HideInInspector]
	public bool islandLoading = false;
	public string islandSceneName;
	public Renderer editorPlaceholder;
	public List<MembraneWall> atmosphere;
	public Vector3 spawnOffset;
	public Vector3 spawnRotation;
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

	public void GenerateAtmosphere(MembraneWall triggerMembrane = null)
	{
		if (atmosphere != null)
		{
			for (int i = 0; i < atmosphere.Count; i++)
			{
				if (atmosphere[i] != null && atmosphere[i] != triggerMembrane)
				{
					atmosphere[i].CreateWall();
				}
			}
		}
	}

	public void DestroyAtmosphere()
	{
		// Ignore ensuing atmosphere breaks until all affected membranes have been handled.
		LevelHandler.Instance.ignoreAtmosphereBreaks = true;

		if (atmosphere != null)
		{
			for (int i = 0; i < atmosphere.Count; i++)
			{
				if (atmosphere[i] != null && atmosphere[i].membraneCreator != null && atmosphere[i].membraneCreator.createdBond != null)
				{
					atmosphere[i].membraneCreator.createdBond.BreakBond();
				}
			}
		}

		// Stop ignoring atmosphere breaks.
		LevelHandler.Instance.ignoreAtmosphereBreaks = false;
	}

	public void DestroyLinkedMembranes(MembraneWall ignoreLinks = null)
	{
		// Ignore ensuing atmosphere breaks until all affected membranes have been handled.
		LevelHandler.Instance.ignoreAtmosphereBreaks = true;

		if (atmosphere != null)
		{
			for (int i = 0; i < atmosphere.Count; i++)
			{
				if (atmosphere[i] != null && atmosphere[i] != ignoreLinks && atmosphere[i].creationLink != null)
				{
					List<MembraneWall> linkedMembranes = atmosphere[i].creationLink.linkedMembranes;
					for (int j = 0; j < linkedMembranes.Count; j++)
					{
						if (linkedMembranes[j] != null && linkedMembranes[j].membraneCreator != null && linkedMembranes[j].membraneCreator.createdBond != null)
						{
							linkedMembranes[j].membraneCreator.createdBond.BreakBond();
						}
					}
				}
				
			}
		}

		// Stop ignoring atmosphere breaks.
		LevelHandler.Instance.ignoreAtmosphereBreaks = false;
	}

	private void MembraneWallBreaking(MembraneWall breakingMembrane)
	{
		// If atmosphere breaks are being ignored, skip this break.
		if (LevelHandler.Instance.ignoreAtmosphereBreaks)
		{
			return;
		}

		// Handle breaking of the island's atmosphere.
		if (breakingMembrane != null && atmosphere.Contains(breakingMembrane))
		{
			// TODO: How should player parenting be handled?
			Globals.Instance.Player1.transform.parent = transform.parent;
			Globals.Instance.Player2.transform.parent = transform.parent;

			// Entering Level.
			if (island == null)
			{
				if (!islandLoading)
				{
					// Unload other islands and generate atmospheres.
					if (tempIsland != null)
					{
						island = tempIsland;
						tempIsland = null;
					}
					LevelHandler.Instance.UnloadIslands(island);
					//LevelHandler.Instance.GenerateIslandAtmospheres(parentRing, this);

					// Load the target island.
					//StartCoroutine(LevelHandler.Instance.LoadIsland(islandSceneName, this));
					//islandLoading = true;

					// TODO disable atmosphere of previous level
					
				}
			}
			//Exiting Level
			else
			{
				// Load the target island.
				MembraneCreationLink membraneConnection = breakingMembrane.GetComponent<MembraneCreationLink>();
				if (membraneConnection != null)
				{
					IslandContainer connectedIslandContainer = membraneConnection.linkedIslandContainer;
					if (connectedIslandContainer != null)
					{
						if (!connectedIslandContainer.islandLoading && connectedIslandContainer.island == null)
						{
							// Make atmosphere into new level unbreakable until the level is loaded.
							for (int i = 0; i < connectedIslandContainer.atmosphere.Count; i++)
							{
								if (connectedIslandContainer.atmosphere[i] != null)
								{
									connectedIslandContainer.atmosphere[i].requiredPlayersToBreak = 3;
								}
							}

							LevelHandler.Instance.UnloadIslands(island);

							StartCoroutine(LevelHandler.Instance.LoadIsland(connectedIslandContainer.islandSceneName, connectedIslandContainer));
							connectedIslandContainer.islandLoading = true;
						}
					}
				}

				// TODO disable membranes linked to other membranes in atmosphere
				//DestroyLinkedMembranes(breakingMembrane);

				// Load the contents of the ether ring that surrounds this island.
				//LevelHandler.Instance.LoadEtherRing(parentRing, this);


				// TODO is this needed for darkness stuff still.
				/*Globals.Instance.visibilityDepthMaskNeeded = false;
				if (DepthMaskHolder.Instance != null)
				{
					Destroy(DepthMaskHolder.Instance.gameObject);
				}*/
			}

			// Enable the membranes linked to the one broken.
			if (breakingMembrane.creationLink != null)
			{
				breakingMembrane.creationLink.CreateMembranes();
			}

			// Enable all other membranes in atmosphere.
			GenerateAtmosphere(breakingMembrane);
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
		//LevelHandler.Instance.UnloadEtherRing(parentRing, this);
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
			if (editorPlaceholder)
			{
				createdIsland.transform.position = editorPlaceholder.transform.position;
			}
			createdIsland.transform.localPosition += spawnOffset;
			createdIsland.transform.localRotation = Quaternion.Euler(spawnRotation);
			if (spawnOnStart) { island = createdIsland; }
			else { tempIsland = createdIsland; }
			createdIsland.container = this;
			islandLoading = false;

			// When level is fully loaded, allow the atmosphere to break.
			for (int i = 0; i < atmosphere.Count; i++)
			{
				if (atmosphere[i] != null)
				{
					atmosphere[i].requiredPlayersToBreak = 2;
				}
			}
			
			/*PlayersEstablish playersEstablish = createdIsland.GetComponentInChildren<PlayersEstablish>();
			if (playersEstablish != null)
			{
				playersEstablish.PlacePlayers();
			}*/

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
