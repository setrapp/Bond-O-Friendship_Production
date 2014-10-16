using UnityEngine;
using System.Collections;

public class OffScreenTrackingOld : MonoBehaviour {
		
	private GameObject objectToTrack = null;

	public GameObject horizontalTracker;
	public GameObject verticalTracker;

	void Update () 
	{

		if(objectToTrack != null)
		{
			//Convert the object we are trackign to viewport coordinates
			Vector3 v3Screen = Camera.main.WorldToViewportPoint(objectToTrack.transform.position);

			//Check if the object is on screen
			if (v3Screen.x > -0.01f && v3Screen.x < 1.01f && v3Screen.y > -0.01f && v3Screen.y < 1.01f)
			{
				verticalTracker.renderer.enabled = false;
				horizontalTracker.renderer.enabled = false;
			}
			//It's offscreen so we need to track it
			else
			{
				//Clamp the x and y coordinates between 0 and 1 to keep the trackers onscreen
				v3Screen.x = Mathf.Clamp (v3Screen.x, 0.01f, 0.99f);
				v3Screen.y = Mathf.Clamp (v3Screen.y, 0.01f, 0.99f);

				//If the object is left or right of us, show the tracker on the left/right sides of the screen
				if((v3Screen.x == 0.01f || v3Screen.x == 0.99f))
					TrackVertical(v3Screen);
				else
					verticalTracker.renderer.enabled = false;
			
				//If the object is above or below us, show the tracker on the top/bottom of the screen
				if((v3Screen.y == 0.01f || v3Screen.y == 0.99f))
					TrackHorizontal(v3Screen);
				else
					horizontalTracker.renderer.enabled = false;

			}


		}		
	}


	//Track Right to Left on the Top and Bottom of the Screen
	private void TrackHorizontal(Vector3 v3Screen)
	{
		//Make the tracker visible
		horizontalTracker.renderer.enabled = true;

		//Round the y to either 0 or 1 (Top or Bottom of the screen)
		v3Screen.y = Mathf.Round(v3Screen.y);

		//Set the tracker's position
		horizontalTracker.transform.position = Camera.main.ViewportToWorldPoint (v3Screen);

		//Use the distance to scale the tracker (Wider == further away)
		var distanceScale = Vector3.Distance(horizontalTracker.transform.position, objectToTrack.transform.position)/2f;
		horizontalTracker.transform.localScale = new Vector3(distanceScale, horizontalTracker.transform.localScale.y,horizontalTracker.transform.localScale.z);


	}


	//Track Up and Down on the Left and Right of the Screen
	private void TrackVertical(Vector3 v3Screen)
	{
		//Make the tracker visible
		verticalTracker.renderer.enabled = true;

		//Round the x to either 0 or 1 (Right or Left of the screen)
		v3Screen.x = Mathf.Round(v3Screen.x);

		//Set the tracker's position
		verticalTracker.transform.position = Camera.main.ViewportToWorldPoint (v3Screen);

		//Use the distance to scale the tracker (Wider == further away)
		var distanceScale = Vector3.Distance(verticalTracker.transform.position, objectToTrack.transform.position)/2f;
		verticalTracker.transform.localScale = new Vector3(verticalTracker.transform.localScale.x,distanceScale,verticalTracker.transform.localScale.z);

	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Converser")
		objectToTrack = gameObject;
	}

	void OnTriggerExit(Collider other)
	{
		if(other.tag == "Converser")
			objectToTrack = null;

		verticalTracker.renderer.enabled = false;
		horizontalTracker.renderer.enabled = false;
	}
}
