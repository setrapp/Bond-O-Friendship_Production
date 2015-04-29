using UnityEngine;
using System.Collections;

public class RotateDoor : MonoBehaviour {

	public float stopAngle;
	public float speed = 8;
	private bool rotating;
	public float rotateDireciton = 1;
	public Collider colliderToDisable = null;
	public SpinOpenTrack.Direction checkRotationAxis = SpinOpenTrack.Direction.Z_AXIS;
	public SpinOpenTrack.Direction rotationAxis = SpinOpenTrack.Direction.Z_AXIS; 

	void Start()
	{
		stopAngle = stopAngle % 360;
	}

	// Update is called once per frame
	void Update () {
		if(rotating == true)
		{
			if (colliderToDisable != null && colliderToDisable.enabled)
			{
				colliderToDisable.enabled = false;
			}

			float currentRotation = transform.localRotation.eulerAngles.z;
			if (checkRotationAxis == SpinOpenTrack.Direction.X_AXIS)
			{
				currentRotation = transform.localRotation.eulerAngles.x;
			}
			else if (checkRotationAxis == SpinOpenTrack.Direction.Y_AXIS)
			{
				currentRotation = transform.localRotation.eulerAngles.y;
			}


			if (stopAngle < 0)
			{
				currentRotation -= 360;
			}

			if (Mathf.Abs(currentRotation) > Mathf.Abs(stopAngle))
			{
				rotateDireciton = -1;
			}

			if (currentRotation != stopAngle)
			{
				float rotationAmount = Time.deltaTime * speed ;
				if (rotationAmount > Mathf.Abs(stopAngle - currentRotation))
				{
					rotationAmount = Mathf.Abs(stopAngle - currentRotation);
					rotating = false;
				}

				Vector3 axis = Vector3.forward;
				if (rotationAxis == SpinOpenTrack.Direction.X_AXIS)
				{
					axis = Vector3.right;
				}
				else if (rotationAxis == SpinOpenTrack.Direction.Y_AXIS)
				{
					axis = Vector3.up;
				}
				transform.Rotate(axis * rotationAmount * rotateDireciton);
			}
		}
	}

	public void ClusterNodesSolved(ClusterNodePuzzle puzzle)
	{
		if (puzzle != null && puzzle.solved)
		{
			rotating = true;
		}
	}
}
