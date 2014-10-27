using UnityEngine;
using System.Collections;

public class SteeredCharacter : MonoBehaviour {
	public Vector3 target;
	public float slowDistance;
	public bool seeking;
	public SteeringBehaviors steering;
	
	void Start()
	{
		if (steering == null)
		{
			steering = GetComponent<SteeringBehaviors>();
		}
	}

	void Update()
	{
		if (steering != null)
		{
			if (seeking)
			{
				//steering.Arrive(target, slowDistance);
				steering.Seek(target, true);
			}
			else
			{
				steering.Flee(target);
			}
		}
	}

	void OnDrawGizmos()
	{
		if (steering != null && steering.mover != null)
		{
			Gizmos.color = Color.white;
			Gizmos.DrawLine(transform.position, transform.position + steering.mover.velocity);
		}
	}
}
