using UnityEngine;
using System.Collections;

public class WaitForLevelLoad : MonoBehaviour {

	public GameObject waitee;
	
	void Awake()
	{
		if (waitee != null && !Application.isEditor)
		{
			Collider[] waitColliders = waitee.GetComponentsInChildren<Collider>();
			for (int i = 0; i < waitColliders.Length; i++)
			{
				waitColliders[i].enabled = false;
			}
		}
	}
	
	void Update()
	{
		if (LevelHandler.Instance != null && !Application.isEditor)
		{
			bool levelFound = false;
			for (int i = 0; i < LevelHandler.Instance.loadedIslands.Count && !levelFound; i++)
			{
				if(LevelHandler.Instance.loadedIslands[i].islandId != IslandID.NONE && LevelHandler.Instance.loadedIslands[i].islandId != IslandID.CREDITS)
				{
					levelFound = true;
				}
			}
			
			if (levelFound)
			{
				if (waitee != null)
				{
					Collider[] waitColliders = waitee.GetComponentsInChildren<Collider>();
					for (int i = 0; i < waitColliders.Length; i++)
					{
						waitColliders[i].enabled = true;
					}
				}
				Destroy(this);
			}
		}
		
		
	}
}
