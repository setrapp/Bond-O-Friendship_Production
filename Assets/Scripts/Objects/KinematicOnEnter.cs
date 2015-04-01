using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KinematicOnEnter : MonoBehaviour {

	public bool toKinematic = true;
	[SerializeField]
	public List<Collider> triggers;
	[SerializeField]
	public List<Rigidbody> targetBodies;

	void OnTriggerEnter(Collider col)
	{
		if (triggers.Contains(col))
		{
			for (int i = 0; i < targetBodies.Count; i++)
			{
				targetBodies[i].isKinematic = toKinematic;
			}
		}
	}

}
