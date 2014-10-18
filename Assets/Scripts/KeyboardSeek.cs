using UnityEngine;
using System.Collections;

public class KeyboardSeek : SimpleSeek {
	public bool useWASD = false;
	public GameObject geometry;

	void Update () {
		
		// Movement.
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

		// Sharing.
		if((useWASD && Input.GetKeyDown(KeyCode.LeftControl)) || (!useWASD && Input.GetKeyDown(KeyCode.RightControl)))
		{
			partnerLink.connection.SendPulse(partnerLink, partnerLink.partner);
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
