using UnityEngine;
using System.Collections;

public class CursorSeek : MonoBehaviour {
	public SimpleMover mover;
	public Tracer tracer;
	public Camera gameCamera = null;
	public GameObject geometry;
	public bool directVelocity;
	private bool seeking;
	public GameObject cursor;
	public bool toggleSeek;
	public bool destroyLineOnUp = true;

	protected void Start () 
	{
		if(gameCamera == null)
		{
			gameCamera = Camera.main;
		}
		if (mover == null)
		{
			mover = GetComponent<SimpleMover>();
		}
		if (tracer == null)
		{
			tracer = GetComponent<Tracer>();
		}
	}
	
	void Update () {
		HandleTouches();

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
			mover.slowDown = false;
			tracer.StartLine();
		}
		else if ((!toggleSeek && Input.GetMouseButton(0)) || (toggleSeek && seeking))
		{
			Drag();
		}
		else
		{
			seeking = false;
			mover.slowDown = true;
			if (destroyLineOnUp)
			{
				tracer.DestroyLine();
			}
		}
	}

	private void Drag(bool criticalLine = true)
	{
		Vector3 dragForward = MousePointInWorld() - transform.position;

		/*float tempZ = dragForward.z;
		dragForward.z = dragForward.y;
		dragForward.y = tempZ;*/
		if (directVelocity)
		{
			mover.Move(dragForward, mover.maxSpeed, true);
		}
		else
		{
			mover.Accelerate(dragForward, true, true);
		}
		//geometry.transform.LookAt(transform.position + mover.velocity, geometry.transform.up);
	}

	private Vector3 MousePointInWorld()
	{
		Vector3 touchPosition = gameCamera.ScreenToWorldPoint(Input.mousePosition);
		touchPosition.z = transform.position.z;
		return touchPosition;
	}
}
