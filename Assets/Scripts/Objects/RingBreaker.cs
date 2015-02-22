using UnityEngine;
using System.Collections;

public class RingBreaker : MonoBehaviour {
	public MembraneShell targetRing;
	public Membrane nearestMembrane;
	public float slowDistance;
	public SimpleMover mover;
	public Collider breakerCollider;
	public BondAttachable bondAttachable;

	void Awake()
	{
		if (mover == null)
		{
			mover = GetComponent<SimpleMover>();
		}
	}

	void Update()
	{
		if (nearestMembrane == null && targetRing != null)
		{
			float minSqrDist = -1;
			for (int i = 0; i < targetRing.createdWalls.Count; i++)
			{
				float sqrDist = (targetRing.createdWalls[i].membraneCreator.createdBond.NearestPoint(transform.position) - transform.position).sqrMagnitude;
				if (minSqrDist < 0 || sqrDist < minSqrDist)
				{
					minSqrDist = sqrDist;
					nearestMembrane = targetRing.createdWalls[i].membraneCreator.createdBond as Membrane;
				}
			}
		}

		if (nearestMembrane != null && mover != null)
		{
			Vector3 target = nearestMembrane.NearestNeighboredPoint(transform.position);
			if ((target - transform.position).sqrMagnitude > Mathf.Pow(slowDistance, 2))
			{
				mover.Accelerate(target - transform.position);
			}
			else
			{
				mover.slowDown = true;
				if (collider != null)
				{
					collider.enabled = true;
				}
			}
		}

		if (nearestMembrane != null && targetRing.createdWalls.Count != targetRing.wallCount)
		{
			Destroy(gameObject);
		}
	}
}
