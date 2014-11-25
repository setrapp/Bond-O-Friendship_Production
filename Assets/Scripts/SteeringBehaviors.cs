using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SteeringBehaviors : MonoBehaviour {
	public SimpleMover mover;
	public Vector3 desiredVelocity;
	public Vector3 steeringForce;
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
		desiredVelocity = seekTarget - transform.position;
		if (!arrive)
		{
			desiredVelocity = desiredVelocity.normalized * mover.maxSpeed;
		}

		steeringForce = desiredVelocity - mover.velocity;

		mover.Accelerate(steeringForce, !arrive, false);
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

	public void Pursue(GameObject pursuee, bool arrive = false)
	{
		Pursue(pursuee, Vector3.zero, false, arrive);
	}

	public void Pursue(GameObject pursuee, float distance, bool acceptWithinProximity = true, bool arrive = false)
	{
		// Only seek if still far away || getting close is not good enough.
		Vector3 fromPursuee = transform.position - pursuee.transform.position;
		if (!acceptWithinProximity || fromPursuee.sqrMagnitude > Mathf.Pow(distance, 2))
		{
			Seek(pursuee.transform.position + (fromPursuee.normalized * distance), arrive);
		}
	}

	public void Pursue(GameObject pursuee, Vector3 offset, bool acceptWithinProximity = true, bool arrive = false)
	{
		Vector3 worldOffset = pursuee.transform.TransformDirection(offset);
		bool needSeek = true;

		// Only seek if still far away || getting close is not good enough.
		if (acceptWithinProximity)
		{
			Vector3 toPursuee = pursuee.transform.position - transform.position;
			if (toPursuee.sqrMagnitude <= worldOffset.sqrMagnitude && Vector3.Dot(-toPursuee, worldOffset) > 0)
			{
				needSeek = false;
			}
		}
		
		if (needSeek)
		{
			Seek(pursuee.transform.position + worldOffset, arrive);
		}
	}

	public void AvoidObstacles(float checkDistance, float checkRadius)
	{
		if (checkDistance <= 0 || checkRadius < 0)
		{
			return;
		}

		if (checkDistance < checkRadius)
		{
			checkDistance = checkRadius;
		}

		Vector3 startPoint = transform.position + (transform.forward * checkRadius);
		Vector3 endPoint = (transform.position + (transform.forward * checkDistance)) - (transform.forward * checkRadius);
		if (Physics.CheckCapsule(startPoint, endPoint, checkRadius))
		{
			RaycastHit[] potentialHits = Physics.CapsuleCastAll(startPoint, endPoint, checkRadius, transform.forward);
			List<RaycastHit> obstacleHits = new List<RaycastHit>();
			for (int i = 0; i < potentialHits.Length; i++)
			{
				bool hitSelf = (potentialHits[i].collider.gameObject == gameObject);
				bool ignoreHit = Physics.GetIgnoreLayerCollision(gameObject.layer, potentialHits[i].collider.gameObject.layer);
				if (!(hitSelf || ignoreHit))
				{
					obstacleHits.Add(potentialHits[i]);
					Debug.Log(obstacleHits[i].collider.gameObject.name + " " + potentialHits[i].distance);
				}
			}

			for (int i = 0; i < obstacleHits.Count; i++)
			{
				Vector3 toObstacle = (obstacleHits[i].collider.gameObject.transform.position - transform.position);
				Vector3 projToObstacle = Helper.ProjectVector(transform.right, toObstacle);
				steeringForce += projToObstacle.normalized * mover.handling;// *Mathf.Max(1 - (toObstacle.magnitude / checkDistance), 0);
				//Debug.Log(obstacleHits[i].collider.gameObject.name + " " + steeringForce);
			}
			Debug.Log("-----");
		}
		mover.Accelerate(steeringForce, false, true);
	}
}
