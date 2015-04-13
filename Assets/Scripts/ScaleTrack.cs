using UnityEngine;
using System.Collections;

public class ScaleTrack : MonoBehaviour {
	public Vector3 maxScale;
	public Vector3 minScale;
	public SpinPad targetPad;

	void Update()
	{
		float portionComplete = (targetPad.rotationProgress / 2) + 0.5f;
		transform.localScale = (maxScale * (1 - portionComplete)) + (minScale * portionComplete);
	}

}
