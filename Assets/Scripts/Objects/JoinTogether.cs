using UnityEngine;
using System.Collections;

public class JoinTogether : MonoBehaviour {

	public Rigidbody baseBody = null;
	public JoinTogetherPair moveable;
	public JoinTogetherPair sepatationTarget;
	public JoinTogetherPair joinTarget;
	private Vector3 oldMoveablePosition;
	public bool atJoin = false;
	private float progressThreshold = 0.95f;

	void Awake()
	{
		if (moveable.baseObject == null)
		{
			moveable.baseObject = gameObject;
		}
		if (baseBody == null)
		{
			baseBody = moveable.baseObject.GetComponent<Rigidbody>();
		}
	}

	void Update()
	{
		if (moveable.baseObject.transform.position != oldMoveablePosition)
		{
			atJoin = false;

			Vector3 separationToMoveable = moveable.baseObject.transform.position - sepatationTarget.baseObject.transform.position;
			Vector3 separationToJoin = joinTarget.baseObject.transform.position - sepatationTarget.baseObject.transform.position;

			separationToMoveable = Helper.ProjectVector(separationToJoin, separationToMoveable);
			float progress = separationToMoveable.magnitude / separationToJoin.magnitude;

			if (progress > 1)
			{
				if (baseBody != null)
				{
					baseBody.MovePosition(joinTarget.baseObject.transform.position);
				}
				else
				{
					moveable.baseObject.transform.position = joinTarget.baseObject.transform.position;
				}
				progress = 1;
			}

			if (progress >= progressThreshold)
			{
				atJoin = true;
			}

			moveable.pairedObject.transform.position = sepatationTarget.pairedObject.transform.position + ((joinTarget.pairedObject.transform.position - sepatationTarget.pairedObject.transform.position) * progress);

			oldMoveablePosition = moveable.baseObject.transform.position;
		}
	}
}

[System.Serializable]
public class JoinTogetherPair
{
	public GameObject baseObject;
	public GameObject pairedObject;
}

