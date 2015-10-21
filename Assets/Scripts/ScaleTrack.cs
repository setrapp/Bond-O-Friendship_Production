using UnityEngine;
using System.Collections;

public class ScaleTrack : MonoBehaviour {
	public Transform targetTransform;
	public Vector3 maxScale;
	public Vector3 minScale;
	public SpinPad targetPad;

	void Start()
	{
		if (targetTransform == null)
		{
			targetTransform = transform;
		}
	}

	void Update()
	{
		float portionComplete = (targetPad.rotationProgress / 2) + 0.5f;
		targetTransform.localScale = (maxScale * (1 - portionComplete)) + (minScale * portionComplete);
	}

}
