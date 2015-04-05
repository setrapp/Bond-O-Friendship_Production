using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class SimpleMover : MonoBehaviour {
	public float maxSpeed;
	public Vector3 velocity;
	private Vector3 unfixedVelocity;
	public float acceleration;
	public float handling;
	public float minHandlingFactor = 1.0f;
	private float currentHandling;
	public float cutSpeedThreshold = 0.1f;
	public float externalSpeedMultiplier = 1;
	private bool moving;
	public bool Moving
	{
		get { return moving; }
	}
	public Rigidbody body;
	public float bodylessDampening = 1;
	public bool slowDown = false;
	public bool jumpToMaxSpeed = false;

	void Awake()
	{
		if (body == null)
		{
			body = GetComponent<Rigidbody>();
		}
	}

	void FixedUpdate() {

		if (jumpToMaxSpeed && unfixedVelocity.sqrMagnitude > 0)
		{
			//unfixedVelocity = unfixedVelocity.normalized * maxSpeed;
			//jumpToMaxSpeed = false;
		}

		if (unfixedVelocity != velocity)
		{
			velocity = unfixedVelocity;
			if (body != null)
			{
				body.velocity = velocity;
			}
		}
		else
		{
			if (body != null)
			{
				velocity = body.velocity;
				
			}
			unfixedVelocity = velocity;
		}

		// If slowing down without a rigidbody attached, dampen speed.
		if (slowDown && body == null)
		{
			velocity *= bodylessDampening;
		}

		// Move, using rigidbody if attached.
		if (body != null && !body.isKinematic)
		{
			body.velocity = velocity;
		}
		else
		{
			transform.position += velocity * Time.deltaTime;
		}
		unfixedVelocity = velocity;

		// Cut the speed to zero if going slow enough.
		if (velocity.sqrMagnitude <= Mathf.Pow(cutSpeedThreshold, 2)) {
			velocity = Vector3.zero;
			unfixedVelocity = Vector3.zero;
			if (body != null && !body.isKinematic)
			{
				body.velocity = Vector3.zero;
			}			
			moving = false;
			slowDown = false;
			currentHandling = 0;
		}
		else
		{
			moving = true;
		}
	}

	public void Stop()
	{
		velocity = Vector3.zero;
		unfixedVelocity = Vector3.zero;
		if (body != null && !body.isKinematic)
		{
			body.velocity = Vector3.zero;
		}
		moving = false;
	}

	public void Accelerate(Vector3 velocityChange, bool clamp = true, bool forceFullAcceleration = true, bool forceFullTurning = true)
	{
		// Dividing by deltaTime later so avoid even trying to change velocity if time has not changed.
		if (Time.deltaTime <= 0)
		{
			return;
		}

		if (clamp)
		{
			velocityChange = ClampMovementChange(velocityChange, forceFullAcceleration, forceFullTurning);
		}

		// Accelerate by recombining parallel and perpendicular components.
		unfixedVelocity += velocityChange * Time.deltaTime;

		// Clamp down to max speed and if a rigid body is attached, update it.
		if (unfixedVelocity.sqrMagnitude > Mathf.Pow(maxSpeed, 2))
		{
			unfixedVelocity = unfixedVelocity.normalized * maxSpeed;
		}
		unfixedVelocity *= Mathf.Max(externalSpeedMultiplier, 0);
	}

	public void Move(Vector3 direction, float speed, bool clampSpeed = true)
	{
		if (direction.sqrMagnitude != 1)
		{
			direction.Normalize();
		}
		if (clampSpeed && speed > maxSpeed)
		{
			speed = maxSpeed;
		}
		unfixedVelocity = direction * speed * Mathf.Max(externalSpeedMultiplier, 0);
	}

	public Vector3 ClampMovementChange(Vector3 moveVector, bool forceFullAcceleration = true, bool forceFullTurning = true)
	{
		Vector3 parallel = Vector3.zero;
		Vector3 perpendicular = Vector3.zero;

		// If already moving separate acceleration into components parallel and perpendicular to velocity.
		Vector3 baseDirection = velocity;
		if (velocity.sqrMagnitude <= 0)
		{
			baseDirection = transform.forward;
		}

		/*TODO make player not turn instantly when stationary*/

		parallel = Helper.ProjectVector(baseDirection, moveVector);
		perpendicular = moveVector - parallel;

		// If forcing full acceleration or attempting to accelerate beyond limits, clamp to max acceleration.
		if (forceFullAcceleration || parallel.sqrMagnitude > Mathf.Pow(acceleration, 2))
		{
			parallel = parallel.normalized * acceleration;
		}

		SetCurrentHandling(moveVector);

		// If forcing full turning or attempting to turn beyond limits, clamp to max handling.
		float absoluteHandling = Mathf.Abs(currentHandling);
		if (forceFullTurning || perpendicular.sqrMagnitude > Mathf.Pow(absoluteHandling, 2))
		{
			perpendicular = perpendicular.normalized * absoluteHandling;

			// Avoid overshooting desired direction.
			if (forceFullTurning)
			{
				Vector3 oldVelPerpAcceleration = velocity - Helper.ProjectVector(moveVector, baseDirection);
				Vector3 newVelocity = velocity + perpendicular * Time.deltaTime;
				Vector3 newVelPerpAcceleration = newVelocity - Helper.ProjectVector(moveVector, newVelocity);
				if (Vector3.Dot(oldVelPerpAcceleration, newVelPerpAcceleration) < 0)
				{
					perpendicular = -oldVelPerpAcceleration / Time.deltaTime;
				}
			}
		}

		return parallel + perpendicular;
	}

	private void SetCurrentHandling(Vector3 turningDirection)
	{
		currentHandling = handling;

		float speedBasedHandling = handling;
		if (minHandlingFactor > 1)
		{
			minHandlingFactor = 1;
		}
		else if (minHandlingFactor < 1)
		{
			speedBasedHandling = Mathf.Min((minHandlingFactor * handling) + ((velocity.magnitude / maxSpeed) * ((1 - minHandlingFactor) * handling)), handling);
		}

		if (currentHandling > speedBasedHandling)
		{
			currentHandling = speedBasedHandling;
		}
		else if (currentHandling < -speedBasedHandling)
		{
			currentHandling = -speedBasedHandling;
		}
	}
}
