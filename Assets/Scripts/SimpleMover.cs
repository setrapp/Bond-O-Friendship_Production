using UnityEngine;
using System.Collections;

public class SimpleMover : MonoBehaviour {
	public float maxSpeed;
	public Vector3 velocity;
	public float acceleration;
	public float handling;
	public float dampening = 0.9f;
	public float dampeningThreshold = 0.005f;
	public float externalSpeedMultiplier = 1;
	public SimpleFreeze freeze;
	private bool moving;
	public bool Moving
	{
		get { return moving; }
	}
	private CharacterController controller;

	void Start()
	{
		controller = GetComponent<CharacterController>();
	}

	void Update() {
		externalSpeedMultiplier = Mathf.Max(externalSpeedMultiplier, 0);

		if (velocity.sqrMagnitude > Mathf.Pow(maxSpeed, 2) * externalSpeedMultiplier)
		{
			velocity = velocity.normalized * maxSpeed * externalSpeedMultiplier;
		}

		ApplyFreezes();
		if (controller != null)
		{
			Vector3 beforeFreeze = transform.position;
			controller.Move(velocity * Time.deltaTime);
			Vector3 afterFreeze = transform.position;
			if (freeze.velocityX)
			{
				afterFreeze.x = beforeFreeze.x;
			}
			if (freeze.velocityY)
			{
				afterFreeze.y = beforeFreeze.y;
			}
			if (freeze.velocityZ)
			{
				afterFreeze.z = beforeFreeze.z;
			}
			transform.position = afterFreeze;
		}
		else
		{
			transform.position += velocity * Time.deltaTime;
		}

		if (velocity.sqrMagnitude < Mathf.Pow(dampeningThreshold, 2)) {
			velocity = Vector3.zero;
			moving = false;
		}
		else
		{
			moving = true;
		}
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
		ApplyFreezes();
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
		ApplyFreezes();
		if (controller != null)
		{
			controller.Move(velocity * Time.deltaTime);
		}
		else
		{
			transform.position += velocity * Time.deltaTime;
		}
	}

	public void MoveTo(Vector3 position, bool updateVelocity = false)
	{
		if (updateVelocity && Time.deltaTime > 0)
		{
			velocity = (position - transform.position) / Time.deltaTime;
			ApplyFreezes();
		}
		if (controller != null)
		{
			controller.Move(position - transform.position);
		}
		else
		{
			transform.position = position;
		}

	}

	public void SlowDown()
	{
		velocity *= dampening;
	}

	private void ApplyFreezes()
	{
		if (freeze.velocityX)
		{
			velocity.x = 0;
		}
		if (freeze.velocityY)
		{
			velocity.y = 0;
		}
		if (freeze.velocityZ)
		{
			velocity.z = 0;
		}
	}
}

[System.Serializable]
public class SimpleFreeze
{
	public bool velocityX;
	public bool velocityY;
	public bool velocityZ;
}
