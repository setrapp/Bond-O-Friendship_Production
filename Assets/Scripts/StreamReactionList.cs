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

	public override bool React(float actionRate)
	{
		bool reacted = base.React(0);
		if (reacted)
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
				else if (streamReactions[i].enabled && streamReactions[i].gameObject.activeInHierarchy)
				{
					streamReactions[i].React(actionRate * reactionRate);

					if (streamReactions[i].reactionProgress < minReaction)
					{
						minReaction = streamReactions[i].reactionProgress;
					}
				}
			}

			if (actionRate > 0)
			{
				lastReaction = Time.time;
			}

			reactionProgress = minReaction;
		}
		return reacted;

	}

	public override void SetTouchedStreams(int streamsTouched)
	{
		base.SetTouchedStreams(streamsTouched);
		for (int i = 0; i < streamReactions.Count; i++)
		{
			if (streamReactions[i] != null && streamReactions[i] != null)
			{
				streamReactions[i].SetTouchedStreams(streamsTouched);
			}
		}
	}

	public override void ReactToEmptyFluffStick(FluffStick fluffStick)
	{
		for (int i = 0; i < streamReactions.Count; i++)
		{
			if (streamReactions[i] != null && streamReactions[i] != null)
			{
				streamReactions[i].ReactToEmptyFluffStick(fluffStick);
			}
		}
	}
}
