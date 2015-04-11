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
	public bool dropParent = false;
	public bool spawnFakeMoving = false;
	public bool sproutFluff = false;

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

		SpawnFluff();
	}

	void Update()
	{
		if (pickTime >= 0 && Time.time - pickTime >= respawnDelay)
		{
			fluffRespawns--;
			SpawnFluff();
		}
		
		if ((createdFluff == null || createdFluff.moving) && pickTime < 0)
		{
			createdFluff = null;
			pickTime = Time.time;
		}
	}

	private void SpawnFluff()
	{
		Material fluffMaterial = null;
		if (materialSource != null)
		{
			fluffMaterial = materialSource.material;
		}

		if (fluffPrefab != null)
		{
			GameObject newFluffObj = (GameObject)Instantiate(fluffPrefab, transform.position, transform.rotation);
			newFluffObj.transform.parent = transform.parent;
			Fluff newFluff = newFluffObj.GetComponent<Fluff>();
			if (newFluff != null)
			{
				if (fluffMaterial != null)
				{
					newFluff.bulb.material = fluffMaterial;
					newFluff.stalk.material = fluffMaterial;
				}
				if (attachee != null)
				{
					newFluff.Attach(attachee/*, attachee.transform.position + attachee.transform.TransformDirection(attachee.stickOffset), attachee.transform.TransformDirection(attachee.stickDirection)*/, true, sproutFluff);
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
		else
		{
			Debug.LogError("Fluff Placeholder unable to spawn fluff. Please ensure parameters are correct.");
		}

		if (fluffRespawns == 0)
		{
			Destroy(gameObject);
		}
		pickTime = -1;
		respawnDelay = Random.Range(respawnDelayMin, respawnDelayMax);
	}
}
