using UnityEngine;
using System.Collections;

public class KeyboardSeek : SimpleSeek {
	public bool useWASD = false;
	public GameObject geometry;

	void Update () {
		Vector3 acceleration = Vector3.zero;
		if ((useWASD && Input.GetKey("w")) || (!useWASD && Input.GetKey(KeyCode.UpArrow)))
		{
			acceleration += Vector3.up;
		}
		if ((useWASD && Input.GetKey("a")) || (!useWASD && Input.GetKey(KeyCode.LeftArrow)))
		{
			acceleration -= Vector3.right;
		}
		if ((useWASD && Input.GetKey("s")) || (!useWASD && Input.GetKey(KeyCode.DownArrow)))
		{
			acceleration -= Vector3.up;
		}
		if ((useWASD && Input.GetKey("d")) || (!useWASD && Input.GetKey(KeyCode.RightArrow)))
		{
			acceleration += Vector3.right;
		}

		if (acceleration.sqrMagnitude > 0)
		{
			mover.Accelerate(acceleration);
			if (tracer.lineRenderer == null)
			{
				tracer.StartLine();
			}
			else
			{
				tracer.AddVertex(transform.position);
			}
		}
		else
		{
			mover.SlowDown();
			tracer.DestroyLine();
		}


		geometry.transform.LookAt(transform.position + mover.velocity, geometry.transform.up);
	}
}
