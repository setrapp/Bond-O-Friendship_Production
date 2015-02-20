using UnityEngine;
using System.Collections;

public class SpawnRingBreaker : MonoBehaviour {

	public GameObject ringBreakerPrefab;
	public WaitPad triggerPad;
	public float delay = 0;
	public Vector3 offset = new Vector3();
	private bool waitingToCreate = false;
	[HideInInspector]
	public RingBreaker createdRingBreaker;
	public AutoBond bondCreator;
	public BondAttachable bondAttachable;
	public Island attachedIsland;

	void Update()
	{
		if (triggerPad != null && triggerPad.activated && createdRingBreaker == null && ! waitingToCreate)
		{
			SpawnBreaker();
		}
	}

	public void SpawnBreaker()
	{
		if (ringBreakerPrefab != null && createdRingBreaker == null && !waitingToCreate)
		{
			StartCoroutine(Spawn());
		}
	}

	private IEnumerator Spawn()
	{
		if (ringBreakerPrefab != null)
		{
			MembraneShell targetRing = null;
			if (attachedIsland != null && attachedIsland.container != null && attachedIsland.container.parentRing != null)
			{
				targetRing = attachedIsland.container.parentRing.ringAtmosphere;
			}


			waitingToCreate = true;
			yield return new WaitForSeconds(delay);
			GameObject newBreaker = (GameObject)Instantiate(ringBreakerPrefab, transform.position, Quaternion.identity);
			createdRingBreaker = newBreaker.GetComponent<RingBreaker>();
			newBreaker.SetActive(true);
			if (bondCreator != null && createdRingBreaker != null && targetRing != null)
			{
				bondCreator.attachable1 = bondAttachable;
				bondCreator.attachable2 = createdRingBreaker.bondAttachable;
				if (bondCreator.attachable1 != null && bondCreator.attachable2 != null)
				{
					bondCreator.CreateBond();
				}
			}
			createdRingBreaker.targetRing = targetRing;
		}
	}
}
