using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class SimpleMover : MonoBehaviour {
	public float maxSpeed;
	public Vector3 velocity;
	private Vector3 oldVelocity;
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
	private bool slowingDown = false;

	void Start()
	{
		body = GetComponent<Rigidbody>();
	}

	void FixedUpdate() {

		// If slowing down either recognize rigidbody drag or apply dampening.
		if (slowingDown)
		{
			if (body != null)
			{
				velocity = body.velocity;
			}
			else
			{
				velocity *= bodylessDampening;
			}
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
		}
		else
		{
			moving = true;
		}

		oldVelocity = velocity;
		slowingDown = false;
	}

	public void Stop()
	{
		velocity = Vector3.zero;
	}

	public void Accelerate(Vector3 velocityChange, bool forceFullAcceleration = true)
	{
		if (forceFullAcceleration && velocityChange.sqrMagnitude != 1)
		{
			velocityChange.Normalize();
		}

		if (velocity.sqrMagnitude <= 0)
		{
			if (forceFullAcceleration)
			{
				velocity += velocityChange * acceleration * Time.deltaTime;
			}
			else if (velocityChange.sqrMagnitude > Mathf.Pow(acceleration, 2))
			{
				velocity += velocityChange.normalized * acceleration * Time.deltaTime;
			}
			else
			{
				velocity += velocityChange * Time.deltaTime;
			}
		}
		else 
		{
			Vector3 parallel = Helper.ProjectVector(velocity, velocityChange);
			Vector3 perpendicular = velocityChange - parallel;

			if (forceFullAcceleration)
			{
				parallel *= acceleration * Time.deltaTime;
				perpendicular *= handling * Time.deltaTime;
			}
			else
			{
				if (parallel.sqrMagnitude > Mathf.Pow(acceleration, 2))
				{
					parallel = parallel.normalized * acceleration * Time.deltaTime;
				}
				if (perpendicular.sqrMagnitude > Mathf.Pow(handling, 2))
				{
					perpendicular = perpendicular.normalized * handling * Time.deltaTime;
				}
			}

			velocity += (parallel + perpendicular);
		}

		if (velocity.sqrMagnitude > Mathf.Pow(maxSpeed, 2))
		{
			velocity = velocity.normalized * maxSpeed;
		}
		velocity *= Mathf.Max(externalSpeedMultiplier, 0);
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
		transform.position += velocity * Time.deltaTime;
	}

	public void SlowDown()
	{
		slowingDown = true;
	}
}
