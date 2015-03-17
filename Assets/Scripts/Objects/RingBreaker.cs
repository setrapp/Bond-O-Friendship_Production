using UnityEngine;
using System.Collections;

public class RingBreaker : MonoBehaviour {
	public MembraneShell targetRing;
	public Membrane nearestMembrane;
	public float slowDistance;
	public SimpleMover mover;
	public Collider breakerCollider;
	public BondAttachable bondAttachable;
	public float fadeRate = 1.0f;
	private MeshRenderer meshRenderer;

	void Awake()
	{
		if (mover == null)
		{
			mover = GetComponent<SimpleMover>();
		}
		meshRenderer = GetComponent<MeshRenderer>();
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
			if ((target - transform.position).sqrMagnitude > Mathf.Pow(slowDistance, 2) && !nearestMembrane.Breaking)
			{
				mover.Accelerate(target - transform.position);
				Vector3 oldRot = transform.rotation.eulerAngles;
				transform.up = target - transform.position;
				if (transform.rotation.eulerAngles.x != oldRot.x || transform.rotation.eulerAngles.y != oldRot.y)
				{
					transform.rotation = Quaternion.Euler(new Vector3(oldRot.x, oldRot.y, transform.rotation.eulerAngles.z));
				}
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

		if (nearestMembrane != null && (nearestMembrane.Breaking || targetRing.createdWalls.Count != targetRing.wallCount))
		{
			Color fadeColor = meshRenderer.material.color;
			fadeColor.a -= fadeRate * Time.deltaTime;
			meshRenderer.material.color = fadeColor;
		}

		if (meshRenderer.material.color.a <= 0)
		{
			Destroy(gameObject);
		}
	}
}
