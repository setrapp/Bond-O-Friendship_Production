using UnityEngine;
using System.Collections;

public class SpinPadForceComplete : MonoBehaviour {

	public SpinPad targetPad;

	void Start()
	{
		if (targetPad == null)
		{
			targetPad = GetComponent<SpinPad>();
		}

		if (targetPad != null)
		{
			targetPad.wasCompleted = false;
			targetPad.neverActivate = false;
			targetPad.forceComplete = true;
		}
	}
}
