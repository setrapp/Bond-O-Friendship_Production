using UnityEngine;
using System.Collections;

public class SpinOpenTrack : MonoBehaviour {
	public float inRotation;
	public float outRotation;
	public SpinPad targetPad;
	public Direction direction = Direction.Z_AXIS;

	void Update()
	{
		float portionComplete = (targetPad.rotationProgress / 2) + 0.5f;
		Vector3 localRot = transform.localRotation.eulerAngles;
		switch(direction)
		{
			case Direction.X_AXIS:
				localRot.x = (outRotation * (1 - portionComplete)) + (inRotation * portionComplete);
				break;
			case Direction.Y_AXIS:
				localRot.y = (outRotation * (1 - portionComplete)) + (inRotation * portionComplete);
				break;
			case Direction.Z_AXIS:
				localRot.z = (outRotation * (1 - portionComplete)) + (inRotation * portionComplete);
				break;
		}
		
		transform.localRotation = Quaternion.Euler(localRot);
	}

	public enum Direction
	{
		X_AXIS = 0,
		Y_AXIS = 1,
		Z_AXIS = 2,
	}
}
