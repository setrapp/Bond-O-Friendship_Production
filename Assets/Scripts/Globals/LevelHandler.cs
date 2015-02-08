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

	public IEnumerator LoadIsland(string sceneName, IslandContainer islandContainer)
	{
		if (sceneName != null && islandContainer != null)
		{
			AsyncOperation islandLoading = Application.LoadLevelAdditiveAsync(sceneName);
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
				removeIsland.container.island = null;
				removeIsland.container = null;
			}
			Destroy(removeIsland.gameObject);
		}
	}

	public void LoadEtherRing() {} 

	public void RegenerateIslandMembrane() { }
	public void RegenerateRingMembrane() { }
}
