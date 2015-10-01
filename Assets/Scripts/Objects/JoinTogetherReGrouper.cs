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
			bool allConsolidated = true;
			for (int i = 0; i < triggerConsolidators.Count; i++)
			{
				if (!triggerConsolidators[i].consolidated)
				{
					allConsolidated = false;
				}
			}

			if (allConsolidated)
			{
				for (int i = 0; i < triggerConsolidators.Count; i++)
				{
					triggerConsolidators[i].ResetConstraints();
				}
				newGroup.enabled = true;
			}
		}
	}
}
