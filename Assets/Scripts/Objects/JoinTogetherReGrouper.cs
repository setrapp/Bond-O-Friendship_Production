using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JoinTogetherReGrouper : MonoBehaviour {

	public JoinTogetherGroup newGroup;
	public List<JoinTogetherGroupConsolidator> triggerConsolidators;

	void Start()
	{
		if (newGroup == null)
		{
			newGroup = GetComponent<JoinTogetherGroup>();
		}

		if (newGroup != null)
		{
			newGroup.enabled = false;
		}
	}
	void Update()
	{
		if (newGroup != null && !newGroup.enabled)
		{
			bool allReady = true;
			for (int i = 0; i < triggerConsolidators.Count; i++)
			{
				if (!triggerConsolidators[i].consolidateReady)
				{
					allReady = false;
				}
			}

			if (allReady)
			{
				newGroup.enabled = true;
				for (int i = 0; i < triggerConsolidators.Count; i++)
				{
					triggerConsolidators[i].Consolidate();
				}
			}
		}
	}
}
