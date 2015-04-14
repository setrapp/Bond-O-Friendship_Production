using UnityEngine;
using System.Collections;

public class PushPullTrack : MonoBehaviour {
	public GameObject pullTarget;
	public GameObject pushTarget;
	public SpinPad targetPad;

	void Update()
	{
		float portionComplete = (targetPad.rotationProgress / 2) + 0.5f;
		transform.position = (pushTarget.transform.position * (1 - portionComplete)) + (pullTarget.transform.position * portionComplete);
	}

}
