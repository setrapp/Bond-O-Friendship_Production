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
	private bool wasMoving;

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
			// Calculate the direction the fluffs should be pushed in both world and local space.
			Vector3 fullBackDir = -rigidbody.velocity.normalized;
			Vector3 localFullBackDir = transform.InverseTransformDirection(fullBackDir);
			localFullBackDir.y = localFullBackDir.z;
			localFullBackDir.z = 0;

			for (int i = 0; i < fluffs.Count; i++)
			{
				Vector3 fluffDir = fullBackDir;

				// Compute the fluff's resting direction in world space.
				Vector3 worldBaseDirection = transform.InverseTransformDirection(fluffs[i].baseDirection);
				worldBaseDirection.x *= -1;
				worldBaseDirection.y = worldBaseDirection.z;
				worldBaseDirection.z = 0;

				// Find the vector from the fluff's root to the position of its head last frame.
				Vector3 pushedDir = (fluffs[i].oldBulbPos - fluffs[i].transform.position).normalized;

				// How close is the pushed direction to the resting direction?
				float baseDotPushed = Vector3.Dot(worldBaseDirection, pushedDir);

				// How close must the pushed direction be to the resting position to be used?
				float constrainedDot = Mathf.Cos(maxAlterAngle * Mathf.Deg2Rad);

				// How close is the fluff's base direction to the local direction of movement. Used for special control of fluffs parallel to movement.
				float localBaseDotMove = Vector3.Dot(fluffs[i].baseDirection, -localFullBackDir);

				// If pushed direction is within constraints, use that direction.
				if (baseDotPushed >= constrainedDot && localBaseDotMove < 1)
				{
					fluffs[i].transform.up = pushedDir;
				}
				// Otherwise, compute the fluff direction at the constraints.
				else
				{
					// If the fluff is on the right side, negate its vertical component.
					Vector3 expectedRight = Vector3.Cross(localFullBackDir, transform.up);
					float yMult = 1;
					if (Vector3.Dot(fluffs[i].baseDirection, expectedRight) > 0)
					{
						yMult = -1;
					}

					// Rotate the base direction by the constrained angle.
					Vector3 baseDirection = fluffs[i].baseDirection;
					float radAngle = (maxAlterAngle) * Mathf.Deg2Rad;
					float sinAngle = Mathf.Sin(radAngle);
					float cosAngle = Mathf.Cos(radAngle);
					fluffDir.x = (baseDirection.x * cosAngle) - ((baseDirection.y * sinAngle) * yMult);
					fluffDir.y = 0;	
					fluffDir.z = ((baseDirection.x * sinAngle) * yMult) + (baseDirection.y * cosAngle);

					// Put the rotated direction into world coordinates and use for fluff direction.
					fluffDir = transform.TransformDirection(fluffDir);
					fluffs[i].transform.up = fluffDir;
				}

				fluffs[i].oldBulbPos = fluffs[i].bulb.transform.position;
			}

			wasMoving = true;
		}
		else if (wasMoving)
		{
			for (int i = 0; i < fluffs.Count; i++)
			{
				Vector3 restingUp = fluffs[i].transform.position - transform.position;

				fluffs[i].transform.up = restingUp;

				fluffs[i].oldBulbPos = fluffs[i].bulb.transform.position;
			}
			wasMoving = false;
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
