using UnityEngine;
using System.Collections;

public class SpinOpenTrack : MonoBehaviour {
	public float inRotation;
	public float outRotation;
	public SpinPad targetPad;

	void Update()
	{
		float portionComplete = (targetPad.rotationProgress / 2) + 0.5f;
		Vector3 localRot = transform.localRotation.eulerAngles;
		localRot.z = (outRotation * (1 - portionComplete)) + (inRotation * portionComplete);
		transform.localRotation = Quaternion.Euler(localRot);
	}
}
