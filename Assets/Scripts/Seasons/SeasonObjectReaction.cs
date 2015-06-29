using UnityEngine;
using System.Collections;

public class SeasonObjectReaction : SeasonReaction {

	protected override void Start()
	{
		base.Start();
	}

	void Update()
	{
		if (manager == null)
		{
			return;
		}

		if (season != manager.activeSeason)
		{
			season = manager.activeSeason;
		}
	}
}
