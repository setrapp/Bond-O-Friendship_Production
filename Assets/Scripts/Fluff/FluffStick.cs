using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FluffStick : MonoBehaviour {
	public FluffStickRoot root;
	public Fluff stuckFluff;
	public Collider stickingCollider;
	public Vector3 stickOffset = Vector3.zero;
	public Vector3 stickDirection = Vector3.up;

	public void Awake()
	{
		if (stickingCollider == null)
		{
			stickingCollider = GetComponent<Collider>();
		}
	}

	public void AddPullForce(Vector3 pullForce, Vector3 position)
	{
		if (root != null)
		{
			root.AddPullForce(pullForce, position);
		}
	}
}
