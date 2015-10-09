using UnityEngine;
using System.Collections;

public class FluffPlaceholder : MonoBehaviour {
	public GameObject fluffPrefab;
	public Renderer materialSource;
	public FluffStick attachee;
	public Fluff createdFluff = null;
	public int fluffRespawns = 0;
	private float respawnDelay = 0;
	public float respawnDelayMin = 0;
	public float respawnDelayMax = 0;
	private float pickTime = -1;
	public bool autoSpawn = true;
	public bool dropParent = false;
	public bool spawnFakeMoving = false;
	public bool sproutFluff = false;
	[HideInInspector]
	public bool readyForSpawn = false;

	void Awake()
	{
		if (materialSource == null)
		{
			materialSource = GetComponent<Renderer>();
		}
		if (materialSource != null)
		{
			materialSource.enabled = false;
		}

		if (attachee == null)
		{
			attachee = GetComponent<FluffStick>();
			if (attachee == null)
			{
				attachee = GetComponentInParent<FluffStick>();
			}
		}

		if (attachee != null)
		{
			attachee.stickOffset = attachee.transform.InverseTransformPoint(transform.position);
			attachee.stickDirection = -attachee.transform.InverseTransformDirection(transform.up);
		}
	}

	void Start()
	{
		if (autoSpawn)
		{
			SpawnFluff();
		}
	}

	void Update()
	{

		if (pickTime >= 0 && Time.time - pickTime >= respawnDelay)
		{
			if (autoSpawn)
			{
				fluffRespawns = Mathf.Max(fluffRespawns - 1, -1);
				SpawnFluff();
			}
			else
			{
				readyForSpawn = true;
			}
		}


		if ((createdFluff == null || createdFluff.moving) && pickTime < 0)
		{
			createdFluff = null;
			pickTime = Time.time;
		}
	}

	public void SpawnFluff()
	{
		Material fluffMaterial = null;
		if (materialSource != null)
		{
			fluffMaterial = materialSource.material;
		}

		if (fluffPrefab != null && (attachee == null || attachee.CanStick()))
		{
			GameObject newFluffObj = (GameObject)Instantiate(fluffPrefab, transform.position, transform.rotation);
			newFluffObj.transform.parent = transform.parent;
			Fluff newFluff = newFluffObj.GetComponent<Fluff>();
			if (newFluff != null)
			{
				if (fluffMaterial != null)
				{
					newFluff.bulb.material = fluffMaterial;
					//newFluff.stalk.material = fluffMaterial;
				}
				if (attachee != null)
				{
					newFluff.Attach(attachee, true, sproutFluff);
				}
				else 
				{
					// If desired, fake movement to allow fluff update to handle attachment to floor.
					if (spawnFakeMoving)
					{
						newFluff.moving = true;
					}
					newFluff.ToggleSwayAnimation(true);
				}
				createdFluff = newFluff;

				if (dropParent)
				{
					if (OrphanFluffHolder.Instance != null)
					{
						createdFluff.transform.parent = OrphanFluffHolder.Instance.transform;
					}
					else
					{
						createdFluff.transform.parent = null;
					}
				}
			}
		}

		if (fluffRespawns == 0)
		{
			Destroy(gameObject);
		}
		pickTime = -1;
		readyForSpawn = false;
		respawnDelay = Random.Range(respawnDelayMin, respawnDelayMax);
	}
}
