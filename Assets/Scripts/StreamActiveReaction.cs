using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StreamActiveReaction : StreamReaction {

	public bool toActive = true;
	[SerializeField]
	public List<GameObject> reactionObjects;
	public bool startOpposite = true;

	protected override void Start()
	{
		base.Start();
		if (startOpposite)
		{
			for (int i = 0; i < reactionObjects.Count; i++)
			{
				if (reactionObjects[i] != null)
				{
					reactionObjects[i].SetActive(!toActive);
				}
			}
		}
	}

	public override bool React(float actionRate)
	{
		bool reacted = base.React(actionRate);
		if (reacted)
		{
			if (actionRate >= 0 && reactionProgress >= 1)
			{
				for (int i = 0; i < reactionObjects.Count; i++)
				{
					if (reactionObjects[i] != null)
					{
						reactionObjects[i].SetActive(toActive);
					}
				}
			}
			else if (actionRate < 0 && reactionProgress < 1)
			{
				for (int i = 0; i < reactionObjects.Count; i++)
				{
					if (reactionObjects[i] != null)
					{
						reactionObjects[i].SetActive(toActive);
					}
				}
			}
		}
		return reacted;
	}
}
