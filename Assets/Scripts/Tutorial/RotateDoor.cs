using UnityEngine;
using System.Collections;

public class RotateDoor : MonoBehaviour {

	public float stopAngle;
	public float speed = 8;
	private bool rotating;
	public float rotateDireciton = 1;

	void Start()
	{
		stopAngle = stopAngle % 360;
	}

	// Update is called once per frame
	void Update () {
		if(rotating == true)
		{
			float currentRotation = transform.localRotation.eulerAngles.z;
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
				transform.Rotate(Vector3.forward * rotationAmount * rotateDireciton);
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
