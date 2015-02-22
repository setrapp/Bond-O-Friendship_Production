using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelHandler : MonoBehaviour {
	private static LevelHandler instance = null;
	public static LevelHandler Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject instanceObject = GameObject.FindGameObjectWithTag("Globals");
				if (instanceObject != null)
				{
					instance = instanceObject.GetComponent<LevelHandler>();
				}
			}
			return instance;
		}
	}
	private List<Island> loadedIslands;
	//private float progressMagicNumber = 0.9f;

	void Awake()
	{
		loadedIslands = new List<Island>();
	}

	public IEnumerator LoadIsland(string islandName, IslandContainer islandContainer)
	{
		if (islandName != null && islandContainer != null && islandContainer.island == null && !islandContainer.islandLoading)
		{
			AsyncOperation islandLoading = Application.LoadLevelAdditiveAsync(islandName);
			yield return islandLoading;

			GameObject[] islandObjects = GameObject.FindGameObjectsWithTag("Island");
			for (int i = 0; i < islandObjects.Length; i++)
			{
				Island checkIsland = islandObjects[i].GetComponent<Island>();
				if (checkIsland != null && checkIsland.islandId == islandContainer.islandId && islandContainer.island == null)
				{
					islandContainer.SendMessage("IslandLoaded", checkIsland);
					loadedIslands.Add(checkIsland);
				}
			}
		}
	}

	public void UnloadIslands()
	{
		for (int i = 0; i < loadedIslands.Count; )
		{
			Island removeIsland = loadedIslands[i];
			loadedIslands.RemoveAt(i);
			if (removeIsland.container != null)
			{
				removeIsland.container.GenerateAtmosphere();

				removeIsland.container.island = null;
				removeIsland.container = null;
			}
			Destroy(removeIsland.gameObject);
		}
	}

	public void LoadEtherRing(EtherRing ring, IslandContainer ignoreIsland = null)
	{
		if (ring != null)
		{
			GenerateIslandAtmospheres(ring, ignoreIsland);
			ring.LoadExpressiveClouds();
		}
	}

	public void UnloadEtherRing(EtherRing ring, IslandContainer ignoreIsland = null)
	{
		if (ring != null)
		{
			IslandContainer[] islandContainers = ring.GetComponentsInChildren<IslandContainer>();
			for (int i = 0; i < islandContainers.Length; i++)
			{
				if (islandContainers[i] != ignoreIsland)
				{
					if (islandContainers[i].atmosphere != null)
					{
						islandContainers[i].atmosphere.SilentBreak();
					}
				}
			}
			ring.UnloadExpressiveClouds();
		}
		
	}

	public void GenerateIslandAtmospheres(EtherRing ring, IslandContainer ignoreIsland = null)
	{
		IslandContainer[] islandContainers = ring.GetComponentsInChildren<IslandContainer>();
		for (int i = 0; i < islandContainers.Length; i++)
		{
			if (islandContainers[i] != ignoreIsland)
			{
				islandContainers[i].GenerateAtmosphere();
			}
		}
	}
	public void RegenerateRingMembrane() { }
}
