using UnityEngine;
using System.Collections;

public class SpinOpenTrack : MonoBehaviour {
	public float startRotation;
	public float endRotation;
	public SpinPad targetPad;

	void Update()
	{
		float portionComplete = targetPad.currentRotation / targetPad.goalRotation;
		Vector3 localRot = transform.localRotation.eulerAngles;
		localRot.z = (startRotation * (1 - portionComplete)) + (endRotation * portionComplete);
		transform.localRotation = Quaternion.Euler(localRot);
	}
}
