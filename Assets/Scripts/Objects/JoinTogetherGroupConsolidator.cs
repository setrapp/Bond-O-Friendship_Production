using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JoinTogetherGroupConsolidator : MonoBehaviour {

	public JoinTogetherGroup triggerGroup;
	public bool consolidated = false;
	public List<Rigidbody> joinedBodies;
	public List<JoinTogether> joinedPieces;

	void Start()
	{
		if (triggerGroup == null)
		{
			triggerGroup = GetComponent<JoinTogetherGroup>();
		}
	}

	void Update()
	{
		if (triggerGroup != null && triggerGroup.solved && !consolidated)
		{
			triggerGroup.enabled = false;

			// Fix joined bodies together and ensure that they are not kinematic.
			for (int i = 0; i < joinedBodies.Count; i++)
			{
				if (i < joinedBodies.Count - 1)
				{
					FixedJoint newJoint = joinedBodies[i].gameObject.AddComponent<FixedJoint>();
					newJoint.connectedBody = joinedBodies[i + 1];
				}
			}
			
			//
			for (int i = 0; i < joinedPieces.Count; i++)
			{
				if (joinedPieces[i].movementConstraint != null)
				{
					//TODO actually target new center, or maybe the join together group will do it
					//joinedPieces[i].movementConstraint.ResetWithDirection(joinedPieces[i].movementConstraint.constrainToDirection);
					//joinedPieces[i].movementConstraint.enabled = false;
				}
				// TODO just set it to not 'atJoin' fixed joint should keep the bits together.
				//joinedPieces[i].enabled = false;
				joinedPieces[i].atJoin = false;
				joinedPieces[i].joinTarget.baseObject = null;
				joinedPieces[i].joinTarget.pairedObject = null;
			}
			
			consolidated = true;
		}
	}

	public void ResetConstraints()
	{
		for (int i = 0; i < joinedBodies.Count; i++)
		{
			joinedBodies[i].isKinematic = false;
		}

		for (int i = 0; i < joinedPieces.Count; i++)
		{
			if (joinedPieces[i].movementConstraint != null)
			{
				//TODO actually target new center, or maybe the join together group will do it
				joinedPieces[i].movementConstraint.ResetWithDirection(joinedPieces[i].movementConstraint.constrainToDirection);
				joinedPieces[i].separationTarget.baseObject.transform.position = joinedPieces[i].moveable.baseObject.transform.position;
				joinedPieces[i].separationTarget.pairedObject.transform.position = joinedPieces[i].moveable.pairedObject.transform.position;
			}
		}
	}
}
