using UnityEngine;
using System.Collections;

public class SeasonManager : MonoBehaviour {
	// NOTE: This script should appear on the level object with a tag set to Island.


	public ActiveSeason activeSeason = ActiveSeason.DRY;

	public enum ActiveSeason
	{
		DRY = 0,
		WET = 1,
		COLD = 2
	}
}
