using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JoinTogetherGroup : MonoBehaviour {

	public bool allChildJoins = true;
	public bool includePairs = true;
	[SerializeField]
	public List<JoinTogether> joins;
	public JoinTogetherPair groupJoinTarget;
	public bool solved = false;
	public PulseStats basePulseStats;
	public PulseStats pairedPulseStats;
	public List<GameObject> joinCompleteActivatees;

	void Start()
	{
		// If desired, find and attach all child joins.
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

		// Direct all attached joins to the specified group target.
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
			joins[i].EstablishConstraints();
		}

		// If not using paired objects, hide them.
		if (!includePairs) {
			if (groupJoinTarget.pairedObject != null)
			{
				groupJoinTarget.pairedObject.gameObject.SetActive(false);
			}
			for (int i = 0; i < joins.Count; i++)
			{
				joins[i].HidePaired();
			}
		}
	}

	void Update()
	{
		if (!solved)
		{
			// Check if all joins are in the joining position.
			bool allJoined = true;
			for (int i = 0; i < joins.Count && allJoined; i++)
			{
				if (!joins[i].atJoin)
				{
					allJoined = false;
				}
			}

			// If all joins are in position, stop their movement, place them all at join goal, and solve the puzzle.
			if (allJoined)
			{
				for (int i = 0; i < joins.Count && allJoined; i++)
				{
					joins[i].JumpToJoinGoal();
					if (joins[i].baseBody != null)
					{
						joins[i].baseBody.isKinematic = true;
					}
				}

				for (int i = 0; i < joinCompleteActivatees.Count; i++)
				{
					joinCompleteActivatees[i].gameObject.SetActive(true);
				}

				solved = true;
				Helper.FirePulse(groupJoinTarget.baseObject.transform.position, basePulseStats);
				if (includePairs)
				{
					Helper.FirePulse(groupJoinTarget.pairedObject.transform.position, pairedPulseStats);
				}
			}
		}
	}
}
