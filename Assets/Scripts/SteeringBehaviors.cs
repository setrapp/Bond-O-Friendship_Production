using UnityEngine;
using System.Collections;

public class SteeringBehaviors : MonoBehaviour {
	public SimpleMover mover;
	/*TODO actual movement should not be done here.*/

	void Start()
	{
		if (mover == null)
		{
			mover = GetComponent<SimpleMover>();
		}
	}


	public void Seek(Vector3 seekTarget, bool arrive = false)
	{
		Vector3 desiredVelocity = seekTarget - transform.position;
		if (!arrive)
		{
			desiredVelocity = desiredVelocity.normalized * mover.maxSpeed;
		}
		mover.Accelerate(desiredVelocity - mover.velocity, !arrive);
	}

	public void Flee(Vector3 fleeTarget)
	{
		if (transform.position == fleeTarget)
		{
			Seek(fleeTarget + -Vector3.right);
		}
		else
		{
			Seek(transform.position + ((transform.position - fleeTarget) * 2));
		}
	}
}
