using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FluffSpawn : MonoBehaviour {
	public GameObject headSprite;
	public int naturalFluffCount;
	public GameObject fluffPrefab;
	public GameObject fluffContainer;
	public List<MovePulse> fluffs;
	public float spawnOffset;
	public Material fluffMaterial;
	public float spawnTime;
	private float sinceSpawn;
	public bool startFluffed;

	void Start()
	{
		if (fluffContainer == null)
		{
			fluffContainer = gameObject;
		}

		if (startFluffed)
		{
			while (fluffs.Count < naturalFluffCount)
			{
				SpawnFluff();
			}
		}
		
		sinceSpawn = 0;
	}

	void Update()
	{
		if (fluffs.Count < naturalFluffCount)
		{
			if (spawnTime >= 0)
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
	}

	private void SpawnFluff()
	{
		if (fluffPrefab.GetComponent<MovePulse>() != null)
		{
			//Quaternion fluffRotation = Random.rotation;
			//fluffRotation.x = fluffRotation.y = 0;
			float fluffAngle = FindOpenFluffAngle();
			Quaternion fluffRotation = Quaternion.Euler(0, 0, fluffAngle);

			GameObject newFluff = (GameObject)Instantiate(fluffPrefab, transform.position, Quaternion.identity);
			newFluff.transform.localRotation = fluffRotation;
			newFluff.transform.position += newFluff.transform.up * spawnOffset;
			newFluff.transform.parent = fluffContainer.transform;
			MovePulse newFluffInfo = newFluff.GetComponent<MovePulse>();
			newFluffInfo.baseAngle = fluffAngle;

			MeshRenderer[] meshRenderers = newFluff.GetComponentsInChildren<MeshRenderer>();
			for (int i = 0; i < meshRenderers.Length; i++)
			{
				meshRenderers[i].material = fluffMaterial;
			}

			fluffs.Add(newFluffInfo);
		}
	}

	public float FindOpenFluffAngle()
	{
		float fluffAngle = -1;
		float angleIncrement = 360.0f;
		int scarceSuperIncrement = -1;
		while(fluffAngle < 0)
		{
			int[] incrementCollections = new int[(int)(360 / angleIncrement)];
			for (int i = 0; i < incrementCollections.Length; i++)
			{
				incrementCollections[i] = 0;
			}

			for (int i = 0; i < fluffs.Count; i++)
			{
				incrementCollections[(int)(fluffs[i].baseAngle / angleIncrement)]++;
			}

			int[] emptyIntervals = new int[incrementCollections.Length];
			int emptyIndex = -1;
			float superIncrement = angleIncrement * 2;
			float minAngle = superIncrement * scarceSuperIncrement;
			float maxAngle = minAngle + superIncrement;
			for (int i = 0; i < incrementCollections.Length; i++)
			{
				float checkAngle = angleIncrement * i;
				if (incrementCollections[i] < 1 && (scarceSuperIncrement < 0 || (checkAngle >= minAngle && checkAngle <= maxAngle)))
				{
					emptyIndex++;
					emptyIntervals[emptyIndex] = i;
				}
			}

			if (emptyIndex >= 0)
			{
				// TODO rather than pick a random from here pick a random from the super increment.
				fluffAngle = angleIncrement * emptyIntervals[0];//Random.Range(0, emptyIndex + 1)];
			}
			else
			{
				int minSuper = scarceSuperIncrement * 2;
				int maxSuper = scarceSuperIncrement * 2 + 2;
				scarceSuperIncrement = Mathf.Max(minSuper, 0);
				for (int i = scarceSuperIncrement; i < incrementCollections.Length && i < maxSuper; i++)
				{
					if (incrementCollections[i] < incrementCollections[scarceSuperIncrement] && (scarceSuperIncrement < 0 || (scarceSuperIncrement >= minSuper && scarceSuperIncrement <= minSuper)))
					{
						scarceSuperIncrement = i;
					}
				}
				angleIncrement /= 2;
			}
		}

		return fluffAngle;
	}
}
