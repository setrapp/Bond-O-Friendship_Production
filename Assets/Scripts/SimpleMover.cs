using UnityEngine;
using System.Collections;

public class SimpleMover : MonoBehaviour {
	public float maxSpeed;
	public Vector3 velocity;
	private Vector3 oldVelocity;
	public float acceleration;
	public float handling;
	public float dampening = 0.9f;
	public float dampeningThreshold = 0.005f;
	public float externalSpeedMultiplier = 1;
	private bool moving;
	public bool Moving
	{
		get { return moving; }
	}
	private CharacterController controller;
	private Rigidbody rigidbody;

	void Start()
	{
		controller = GetComponent<CharacterController>();
		rigidbody = GetComponent<Rigidbody>();
	}

	void FixedUpdate() {
		// TODO NOPE.
		externalSpeedMultiplier = 1;

		externalSpeedMultiplier = Mathf.Max(externalSpeedMultiplier, 0);

		if (velocity.sqrMagnitude > Mathf.Pow(maxSpeed, 2) * externalSpeedMultiplier)
		{
			velocity = velocity.normalized * maxSpeed * externalSpeedMultiplier;
		}

		if (rigidbody != null)
		{
			rigidbody.velocity = velocity;
		}
		else
		{
			transform.position += velocity * Time.deltaTime;
		}

		if (velocity.sqrMagnitude < Mathf.Pow(dampeningThreshold, 2)) {
			velocity = Vector3.zero;
			rigidbody.velocity = Vector3.zero;
			moving = false;
		}
		else
		{
			moving = true;
		}

		oldVelocity = velocity;
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
		if (controller != null)
		{
			controller.Move(velocity * Time.deltaTime);
		}
		else
		{
			transform.position += velocity * Time.deltaTime;
		}
	}

	/*public void MoveTo(Vector3 position, bool updateVelocity = false)
	{
		if (updateVelocity && Time.deltaTime > 0)
		{
			velocity = (position - transform.position) / Time.deltaTime;
		}
		if (controller != null)
		{
			controller.Move(position - transform.position);
		}
		else
		{
			transform.position = position;
		}

	}*/

	public void SlowDown()
	{
		velocity *= dampening;
	}
}
