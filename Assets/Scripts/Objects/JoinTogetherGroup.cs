using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JoinTogetherGroup : MonoBehaviour {

	public bool allChildJoins = true;
	[SerializeField]
	public List<JoinTogether> joins;
	public JoinTogetherPair groupJoinTarget;

	void Start()
	{
		if (allChildJoins)
		{
			JoinTogether[] childJoins = GetComponentsInChildren<JoinTogether>();
			for (int i = 0; i < childJoins.Length; i++)
			{
				if (!joins.Contains(childJoins[i]))
				{
					joins.Add(childJoins[i]);
				}
			}
		}

		for (int i = 0; i < joins.Count; i++)
		{
			if (joins[i].joinTarget.baseObject == null)
			{
				joins[i].joinTarget.baseObject = groupJoinTarget.baseObject;
			}
			if (joins[i].joinTarget.pairedObject == null)
			{
				joins[i].joinTarget.pairedObject = groupJoinTarget.pairedObject;
			}

			// Attach spring joints to join pieces to prevent collisions.
			for (int j = i + 1; j < joins.Count; j++)
			{
				if (joins[i].baseBody != null && joins[j].baseBody != null)
				{
					SpringJoint noCollide = joins[i].baseBody.gameObject.AddComponent<SpringJoint>();
					noCollide.connectedBody = joins[j].baseBody;
					noCollide.spring = 0;
					noCollide.damper = 0;
					noCollide.anchor = Vector3.zero;
					noCollide.autoConfigureConnectedAnchor = false;
					noCollide.connectedAnchor = Vector3.zero;
				}
				
			}
		}
	}

	void Update()
	{
		bool allJoined = true;
		for (int i = 0; i < joins.Count && allJoined; i++)
		{
			if (!joins[i].atJoin)
			{
				allJoined = false;
			}
		}

		if (allJoined)
		{
			for (int i = 0; i < joins.Count && allJoined; i++)
			{
				if (joins[i].baseBody != null)
				{
					joins[i].baseBody.isKinematic = true;
				}
			}
		}
	}
}
