using UnityEngine;
using System.Collections;

public class MembraneBreakToFade : MonoBehaviour {
	public FadeToBeContinued fadeTarget = null;

	private void SendFade()
	{
		fadeTarget.gameObject.SetActive(true);
		fadeTarget.StartFade();
	}

	private void MembraneBroken(Membrane membrane)
	{
		SendFade();
	}

	private void MembraneBroken(MembraneWall membrane)
	{
		SendFade();
	}

	private void MembraneBroken(MembraneShell membrane)
	{
		SendFade();
	}
}
