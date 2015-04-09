using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StreamReactionList : StreamReaction {

	[SerializeField]
	public List<StreamReaction> streamReactions;
	public bool trackObjectReactions = true;
	public bool trackChildReactions = true;

	void Awake()
	{
		if (trackObjectReactions)
		{
			StreamReaction[] objectReactions = GetComponents<StreamReaction>();
			for (int i = 0; i < objectReactions.Length; i++)
			{
				if (objectReactions[i] != this && !streamReactions.Contains(objectReactions[i]))
				{
					streamReactions.Add(objectReactions[i]);
				}
			}
		}
		if (trackChildReactions)
		{
			StreamReaction[] childReactions = GetComponentsInChildren<StreamReaction>();
			for (int i = 0; i < childReactions.Length; i++)
			{
				if (childReactions[i].gameObject != gameObject && !streamReactions.Contains(childReactions[i]))
				{
					streamReactions.Add(childReactions[i]);
				}
			}
		}
	}

	public override void React(float actionRate)
	{
		// Provoke all listed reactions.
		float minReaction = 1;
		for (int i = 0; i < streamReactions.Count; i++)
		{
			if (streamReactions[i] == null || streamReactions[i] == this)
			{
				streamReactions.RemoveAt(i);
				i--;
			}
			else
			{
				streamReactions[i].React(actionRate);
				if (streamReactions[i].reactionProgress < minReaction)
				{
					minReaction = streamReactions[i].reactionProgress;
				}
			}
		}
	}
}
