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

	void Start()
	{
		SeasonPlayerReaction player1Reaction = Globals.Instance.Player1.GetComponent<SeasonPlayerReaction>();
		SeasonPlayerReaction player2Reaction = Globals.Instance.Player2.GetComponent<SeasonPlayerReaction>();

		if (player1Reaction != null)
		{
			player1Reaction.manager = this;
			player1Reaction.enabled = true;
		}

		if (player2Reaction != null)
		{
			player2Reaction.manager = this;
			player2Reaction.enabled = true;
		}
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
