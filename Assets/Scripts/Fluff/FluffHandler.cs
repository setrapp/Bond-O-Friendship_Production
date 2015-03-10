using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FluffHandler : MonoBehaviour {
	public CharacterComponents character;
	public Rigidbody body;
	public GameObject headSprite;
	public int naturalFluffCount;
	public GameObject fluffPrefab;
	public GameObject fluffContainer;
	public List<Fluff> fluffs;
	public float spawnOffset;
	public Material fluffMaterial;
	public float spawnTime;
	private float sinceSpawn;
	public float startingFluff;
	public float maxFluffs = 32;
	public Fluff spawnedFluff;
	private Vector3 endPosition;
	public float sproutSpeed = 0.01f;
	public float maxAlterAngle;
	private float oldSpeed;
	private bool wasSlowing;
	private Vector3 oldForward;
	private FluffStick fluffStick;
	private List<Fluff> fluffsToAdd;

	void Awake()
	{
		if (body == null)
		{
			body = GetComponent<Rigidbody>();
		}
		if (fluffContainer == null)
		{
			fluffContainer = gameObject;
		}
		if (character == null)
		{
			character = GetComponent<CharacterComponents>();
		}
		
		fluffStick = GetComponent<FluffStick>();

		SpawnStartingFluff();
		
		sinceSpawn = 0;
		oldForward = transform.forward;
		wasSlowing = false;
	}

	void Update()
	{
		if (fluffs.Count >= naturalFluffCount)
		{
			if (naturalFluffCount <= 0)
			{
				character.fillScale = 0;
			}
			else
			{
				character.fillScale = 1;
			}
		}

		// Attempt to spawn more fluff.
		if (fluffs.Count < naturalFluffCount && fluffs.Count < maxFluffs)
		{
			if (spawnTime >= 0)
			{
				if (sinceSpawn >= spawnTime)
				{
					SpawnFluff();
					sinceSpawn = 0;
					character.fillScale = 1;
				}
				else if (spawnedFluff == null)
				{
					if (character.fillScale == 1)
					{
						character.fillScale = 0;
					}
					sinceSpawn += Time.deltaTime;
					character.fillScale += Time.deltaTime;
				}
			}
		}

		// Push the most recently spawned fluff to the outside.
		if (spawnedFluff != null)
		{
			spawnedFluff.transform.localPosition = Vector3.MoveTowards(spawnedFluff.transform.localPosition, endPosition, sproutSpeed);
			spawnedFluff.oldBulbPos = spawnedFluff.bulb.transform.position;
			if (spawnedFluff.transform.localPosition == endPosition)
			{
				spawnedFluff = null;
			}
		}

		if (fluffsToAdd != null)
		{
			// Spawn fluffs that look like clones of the ones being added.
			for (int i = fluffsToAdd.Count - 1; i >= 0; i--)
			{
				Material fluffMaterial = null;
				if (fluffsToAdd != null)
				{
					fluffMaterial = fluffsToAdd[i].bulb.material;

					SpawnFluff(true, fluffMaterial);

					Destroy(fluffsToAdd[i].gameObject);
					fluffsToAdd.RemoveAt(i);
				}
			}

			fluffsToAdd.Clear();
			fluffsToAdd = null;
		}
	}

	void FixedUpdate()
	{
		/*TODO rotate fluff constraints to rigidbody direction rather that transform direction.*/

		// Rotate fluffs based on movement.
		if (body.velocity.sqrMagnitude > 0)
		{
			// Calculate the direction the fluffs should be pushed in both world and local space.
			float currentSpeed = body.velocity.magnitude;
			float currentByOldSpeed = currentSpeed / oldSpeed;
			Vector3 fullBackDir = -body.velocity / currentSpeed;
			Vector3 localFullBackDir = fluffContainer.transform.InverseTransformDirection(fullBackDir);
			for (int i = 0; i < fluffs.Count; i++)
			{
				// Compute the fluff's resting direction in world space.
				Vector3 worldBaseDirection = fluffs[i].baseDirection;
				worldBaseDirection = fluffContainer.transform.TransformDirection(worldBaseDirection);
				
				Vector3 fluffDir = fluffs[i].transform.up;

				// If the character turned about-face from last frame, flip fluffs expected parallel direction.
				if (Vector3.Dot(transform.forward, oldForward) < 0)
				{
					fluffDir *= -1;
					fluffs[i].oldBulbPos -= (fluffs[i].oldBulbPos - transform.position) * 2;
				}

				if (currentByOldSpeed >= 1 || !wasSlowing)
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
						fluffDir.y = ((baseDirection.x * sinAngle) * yMult) + (baseDirection.y * cosAngle);
						fluffDir.z = 0;

						// Put the rotated direction into world coordinates and use for fluff direction.
						fluffDir = fluffContainer.transform.TransformDirection(fluffDir);
					}
				}
				else
				{
					// Localize the current fluff direction.
					Vector3 localFluffDir = fluffContainer.transform.InverseTransformDirection(fluffDir);

					// Interpolate towards resting direction.
					Vector3 fromRest = localFluffDir - fluffs[i].baseDirection;
					localFluffDir = fluffs[i].baseDirection + (fromRest * currentByOldSpeed);

					// Worldize new fluff direction.
					fluffDir = fluffContainer.transform.TransformDirection(localFluffDir);
				}
				
				fluffs[i].transform.up = fluffDir;
				fluffs[i].oldBulbPos = fluffs[i].bulb.transform.position;
			}

			wasSlowing = currentByOldSpeed < 1;
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
		oldForward = transform.forward;
	}

	public void SpawnFluff(bool instantSprout = false, Material useMaterial = null)
	{
		if (fluffPrefab.GetComponent<Fluff>() != null)
		{
			Vector3 fluffRotation = FindOpenFluffAngle();

			GameObject newFluff = (GameObject)Instantiate(fluffPrefab, fluffContainer.transform.position, Quaternion.identity);
			newFluff.transform.parent = fluffContainer.transform;
			newFluff.GetComponent<Rigidbody>().isKinematic = true;
			newFluff.transform.localEulerAngles = fluffRotation;
			Vector3 tempEndPosition = newFluff.transform.up * spawnOffset + newFluff.transform.position;
			tempEndPosition = fluffContainer.transform.InverseTransformPoint(tempEndPosition);

			Fluff newFluffInfo = newFluff.GetComponent<Fluff>();

			if (instantSprout)
			{
				newFluff.transform.localPosition = tempEndPosition;
				newFluffInfo.oldBulbPos = newFluffInfo.bulb.transform.position;
			}
			else
			{
				// Force the old spawned fluff out.
				if (spawnedFluff != null)
				{
					spawnedFluff.transform.localPosition = fluffContainer.transform.InverseTransformPoint(endPosition);
				}

				endPosition = tempEndPosition;
				spawnedFluff = newFluffInfo;
				newFluff.transform.position += newFluff.transform.up * spawnOffset * (Time.deltaTime / spawnTime);
			}
			
			newFluffInfo.baseAngle = fluffRotation.z;

			newFluffInfo.baseDirection = newFluff.transform.up;
			newFluffInfo.baseDirection = fluffContainer.transform.InverseTransformDirection(newFluffInfo.baseDirection);

			if (useMaterial == null)
			{
				useMaterial = fluffMaterial;
			}

			if (newFluffInfo.bulb != null)
			{
				newFluffInfo.bulb.material = useMaterial;
			}

			if (newFluffInfo.stalk != null)
				newFluffInfo.stalk.material = useMaterial;

			newFluffInfo.ToggleSwayAnimation(false);
			newFluffInfo.hull.isTrigger = true;
			newFluffInfo.attachee = new Attachee(gameObject, fluffStick, endPosition, true, true);
			newFluffInfo.creator = character.bondAttachable;
			fluffs.Add(newFluffInfo);
		}
	}

	public void SpawnStartingFluff()
	{
		DestroyAllFluffs(false);
		while (fluffs.Count < startingFluff)
		{
			SpawnFluff(true);
		}
	}

	public void AttachFluff(Fluff fluff)
	{
		if (fluff != null && (character.attractor.attracting || fluff.moving) && (fluff.attachee == null || fluff.attachee.gameObject == gameObject || !fluff.attachee.possessive))
		{
			character.bondAttachable.AttemptBond(fluff.creator, fluff.transform.position);

			if (fluff.creator != null && fluff.creator != character.bondAttachable)
			{
				character.SetFlashAndFill(fluff.creator.attachmentColor);
			}

			if (fluffs.Count < maxFluffs)
			{
				if (fluffsToAdd == null)
				{
					fluffsToAdd = new List<Fluff>();
				}
				fluffsToAdd.Add(fluff);
			}
			else
			{
				Destroy(fluff.gameObject);
			}
		}
	}

	public void DestroyAllFluffs(bool popFluff = true)
	{
		for (int i = fluffs.Count - 1; i >= 0; i--)
		{
			DestroyFluff(fluffs[i], popFluff);
		}
	}

	public void DestroyFluff(Fluff fluffToDestroy, bool popFluff = true)
	{
		if (fluffs.Contains(fluffToDestroy))
		{
			if (popFluff)
			{
				fluffToDestroy.PopFluff();
				fluffToDestroy.transform.parent = transform.parent;
				fluffToDestroy.StopMoving();
			}
			else
			{
				Destroy(fluffToDestroy.gameObject);
			}
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

		return new Vector3(0, 0, fluffAngle);
	}
}
