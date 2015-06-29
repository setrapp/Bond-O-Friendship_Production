using UnityEngine;
using System.Collections;

public class SeasonManager : MonoBehaviour {
	// NOTE: This script should appear on the level object with a tag set to Island.


	public ActiveSeason activeSeason = ActiveSeason.DRY;

	public enum ActiveSeason
	{
		DRY = 1,
		WET = 2,
		COLD = 4
	}
}
