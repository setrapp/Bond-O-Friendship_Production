using UnityEngine;
using System.Collections;

public class ScaleTrack : MonoBehaviour {
	public Vector3 maxScale;
	public Vector3 minScale;
	public SpinPad targetPad;

	void Update()
	{
		float portionComplete = targetPad.currentRotation / targetPad.goalRotation;
		if (targetPad.flipDirection)
		{
			transform.localScale = (maxScale * (1 - portionComplete)) + (minScale * portionComplete);
		}
		else
		{
			transform.localScale = (minScale * (1 - portionComplete)) + (maxScale * portionComplete);
		}
	}

}
