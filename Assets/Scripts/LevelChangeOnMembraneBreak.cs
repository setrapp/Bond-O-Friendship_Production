using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelChangeOnMembraneBreak : MonoBehaviour {

	public MembraneWall membraneWall;
	public Island levelToUse;

	private void MembraneBroken(MembraneWall brokenMembrane)
	{
		if (brokenMembrane != null && brokenMembrane == membraneWall)
		{
			if (levelToUse != null)
			{
				CameraColorFade.Instance.FadeToColor(levelToUse.backgroundColor);
				// fade background audio.
			}
		}
	}
}
