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
	public float startingFluff;

	void Start()
	{
		if (fluffContainer == null)
		{
			fluffContainer = gameObject;
		}

		while (fluffs.Count < startingFluff)
		{
			SpawnFluff();
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
			Vector3 fluffRotation = FindOpenFluffAngle();

			GameObject newFluff = (GameObject)Instantiate(fluffPrefab, transform.position, Quaternion.identity);
			newFluff.transform.parent = fluffContainer.transform;
			newFluff.transform.localEulerAngles = fluffRotation;
			newFluff.transform.position += newFluff.transform.up *spawnOffset;
			
			MovePulse newFluffInfo = newFluff.GetComponent<MovePulse>();
			newFluffInfo.baseAngle = fluffRotation.z;

			MeshRenderer[] meshRenderers = newFluff.GetComponentsInChildren<MeshRenderer>();
			for (int i = 0; i < meshRenderers.Length; i++)
			{
				meshRenderers[i].material = fluffMaterial;
			}
			if (newFluffInfo.swayAnimation != null)
			{
				newFluffInfo.swayAnimation.enabled = false;
			}
			fluffs.Add(newFluffInfo);
		}
	}

	public Vector3 FindOpenFluffAngle()
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
				fluffAngle = angleIncrement * emptyIntervals[0];
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

		return new Vector3(90, 0, fluffAngle);
	}
}
