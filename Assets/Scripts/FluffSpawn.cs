using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FluffSpawn : MonoBehaviour {
	public Rigidbody rigidbody;
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
	public float maxAlterAngle;

	void Start()
	{
		if (rigidbody == null)
		{
			rigidbody = GetComponent<Rigidbody>();
		}
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
		// Attempt to spawn more fluff.
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

		// Rotate fluffs based on movement.
		if (rigidbody.velocity.sqrMagnitude > 0)
		{
			float checkCos = Mathf.Cos(maxAlterAngle * Mathf.Deg2Rad);
			//float sinAngle = Mathf.Sin(maxAlterAngle * Mathf.Deg2Rad);
			//float cosAngle = Mathf.Cos(maxAlterAngle * Mathf.Deg2Rad);
			//Debug.Log(sinAngle + " " + cosAngle);
			Vector3 fullBackDir = -rigidbody.velocity.normalized;
			Vector3 localFullBackDir = transform.InverseTransformDirection(fullBackDir);
			localFullBackDir.y = localFullBackDir.z;
			localFullBackDir.z = 0;
			//Debug.Log(checkCos);

			for (int i = 0; i < fluffs.Count; i++)
			{
				Vector3 fluffDir = fullBackDir;

				Vector3 worldBaseDirection = transform.InverseTransformDirection(fluffs[i].baseDirection);
				worldBaseDirection.x *= -1;
				worldBaseDirection.y = worldBaseDirection.z;
				worldBaseDirection.z = 0;


				Vector3 newFluffUp = fluffs[i].oldBulbPos - fluffs[i].transform.position;

				float baseDotUp = Vector3.Dot(worldBaseDirection, newFluffUp);
				float cosMax = Mathf.Cos(maxAlterAngle * Mathf.Deg2Rad);

				if (baseDotUp >= cosMax)
				{
					fluffs[i].transform.up = newFluffUp;
				}
				else
				{
					Vector3 expectedRight = Vector3.Cross(-localFullBackDir, transform.up);
					float yMult = 1;
					if (Vector3.Dot(fluffs[i].baseDirection, expectedRight) > 0)
					{
						yMult = -1;
					}

					Vector3 oldUp = fluffs[i].baseDirection;
					float radAngle = (maxAlterAngle) * Mathf.Deg2Rad;
					float sinAngle = Mathf.Sin(radAngle);
					float cosAngle = Mathf.Cos(radAngle);
					fluffDir.x = (oldUp.x * cosAngle) - ((oldUp.y * sinAngle) * yMult);
					fluffDir.y = (oldUp.x * sinAngle) + ((oldUp.y * cosAngle) * yMult);
					fluffDir.z = 0;
					if (Vector3.Dot(fluffs[i].baseDirection, Vector3.right) > 0)
					{
						fluffDir.y *= -1;
					}

					fluffDir = transform.InverseTransformDirection(fluffDir);
					fluffDir.x *= -1;
					fluffDir.y = fluffDir.z;
					fluffDir.z = 0;
					fluffs[i].transform.up = fluffDir;
				}

				fluffs[i].oldBulbPos = fluffs[i].bulb.transform.position;
			}
		}
		else
		{
			for (int i = 0; i < fluffs.Count; i++)
			{
				Vector3 restingUp = fluffs[i].transform.position - transform.position;

				fluffs[i].transform.up = restingUp;

				fluffs[i].oldBulbPos = fluffs[i].bulb.transform.position;
			}
		}

		//Debug.Log("-----");
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

			newFluffInfo.baseDirection = newFluff.transform.up;
			newFluffInfo.baseDirection = transform.InverseTransformDirection(newFluffInfo.baseDirection);
			newFluffInfo.baseDirection.y = newFluffInfo.baseDirection.z;
			newFluffInfo.baseDirection.z = 0;

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
