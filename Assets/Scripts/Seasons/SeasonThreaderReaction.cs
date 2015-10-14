using UnityEngine;
using System.Collections;

public class SeasonThreaderReaction : SeasonReaction {

	public ThreadParent targetThreadParent;
	//public float wetMinFluffCount = 0;

	protected override void Start()
	{
		base.Start();
	}

	protected override void ApplySeasonChanges()
	{
		base.ApplySeasonChanges();

		if (targetThreadParent == null)
		{
			return;
		}

		if (season == SeasonManager.ActiveSeason.WET)
		{
			targetThreadParent.minBondFluffCount = Globals.Instance.Player1.character.bondAttachable.bondOverrideStats.stats.maxFluffCapacity;
		}
		else
		{
			targetThreadParent.minBondFluffCount = -1;
		}
	}
}
