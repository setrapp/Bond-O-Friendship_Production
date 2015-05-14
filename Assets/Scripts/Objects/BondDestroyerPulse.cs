using UnityEngine;
using System.Collections;

public class BondDestroyerPulse : MonoBehaviour {

	public BondDestroyer creator;
	public Renderer renderer;
	public RingPulse pulseBase;

	void OnCollisionEnter(Collision col)
	{
		if (creator != null)
		{
			creator.AttemptDestoy(col.collider, this);
		}
		
	}

	void OnCollisionStay(Collision col)
	{
		if (creator != null)
		{
			creator.AttemptDestoy(col.collider, this);
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if (creator != null)
		{
			creator.AttemptDestoy(col, this);
		}
	}

	void OnTriggerStay(Collider col)
	{
		if (creator != null)
		{
			creator.AttemptDestoy(col, this);
		}
	}
}
