using UnityEngine;
using System.Collections;

public class RotateDoor : MonoBehaviour {

	public float stopAngle;
	public float speed = 8;
	private bool rotating;
	public float rotateDireciton = 1;

	void Start()
	{
		if (transform.localRotation.eulerAngles.z > stopAngle % 360)
		{
			rotateDireciton = -1;
		}
	}

	// Update is called once per frame
	void Update () {
		if(rotating == true)
		{
			if ((rotateDireciton > 0 && transform.localRotation.eulerAngles.z < stopAngle) || (rotateDireciton < 0 && transform.localRotation.eulerAngles.z > stopAngle))
			{
				transform.Rotate(Vector3.forward * Time.deltaTime * speed *rotateDireciton);
			}
			/*if(name == "Gate Hinge" && transform.localRotation.eulerAngles.z < 237.0f)
				transform.Rotate(Vector3.forward * Time.deltaTime * speed);
			if(name == "Gate Hinge 2" && transform.localRotation.eulerAngles.z > 56.0f)
				transform.Rotate(-Vector3.forward * Time.deltaTime * speed);*/
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
