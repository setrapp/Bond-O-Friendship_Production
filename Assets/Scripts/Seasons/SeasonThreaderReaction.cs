using UnityEngine;
using System.Collections;

public class SeasonThreaderReaction : SeasonReaction {

	public Threader targetThreader;

	protected virtual void Start()
	{
		base.Start();
	}

	protected virtual void ApplySeasonChanges()
	{
		base.ApplySeasonChanges();
		if (season == SeasonManager.ActiveSeason.WET)
		{
			//todo set min fluffs
		}
	}
}
