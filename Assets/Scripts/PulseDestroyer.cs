using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PulseDestroyer : MonoBehaviour {
	public bool destroyPulses = true;
	public bool destroyConnections = true;
	public int newNaturalFluff = -1;
	private List<GameObject> toDestroy = null;
	private List<FluffSpawn> toEmpty = null;


	void Update()
	{
		if (toDestroy != null)
		{
			for (int i = toDestroy.Count - 1; i >= 0; i--)
			{
				Destroy(toDestroy[i]);
				toDestroy.RemoveAt(i);
			}
			toDestroy.Clear();
			toDestroy = null;
		}

		if (toEmpty != null)
		{
			for (int i = toEmpty.Count - 1; i >= 0; i--)
			{
				toEmpty[i].DestroyAllFluffs();
				if (newNaturalFluff >= 0)
				{
					toEmpty[i].naturalFluffCount = newNaturalFluff;
				}
				toEmpty.RemoveAt(i);
			}
			toEmpty.Clear();
			toEmpty = null;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Pulse" && destroyPulses)
		{
			MovePulse fluff = other.GetComponent<MovePulse>();
			if (fluff != null && (fluff.attachee == null || !fluff.attachee.possessive))
			{
				if (toDestroy == null)
				{
					toDestroy = new List<GameObject>();
				}
				toDestroy.Add(other.gameObject);
			}
		}

		else if (other.gameObject.tag == "Converser")
		{
			if (destroyPulses)
			{
				FluffSpawn spawn = other.GetComponent<FluffSpawn>();
				if (spawn != null)
				{
					if (toEmpty == null)
					{
						toEmpty = new List<FluffSpawn>();
					}
					toEmpty.Add(spawn);
				}
			}
			if (destroyConnections)
			{
				PartnerLink partnerLink = other.GetComponent<PartnerLink>();
				for (int i = 0; i < partnerLink.connections.Count; i++)
				{
					partnerLink.connections[i].BreakConnection();
				}
			}
		}
	}
}
