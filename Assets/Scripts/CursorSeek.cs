using UnityEngine;
using System.Collections;

public class CursorSeek : SimpleSeek {
	public bool useController = false;
	public Camera gameCamera = null;
	public GameObject geometry;
	public bool directVelocity;
	private bool seeking;
	public GameObject cursor;
	public bool toggleSeek;

	protected override void Start () 
	{
		base.Start();
		if(gameCamera == null)
		{
			gameCamera = Camera.main;
		}
	}
	
	void Update () {
		if (useController)
		{ 
			FollowCursor(); 
		}
		else
		{
			HandleTouches();
		}

		if (tracer != null)
		{
			
			tracer.AddVertex(transform.position);
		}
	}

	private void HandleTouches()
	{
		if (Input.GetMouseButtonDown(0))
		{
			seeking = !(toggleSeek && seeking);
				tracer.StartLine();
		}
		else if ((!toggleSeek && Input.GetMouseButton(0)) || (toggleSeek && seeking))
		{
			Drag();
		}
		else
		{
			seeking = false;
			mover.SlowDown();
			tracer.DestroyLine();
		}
	}


	private void FollowCursor()
	{
		if (cursor.GetComponent<ControllerSeek>().enabled)
		{
			if (!seeking)
			{
				seeking = true;
				tracer.StartLine();
			}
			else
			{
				Drag();
			}
		}
		else
		{
			mover.SlowDown();
			seeking = false;
			tracer.DestroyLine();
		}
	}

	private void Drag(bool criticalLine = true)
	{
		Vector3 dragForward = MousePointInWorld() - transform.position;
		if (useController)
		{
			dragForward = cursor.GetComponent<ControllerSeek>().forward;
		}

		if (directVelocity)
		{
			mover.Move(dragForward, mover.maxSpeed, true);
		}
		else
		{
			mover.Accelerate(dragForward);
		}
		geometry.transform.LookAt(transform.position + mover.velocity, geometry.transform.up);
	}

	private Vector3 MousePointInWorld()
	{
		Vector3 touchPosition = gameCamera.ScreenToWorldPoint(Input.mousePosition);
		touchPosition.z = transform.position.z;
		return touchPosition;
	}

	private void TailStartFollow()
	{
		tracer.StartLine();
	}

	private void TailEndFollow()
	{
		if (tracer)
		{
			tracer.DestroyLine();
		}
	}
}
