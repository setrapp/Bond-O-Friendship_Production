using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StreamMusicReaction : StreamReaction {

	public AudioSource audioSource;

	public override bool React(float actionRate)
	{
		bool reacted = base.React(actionRate);
		if (reacted)
		{
			if (actionRate >= 0 && reactionProgress >= 1)
			{
				if (audioSource != null && audioSource.clip != null)
				{
					audioSource.Play();
				}
			}
		}
		return reacted;
	}
}
