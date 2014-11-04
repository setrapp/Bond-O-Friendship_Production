using UnityEngine;
using System.Collections;

public class FloatMoving : MonoBehaviour {
	public SimpleMover mover;
	private MovementStats startingStats;
	public MovementStats floatStats;
	public LayerMask ignoreLayers;
	private bool wasFloating = false;
	public bool Floating
	{
		get { return wasFloating; }
	}

	void Start()
	{
		if (mover == null)
		{
			mover = GetComponent<SimpleMover>();
		}

		startingStats = new MovementStats();
		startingStats.acceleration = mover.acceleration;
		startingStats.handling = mover.handling;
		startingStats.dampening = mover.dampening;
	}

	void Update()
	{
		RaycastHit hit;
		if(Physics.Raycast(transform.position, Vector3.forward, out hit, Mathf.Infinity, ~ignoreLayers))
		{
			if (wasFloating)
			{
				mover.acceleration = startingStats.acceleration;
				mover.handling = startingStats.handling;
				mover.dampening = startingStats.dampening;
				wasFloating = false;
			}
		}
		else if (!wasFloating)
		{
			mover.acceleration = floatStats.acceleration;
			mover.handling = floatStats.handling;
			mover.dampening = floatStats.dampening;
			wasFloating = true;
		}
	}
}

[System.Serializable]
public class MovementStats
{
	public float acceleration;
	public float handling;
	public float dampening;
}
