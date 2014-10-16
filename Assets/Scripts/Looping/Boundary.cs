using UnityEngine;
using System.Collections;

public class Boundary : MonoBehaviour {

	public TriggerLooping.ColliderLocation colliderLocation;

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Converser")
		{
			if (other.GetComponent<PartnerLink>().isPlayer)
				transform.parent.GetComponent<TriggerLooping>().MoveWorld(colliderLocation);
			else 
			{
				LoopTag loopTag = other.GetComponentInChildren<LoopTag>();
				if (loopTag != null && !loopTag.passThrough)
					transform.parent.GetComponent<TriggerLooping>().MoveIndividual(colliderLocation, loopTag.gameObject, other.GetComponent<Tracer>());
			}
		}
	}
}
