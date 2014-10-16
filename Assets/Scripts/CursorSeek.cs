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
			if (tail != null)
			{
				tracer.AddVertex(tail.transform.position);
			}
			else
			{
				tracer.AddVertex(transform.position);
			}
		}
	}

	private void HandleTouches()
	{
		if (Input.GetMouseButtonDown(0))
		{
			seeking = !(toggleSeek && seeking);
			if (tail == null)
			{
				tracer.StartLine();
			}
		}
		else if ((!toggleSeek && Input.GetMouseButton(0)) || (toggleSeek && seeking))
		{
			Drag();
		}
		else
		{
			seeking = false;
			mover.SlowDown();
			if (tail == null)
			{
				tracer.DestroyLine();
			}
			if (tailTrigger != null)
			{
				tail.trigger.enabled = true;
			}
		}
	}


	private void FollowCursor()
	{
		if (cursor.GetComponent<ControllerSeek>().active)
		{
			if (!seeking)
			{
				seeking = true;
				if (tail == null)
				{
					tracer.StartLine();
				}
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
			if (tail == null)
			{
				tracer.DestroyLine();
			}
			if (tailTrigger != null)
			{
				tail.trigger.enabled = true;
			}
		}
	}

	private void Drag(bool criticalLine = true)
	{
		Vector3 dragForward = MousePointInWorld() - transform.position;
		if (useController)
		{
			dragForward = cursor.GetComponent<ControllerSeek>().forward;
		}

		/*float tempZ = dragForward.z;
		dragForward.z = dragForward.y;
		dragForward.y = tempZ;*/
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
		if (tracer != null)
		{
			tracer.StartLine();
		}
		if (tailTrigger != null)
		{
			tail.trigger.enabled = false;
		}
	}

	private void TailEndFollow()
	{
		if (tracer)
		{
			tracer.DestroyLine();
		}
	}
}
