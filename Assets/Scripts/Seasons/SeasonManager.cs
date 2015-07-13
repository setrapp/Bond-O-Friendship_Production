using UnityEngine;
using System.Collections;

public class SeasonManager : MonoBehaviour {
	// NOTE: This script should appear on the level object with a tag set to Island.


	public ActiveSeason activeSeason = ActiveSeason.DRY;
	public float transitionTime = 1;
	private float startTransition = -1;

	public enum ActiveSeason
	{
		DRY = 0,
		WET = 1,
		COLD = 2
	}

	public bool AttemptSeasonChange(ActiveSeason newSeason)
	{
		if (Time.time - startTransition >= transitionTime || startTransition < 0)
		{
			startTransition = Time.time;
			activeSeason = newSeason;
			return true;
		}
		return false;
	}
}
