using UnityEngine;
using System.Collections;

public class PushPullTrack : MonoBehaviour {
	public GameObject pullTarget;
	public GameObject pushTarget;
	public SpinPad targetPad;

	void Update()
	{
		float portionComplete = targetPad.currentRotation / targetPad.goalRotation;
		if (targetPad.flipDirection)
		{
			transform.position = (pushTarget.transform.position * (1 - portionComplete)) + (pullTarget.transform.position * portionComplete);
		}
		else
		{
			transform.position = (pullTarget.transform.position * (1 - portionComplete)) + (pushTarget.transform.position * portionComplete);
		}
	}

}
