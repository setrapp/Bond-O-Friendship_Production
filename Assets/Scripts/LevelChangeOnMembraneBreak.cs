using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelChangeOnMembraneBreak : MonoBehaviour {

	public MembraneWall membraneWall;
	public Island levelToUse;
	public float newAudioVolume = 1;

	private void MembraneWallBroken(MembraneWall brokenMembrane)
	{
		if (brokenMembrane != null && brokenMembrane == membraneWall)
		{
			if (levelToUse != null)
			{
				CameraColorFade.Instance.FadeToColor(levelToUse.backgroundColor);
				AudioSource backgroundAudio = Globals.Instance.levelsBackgroundAudio[(int)levelToUse.backgroundAudioId];
				if (backgroundAudio != Globals.Instance.bgm)
				{
					StartCoroutine(BackgroundAudioCrossFade.Instance.CrossFade(backgroundAudio, newAudioVolume));
				}

				Globals.Instance.Player1.SendMessage("ChangeActiveLevel", levelToUse, SendMessageOptions.DontRequireReceiver);
				Globals.Instance.Player2.SendMessage("ChangeActiveLevel", levelToUse, SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}
