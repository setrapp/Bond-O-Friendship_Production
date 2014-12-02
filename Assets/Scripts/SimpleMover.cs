using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class SimpleMover : MonoBehaviour {
	public float maxSpeed;
	public Vector3 velocity;
	public float acceleration;
	public float handling;
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

	void Start()
	{
		if (body == null)
		{
			body = GetComponent<Rigidbody>();
		}
	}

	void FixedUpdate() {

		if (body != null)
		{
			velocity = body.velocity;
		}

		// If slowing down without a rigidbody attached, dampen speed.
		if (slowDown && body == null)
		{
			velocity *= bodylessDampening;
		}

		// Clamp velocity within max speed, taking into account external speed multiplier.
		externalSpeedMultiplier = Mathf.Max(externalSpeedMultiplier, 0);
		if (velocity.sqrMagnitude > Mathf.Pow(maxSpeed, 2) * externalSpeedMultiplier)
		{
			velocity = velocity.normalized * maxSpeed * externalSpeedMultiplier;
		}

		// Move, using rigidbody if attached.
		if (body != null)
		{
			body.velocity = velocity;
		}
		else
		{
			transform.position += velocity * Time.deltaTime;
		}

		// Cut the speed to zero if going slow enough.
		if (velocity.sqrMagnitude < Mathf.Pow(cutSpeedThreshold, 2)) {
			velocity = Vector3.zero;
			body.velocity = Vector3.zero;
			moving = false;
			slowDown = false;
		}
		else
		{
			moving = true;
		}
	}

	public void Stop()
	{
		velocity = Vector3.zero;
		if (body != null)
		{
			body.velocity = Vector3.zero;
		}
	}

	public void Accelerate(Vector3 velocityChange, bool forceFullAcceleration = true, bool forceFullTurning = true)
	{
		// Dividing by deltaTime later so avoid even trying to change velocity if time has not changed.
		if (Time.deltaTime <= 0)
		{
			return;
		}

		Vector3 parallel = velocityChange;
		Vector3 perpendicular = Vector3.zero;

		// If already moving separate acceleration into components parallel and perpendicular to velocity.
		if (velocity.sqrMagnitude > 0)
		{
			parallel = Helper.ProjectVector(velocity, velocityChange);
			perpendicular = velocityChange - parallel;
		}

		// If forcing full acceleration or attempting to accelerate beyond limits, clamp to max acceleration.
		if (forceFullAcceleration || parallel.sqrMagnitude > Mathf.Pow(acceleration, 2))
		{
			parallel = parallel.normalized * acceleration;
		}

		// If forcing full turning or attempting to turn beyond limits, clamp to max handling.
		if (forceFullTurning || perpendicular.sqrMagnitude > Mathf.Pow(handling, 2))
		{
			perpendicular = perpendicular.normalized * handling;
			// Avoid overshooting desired direction.
			if (forceFullTurning)
			{
				Vector3 oldVelPerpAcceleration = velocity - Helper.ProjectVector(velocityChange, velocity);
				Vector3 newVelocity = velocity + perpendicular * Time.deltaTime;
				Vector3 newVelPerpAcceleration = newVelocity - Helper.ProjectVector(velocityChange, newVelocity);
				if (Vector3.Dot(oldVelPerpAcceleration, newVelPerpAcceleration) < 0)
				{
					perpendicular = -oldVelPerpAcceleration / Time.deltaTime;
				}
			}
		}

		// Accelerate by recombining parallel and perpendicular components.
		velocity += (parallel + perpendicular) * Time.deltaTime;

		// Clamp down to max speed and if a rigid body is attached, update it.
		if (velocity.sqrMagnitude > Mathf.Pow(maxSpeed, 2))
		{
			velocity = velocity.normalized * maxSpeed;
		}
		velocity *= Mathf.Max(externalSpeedMultiplier, 0);
		if (body != null && !float.IsNaN(velocity.x))
		{
			body.velocity = velocity;
		}
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
		velocity = direction * speed * Mathf.Max(externalSpeedMultiplier, 0);
		if (body != null && !float.IsNaN(velocity.x))
		{
			body.velocity = velocity;
		}
		transform.position += velocity * Time.deltaTime;
	}
}
