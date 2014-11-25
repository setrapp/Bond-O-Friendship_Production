using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FluffSpawn : MonoBehaviour {
	public Rigidbody body;
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
	public GameObject spawnedFluff;
	private Vector3 endPosition;
	public float sproutSpeed = 0.01f;
	public float maxAlterAngle;
	private float oldSpeed;
	[HideInInspector]
	public PartnerLink partnerLink;
	private FluffStick fluffStick;

	void Start()
	{
		if (body == null)
		{
			body = GetComponent<Rigidbody>();
		}
		if (fluffContainer == null)
		{
			fluffContainer = gameObject;
		}

		partnerLink = GetComponent<PartnerLink>();
		fluffStick = GetComponent<FluffStick>();
		

		while (fluffs.Count < startingFluff)
		{
			SpawnFluff(true);
		}
		
		sinceSpawn = 0;
	}

	void FixedUpdate()
	{
		if (fluffs.Count >= naturalFluffCount)
		{
			partnerLink.fillScale = 1;
		}

		// Attempt to spawn more fluff.
		if (fluffs.Count < naturalFluffCount)
		{
			if (spawnTime >= 0)
			{
				if (sinceSpawn >= spawnTime)
				{
					SpawnFluff();
					sinceSpawn = 0;
					partnerLink.fillScale = 1;
				}
				else if (spawnedFluff == null)
				{
					if (partnerLink.fillScale == 1)
					{
						partnerLink.fillScale = 0;
					}
					sinceSpawn += Time.deltaTime;
					partnerLink.fillScale += Time.deltaTime;
				}
			}
		}

		if(spawnedFluff != null)
		{
			spawnedFluff.transform.localPosition = Vector3.MoveTowards(spawnedFluff.transform.localPosition, endPosition, sproutSpeed);
			if(spawnedFluff.transform.localPosition == endPosition)
			{
				spawnedFluff = null;
			}
		}

		// Rotate fluffs based on movement.
		if (body.velocity.sqrMagnitude > 0)
		{
			// Calculate the direction the fluffs should be pushed in both world and local space.
			float currentSpeed = body.velocity.magnitude;
			float currentByOldSpeed = currentSpeed / oldSpeed;
			Vector3 fullBackDir = -body.velocity / currentSpeed;
			Vector3 localFullBackDir = transform.InverseTransformDirection(fullBackDir);
			localFullBackDir.y = localFullBackDir.z;
			localFullBackDir.z = 0;

			for (int i = 0; i < fluffs.Count; i++)
			{
				// Compute the fluff's resting direction in world space.
				Vector3 worldBaseDirection = transform.InverseTransformDirection(fluffs[i].baseDirection);
				worldBaseDirection.x *= -1;
				worldBaseDirection.y = worldBaseDirection.z;
				worldBaseDirection.z = 0;

				Vector3 fluffDir = fluffs[i].transform.up;

				if (currentByOldSpeed>= 1)
				{
					// Find the vector from the fluff's root to the position of its head last frame.
					fluffDir = (fluffs[i].oldBulbPos - fluffs[i].transform.position).normalized;

					// How close is the pushed direction to the resting direction?
					float baseDotPushed = Vector3.Dot(worldBaseDirection, fluffDir);

					// How close must the pushed direction be to the resting position to be used?
					float constrainedDot = Mathf.Cos(maxAlterAngle * Mathf.Deg2Rad);

					// How close is the fluff's base direction to the local direction of movement. Used for special control of fluffs parallel to movement.
					float localBaseDotMove = Vector3.Dot(fluffs[i].baseDirection, -localFullBackDir);

					// If pushed direction is beyond constraints, compute the fluff direction at the constraints.
					if (baseDotPushed < constrainedDot || localBaseDotMove > 1)
					{
						// If the fluff is on the right side, negate its vertical component.
						Vector3 expectedRight = Vector3.right;
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
					}
				}
				else
				{
					// Localize the current fluff direction.
					Vector3 localFluffDir = transform.InverseTransformDirection(fluffDir);
					localFluffDir.y = localFluffDir.z;
					localFluffDir.z = 0;

					// Interpolate towards resting direction.
					Vector3 fromRest = localFluffDir - fluffs[i].baseDirection;
					localFluffDir = fluffs[i].baseDirection + (fromRest * currentByOldSpeed);

					// Worldize new fluff direction.
					localFluffDir.z = localFluffDir.y;
					localFluffDir.y = 0;
					fluffDir = transform.TransformDirection(localFluffDir);
				}

				fluffs[i].transform.up = fluffDir;
				fluffs[i].oldBulbPos = fluffs[i].bulb.transform.position;
			}

			oldSpeed = currentSpeed;
		}
		else if (oldSpeed > 0)
		{
			for (int i = 0; i < fluffs.Count; i++)
			{
				Vector3 restingUp = fluffs[i].transform.position - transform.position;

				fluffs[i].transform.up = restingUp;

				fluffs[i].oldBulbPos = fluffs[i].bulb.transform.position;
			}
			oldSpeed = 0;
		}
	}

	public void SpawnFluff(bool instantSprout = false, Material useMaterial = null)
	{
		if (fluffPrefab.GetComponent<MovePulse>() != null)
		{
			Vector3 fluffRotation = FindOpenFluffAngle();

			GameObject newFluff = (GameObject)Instantiate(fluffPrefab, transform.position, Quaternion.identity);
			newFluff.transform.parent = fluffContainer.transform;
			newFluff.GetComponent<Rigidbody>().isKinematic = true;
			newFluff.transform.localEulerAngles = fluffRotation;
			Vector3 tempEndPosition = newFluff.transform.up * spawnOffset + newFluff.transform.position;
			tempEndPosition = transform.InverseTransformPoint(tempEndPosition);
			
			if (instantSprout)
			{
				newFluff.transform.localPosition = tempEndPosition;
			}
			else
			{
				// Force the old spawned fluff out.
				if (spawnedFluff != null)
				{
					spawnedFluff.transform.localPosition = transform.InverseTransformPoint(endPosition);
				}

				endPosition = tempEndPosition;
				spawnedFluff = newFluff;
				newFluff.transform.position += newFluff.transform.up * spawnOffset * (Time.deltaTime / spawnTime);
			}
			
			MovePulse newFluffInfo = newFluff.GetComponent<MovePulse>();
			newFluffInfo.baseAngle = fluffRotation.z;

			newFluffInfo.baseDirection = newFluff.transform.up;
			newFluffInfo.baseDirection = transform.InverseTransformDirection(newFluffInfo.baseDirection);
			newFluffInfo.baseDirection.y = newFluffInfo.baseDirection.z;
			newFluffInfo.baseDirection.z = 0;

			if (useMaterial == null)
			{
				useMaterial = fluffMaterial;
			}

			MeshRenderer[] meshRenderers = newFluff.GetComponentsInChildren<MeshRenderer>();
			
			for (int i = 0; i < meshRenderers.Length; i++)
			{
				meshRenderers[i].material = useMaterial;
			}
			newFluffInfo.ToggleSwayAnimation(false);
			newFluffInfo.hull.isTrigger = true;
			newFluffInfo.attachee = new Attachee(gameObject, fluffStick, endPosition, true, true);
			fluffs.Add(newFluffInfo);
		}
	}

	public void DestroyAllFluffs()
	{
		for (int i = fluffs.Count - 1; i >= 0; i--)
		{
			DestroyFluff(fluffs[i]);
		}
	}

	public void DestroyFluff(MovePulse fluffToDestroy)
	{
		if (fluffs.Contains(fluffToDestroy))
		{
			Destroy(fluffToDestroy.gameObject);
			fluffs.Remove(fluffToDestroy);
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
