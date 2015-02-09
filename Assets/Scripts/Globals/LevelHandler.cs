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
			Island createdIsland = null;
			for (int i = 0; i < islandObjects.Length; i++)
			{
				Island checkIsland = islandObjects[i].GetComponent<Island>();
				if (checkIsland != null && checkIsland.islandId == islandContainer.islandId)
				{
					createdIsland = checkIsland;
					createdIsland.transform.parent = islandContainer.transform;
					createdIsland.transform.localPosition = Vector3.zero + islandContainer.spawnOffset;
					islandContainer.island = createdIsland;
					createdIsland.container = islandContainer;
					islandContainer.islandLoading = false;
					loadedIslands.Add(createdIsland);
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

	public IEnumerator LoadEtherRing(string ringScene, IslandContainer ignoreIsland = null)
	{
		//Debug.Log("start");
		AsyncOperation ringLoading = Application.LoadLevelAdditiveAsync(ringScene);
		yield return ringScene;
		//Debug.Log("end");
		
		/*GameObject[] ringObjects = GameObject.FindGameObjectsWithTag("Ether Ring");
		for (int i = 0; i < ringObjects.Length; i++)
		{
			IslandContainer[] islandContainers = ringObjects[i].GetComponentsInChildren<IslandContainer>();
			for (int j = 0; j < islandContainers.Length; j++)
			{
				if (ignoreIsland != null && islandContainers[j].islandId == ignoreIsland.islandId)
				{
					ignoreIsland.transform.parent = islandContainers[j].transform.parent;
					Destroy(islandContainers[i]);
				}
			}
		}*/
	}

	public void UnloadEtherRing(EtherRing ring, IslandContainer ignoreIsland = null)
	{
		Debug.Log("unload");
		if (ignoreIsland != null && ring != null)
		{
			ignoreIsland.transform.parent = ring.transform.parent;
			Destroy(ring.gameObject);
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
