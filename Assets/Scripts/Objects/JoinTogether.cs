using UnityEngine;
using System.Collections;

public class JoinTogether : MonoBehaviour {

	public Rigidbody baseBody = null;
	public ConstrainOnDirection movementConstraint;
	public JoinTogetherPair moveable;
	public JoinTogetherPair sepatationTarget;
	public JoinTogetherPair joinTarget;
	private Vector3 oldMoveablePosition;
	public bool atJoin = false;

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
		if (movementConstraint == null)
		{
			movementConstraint = moveable.baseObject.GetComponent<ConstrainOnDirection>();
		}

		EstablishConstraints();
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
			float progressDirection = Vector3.Dot(separationToMoveable, separationToJoin);

			if (progress > 1 && progressDirection > 0)
			{
				if (baseBody != null)
				{
					baseBody.MovePosition(joinTarget.baseObject.transform.position);
					if (!baseBody.isKinematic)
					{
						baseBody.velocity = Vector3.zero;
					}
				}
				else
				{
					moveable.baseObject.transform.position = joinTarget.baseObject.transform.position;
				}
				progress = 1;
			}
			else if (progressDirection < 0)
			{
				if (baseBody != null)
				{
					baseBody.MovePosition(sepatationTarget.baseObject.transform.position);
					if (!baseBody.isKinematic)
					{
						baseBody.velocity = Vector3.zero;
					}
				}
				else
				{
					moveable.baseObject.transform.position = sepatationTarget.baseObject.transform.position;
				}
				progress = 0;
			}

			if (progress >= 1)
			{
				atJoin = true;
			}

			moveable.pairedObject.transform.position = sepatationTarget.pairedObject.transform.position + ((joinTarget.pairedObject.transform.position - sepatationTarget.pairedObject.transform.position) * progress);

			oldMoveablePosition = moveable.baseObject.transform.position;
		}
	}

	public void EstablishConstraints()
	{
		if (movementConstraint != null && baseBody != null && sepatationTarget.baseObject != null && joinTarget.baseObject != null)
		{
			movementConstraint.directionSpace = Space.World;
			movementConstraint.constrainToDirection = (joinTarget.baseObject.transform.position - sepatationTarget.baseObject.transform.position).normalized;
		}
	}
}

[System.Serializable]
public class JoinTogetherPair
{
	public GameObject baseObject;
	public GameObject pairedObject;
}

