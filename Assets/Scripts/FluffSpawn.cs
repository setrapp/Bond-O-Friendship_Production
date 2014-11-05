using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FluffSpawn : MonoBehaviour {
	public GameObject headSprite;
	public int naturalFluffCount;
	public GameObject fluffPrefab;
	public GameObject fluffContainer;
	public List<GameObject> fluffs;
	public float spawnOffset;
	public Material fluffMaterial;
	public float spawnTime;
	private float sinceSpawn;

	void Start()
	{
		if (fluffContainer == null)
		{
			fluffContainer = gameObject;
		}

		while (fluffs.Count < naturalFluffCount)
		{
			SpawnFluff();
		}
		sinceSpawn = 0;
	}

	void Update()
	{
		if (fluffs.Count < naturalFluffCount)
		{
			if (sinceSpawn >= spawnTime)
			{
				SpawnFluff();
				sinceSpawn = 0;
			}
			else
			{
				sinceSpawn += Time.deltaTime;
			}
		}
	}

	private void SpawnFluff()
	{
		Quaternion fluffRotation = Random.rotation;
		fluffRotation.x = fluffRotation.y = 0;
		GameObject newFluff = (GameObject)Instantiate(fluffPrefab, transform.position, fluffRotation);
		newFluff.transform.position += newFluff.transform.up * spawnOffset;
		newFluff.transform.parent = fluffContainer.transform;
		
		MeshRenderer[] meshRenderers = newFluff.GetComponentsInChildren<MeshRenderer>();
		for (int i = 0; i < meshRenderers.Length; i++)
		{
			meshRenderers[i].material = fluffMaterial;
		}

		fluffs.Add(newFluff);
	}
}
