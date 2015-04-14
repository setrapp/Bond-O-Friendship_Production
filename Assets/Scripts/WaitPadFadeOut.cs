using UnityEngine;
using System.Collections;

public class WaitPadFadeOut : MonoBehaviour {

	//public WaitPad triggerPad;
	public OrbWaitPad triggerPad;
	public FadeToBeContinued fadeOut;

	void Update()
	{
		if (triggerPad.fullyLit)
		{
			fadeOut.gameObject.SetActive(true);
			fadeOut.StartFade();
		}
	}
}
