using UnityEngine;
using System.Collections;

public class ControllerSeek : MonoBehaviour {

	//parent
	public GameObject cursor;
	public GameObject parent;

	//current direction
	public Vector3 forward;
	private Vector3 cursorPosition;
	private float radius;

	//controller input
	private float controllerX;
	private float controllerY;

	// Use this for initialization
	void Start () {

		active = false;
		cursorPosition = cursor.transform.position;
		forward = cursor.transform.position - parent.transform.position;
		radius = forward.magnitude;
	}
	
	// Update is called once per frame
	void Update () {

		HandleControllerInput ();
		ResetCursor ();
	}

	void HandleControllerInput()
	{
		controllerX = Input.GetAxis ("Horizontal");
		controllerY = Input.GetAxis ("Vertical");
		
		if ( controllerX != 0 || controllerY != 0)
		{
			active = true;
			forward += new Vector3 (controllerX, controllerY, 0);
		}

		else 
			active = false;

	}

	void ResetCursor()
	{
		//position
		forward.Normalize ();
		forward = forward * radius;
		cursorPosition = parent.transform.position + forward;
		cursor.transform.localPosition = forward;

		//rotation
		cursor.transform.rotation = Quaternion.LookRotation (forward, new Vector3 (0, 0, 1));
	}

}