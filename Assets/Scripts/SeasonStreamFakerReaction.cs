using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SeasonStreamFakerReaction : SeasonReaction
{

	public bool fakingActive = false;
	private bool wasActive = false;
	public StreamBody fakeStream;
	public List<StreamReaction> reactions;

	protected override void Start()
	{
		base.Start();
		if(fakeStream == null)
		{
			fakeStream = GetComponent<StreamBody>();
		}
		UpdateTouchedStreams();
		wasActive = fakingActive;
	}

	protected void FixedUpdate()
	{
		fakingActive = (season == SeasonManager.ActiveSeason.WET);

		if (fakingActive && fakeStream != null)
		{
			for (int i = 0; i < reactions.Count; i++)
			{
				if (reactions[i] != null)
				{
					fakeStream.ProvokeReaction(reactions[i]);
				}
			}
		}

		if (wasActive != fakingActive)
		{
			UpdateTouchedStreams();
		}

		wasActive = fakingActive;
	}

	void UpdateTouchedStreams()
	{
		for (int i = 0; i < reactions.Count; i++)
		{
			if (reactions[i] != null)
			{
				if (fakingActive)
				{
					reactions[i].SetTouchedStreams(reactions[i].streamsTouched + 1);
				}
				else
				{
					reactions[i].SetTouchedStreams(reactions[i].streamsTouched - 1);
				}
			}
		}
	}
}
