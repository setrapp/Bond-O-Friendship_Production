using UnityEngine;
using System.Collections;

public class JoinTogether : MonoBehaviour {

	public Rigidbody baseBody = null;
	public ConstrainOnDirection movementConstraint;
	public JoinTogetherPair moveable;
	public JoinTogetherPair sepatationTarget;
	public JoinTogetherPair joinTarget;
	private Vector3 oldMoveablePosition;
	public float requiredProgress = 0.975f;
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
		requiredProgress = Mathf.Clamp01(requiredProgress);

		if (moveable.baseObject.transform.position != oldMoveablePosition)
		{
			atJoin = false;

			Vector3 separationToMoveable = moveable.baseObject.transform.position - sepatationTarget.baseObject.transform.position;
			Vector3 separationToJoin = joinTarget.baseObject.transform.position - sepatationTarget.baseObject.transform.position;

			separationToMoveable = Helper.ProjectVector(separationToJoin, separationToMoveable);
			float progress = separationToMoveable.magnitude / separationToJoin.magnitude;
			float progressDirection = Vector3.Dot(separationToMoveable, separationToJoin);

			// If the base body is beyond the most separated point, place it at that point.
			if (progressDirection < 0)
			{
				if (baseBody != null)
				{
					if (!baseBody.isKinematic)
					{
						baseBody.velocity = Vector3.zero;
					}
				}
				moveable.baseObject.transform.position = sepatationTarget.baseObject.transform.position;
				progress = 0;
			}
			// Else if the base body is close enough to the join goal, but not beyond, flag it as ready to join.
			else if (progress >= requiredProgress)
			{
				// If beyond the join goal, place it there.
				if (progress > 1)
				{
					JumpToJoinGoal();
					progress = 1;
				}
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

	public void JumpToJoinGoal()
	{
		if (baseBody != null)
		{
			if (!baseBody.isKinematic)
			{
				baseBody.velocity = Vector3.zero;
			}
		}
		moveable.baseObject.transform.position = joinTarget.baseObject.transform.position;
	}
}

[System.Serializable]
public class JoinTogetherPair
{
	public GameObject baseObject;
	public GameObject pairedObject;
}

