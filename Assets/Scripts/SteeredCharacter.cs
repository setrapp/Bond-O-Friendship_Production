using UnityEngine;
using System.Collections;

public class SteeredCharacter : MonoBehaviour {
	public Vector3 target;
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
				steering.Seek(target);
			}
			else
			{
				steering.Flee(target);
			}
		}
	}
}
