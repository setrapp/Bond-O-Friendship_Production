using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Island : MonoBehaviour {
	public IslandID islandId;
	public IslandContainer container;
	public LevelHelper levelHelper;
	//public bool forcePlayersEstablish = true;
	public bool visibilityDepthMaskNeeded = false;
	public bool fadeBackgroundColor = true;
	public Color backgroundColor;

	void Start()
	{
		if (levelHelper == null)
		{
			levelHelper = GetComponentInChildren<LevelHelper>();
			if (levelHelper == null)
			{
				Debug.LogError("Level Helper not attached to " + ((container != null) ? container.name + "'s island" : "an island. Please add one as child of the island level."));
			}
		}

		if (levelHelper != null && levelHelper.playersEstablish != null)
		{
			if (levelHelper.playersEstablish.defaultPlayerParent == null)
			{
				levelHelper.playersEstablish.defaultPlayerParent = transform;
			}
			if (Globals.Instance != null && Globals.Instance.existingEther == null)
			{
				levelHelper.playersEstablish.PlacePlayers();
			}
		}

		if (Globals.Instance != null)
		{
			Globals.Instance.visibilityDepthMaskNeeded = visibilityDepthMaskNeeded;
		}

		if (fadeBackgroundColor && CameraColorFade.Instance != null)
		{
			CameraColorFade.Instance.FadeToColor(backgroundColor);
		}
	}
}

public enum IslandID
{
	NONE = 0,
	TUTORIAL,
	HARMONY_A,
	INTIMACY_A,
	ASYMMETRY_A
};