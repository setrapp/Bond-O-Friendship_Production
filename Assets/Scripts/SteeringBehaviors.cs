using UnityEngine;
using System.Collections;

public class SteeringBehaviors : MonoBehaviour {
	public SimpleMover mover;

	void Start()
	{
		if (mover == null)
		{
			mover = GetComponent<SimpleMover>();
		}
	}


	public void Seek(Vector3 seekTarget)
	{
		mover.Accelerate(seekTarget - transform.position);
	}

	public void Flee(Vector3 fleeTarget)
	{
		mover.Accelerate(transform.position- fleeTarget);
	}

}
