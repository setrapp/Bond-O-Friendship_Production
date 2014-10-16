using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/////////////////////////////////////////////////////////
/// Attach to the Main Player                         ///
/// Gets a list of all other conversers               ///
/// Get all of the boundaries                         ///
/// Calculate player distance to boundaries           ///
/// Calculate player distance to each converser       /// 
/// Calculate each converser's distance to boundaries ///
/// Add in tracking                                   ///
/////////////////////////////////////////////////////////

public class OffScreenTracking : MonoBehaviour {

	private List<GameObject> conversersList = new List<GameObject>();

	public float drawDistance = 50f;
	private float drawDistanceY = 0f;
	private float drawDistanceX = 0f;

	public GameObject topBoundary;
	public GameObject bottomBoundary;
	public GameObject leftBoundary;
	public GameObject rightBoundary;

	private float playerDistanceToTop = 0;
	private float playerDistanceToBottom = 0;
	private float playerDistanceToLeft = 0;
	private float playerDistanceToRight = 0;

	private float converserDistanceToTop = 0;
	private float converserDistanceToBottom = 0;
	private float converserDistanceToLeft = 0;
	private float converserDistanceToRight = 0;

	private float playerConverserDistanceHeight = 0;
	private float playerConverserDistanceWidth = 0;

	private float shortestY = 0f;
	private float shortestX = 0f;

	// Use this for initialization
	void Start () {

		GameObject[] conversers;
		conversers = GameObject.FindGameObjectsWithTag("Converser");

		foreach(GameObject go in conversers)
		{
			if(go.transform != transform)
				conversersList.Add(go);
		}

		GameObject[] boundaries = GameObject.FindGameObjectsWithTag("World Boundary");
		for (int i = 0; i < boundaries.Length; i++)
		{
			Boundary boundary = boundaries[i].GetComponent<Boundary>();
			if (boundary != null)
			{
				switch(boundary.colliderLocation)
				{
					case TriggerLooping.ColliderLocation.Top:
						topBoundary = boundary.gameObject;
						break;
					case TriggerLooping.ColliderLocation.Bottom:
						bottomBoundary = boundary.gameObject;
						break;
					case TriggerLooping.ColliderLocation.Left:
						leftBoundary = boundary.gameObject;
						break;
					case TriggerLooping.ColliderLocation.Right:
						rightBoundary = boundary.gameObject;
						break;
				}
			}
		}

		//var test = Camera.main.orthographicSize * Camera.main.aspect;
		//Debug.Log(test);
	
	}
	
	// Update is called once per frame
	void Update () {

		if(GetComponent<PartnerLink>().Partner != null)
		{
			foreach(GameObject go in conversersList)
			{
				go.transform.Find("HorizontalTracker").renderer.enabled = false;
				go.transform.Find("VerticalTracker").renderer.enabled = false;
			}
			return;
		}

		drawDistanceY = drawDistance + Camera.main.orthographicSize;
		drawDistanceX = drawDistance + (Camera.main.orthographicSize * Camera.main.aspect);

		PlayerDistanceToBoundaries();
		//Debug.Log("To Top: " + playerDistanceToTop);

		foreach(GameObject go in conversersList)
		{
			if(go.GetComponent<LoopTag>().stayOutsideBounds)
				continue;
			//Convert the object we are tracking to viewport coordinates
			Vector3 v3Screen = Camera.main.WorldToViewportPoint(go.transform.position);

			var horizontalTracker = go.transform.Find("HorizontalTracker");
			var verticalTracker = go.transform.Find("VerticalTracker");

			//Debug.Log(v3Screen);
			//Debug.Log(v3Screen.y);
			//Check if the object is on screen
			if (v3Screen.x > -0.01f && v3Screen.x < 1.01f && v3Screen.y > -0.01f && v3Screen.y < 1.01f)
			{
				horizontalTracker.renderer.enabled = false;
				verticalTracker.renderer.enabled = false;
			}
			else
			{
				ConverserDistanceToBoundaries(go);
				playerConverserDistanceHeight = Mathf.Abs(transform.position.y - go.transform.position.y);
				playerConverserDistanceWidth = Mathf.Abs(transform.position.x - go.transform.position.x);

				//Debug.Log(playerDistanceToLeft);
			   // Debug.Log(converserDistanceToRight);

				//Calculate the shortest x and y
				if(playerConverserDistanceHeight < (playerDistanceToTop + converserDistanceToBottom) && playerConverserDistanceHeight < (playerDistanceToBottom + converserDistanceToTop))
					shortestY = playerConverserDistanceHeight;
				else if((playerDistanceToTop + converserDistanceToBottom) < (playerDistanceToBottom + converserDistanceToTop))
					shortestY = playerDistanceToTop + converserDistanceToBottom;
				else
					shortestY = playerDistanceToBottom + converserDistanceToTop;

				if(playerConverserDistanceWidth < (playerDistanceToRight + converserDistanceToLeft) && playerConverserDistanceWidth < (playerDistanceToLeft + converserDistanceToRight))
					shortestX = playerConverserDistanceWidth;
				else if((playerDistanceToLeft + converserDistanceToRight) < (playerDistanceToRight + converserDistanceToLeft))
					shortestX = playerDistanceToLeft + converserDistanceToRight;
				else 
					shortestX = playerDistanceToRight + converserDistanceToLeft;

				//Debug.Log(playerDistanceToLeft);
				//var test = playerDistanceToRight + converserDistanceToLeft;
				//var testt = playerDistanceToLeft + converserDistanceToRight;
				//Debug.Log("P Right, C Left: " +test);
				//Debug.Log("P Left, C Right: " +testt);
				//Debug.Log("P to C: " + playerConverserDistanceWidth);
				//Debug.Log("Shortest: "+test);
				//Debug.Log("ShortestX: " +shortestX);
				//Debug.Log("ShortestY: " +shortestY);
				//var testt = (Mathf.Pow(drawDistance, 2f));
				//Debug.Log("Draw Distance: "+testt);


				Vector3 v3ScreenHorizontal = v3Screen;
				Vector3 v3ScreenVertical = v3Screen;
				if((Mathf.Pow(shortestX,2f) + Mathf.Pow(shortestY,2f)) < Mathf.Pow(drawDistance, 2f))
				{
					//Check tracking on top and bottom of screen
					if(shortestY < drawDistanceY && !(v3Screen.y > -0.01f && v3Screen.y < 1.01f))
					{
						horizontalTracker.renderer.enabled = true;
						var horizontalTrackerScale = Mathf.Abs(shortestY - Camera.main.orthographicSize);
						if(playerConverserDistanceHeight == shortestY)
						{
							v3ScreenHorizontal.x = Mathf.Clamp (v3ScreenHorizontal.x, 0.00f, 1f);
							v3ScreenHorizontal.y = Mathf.Clamp (v3ScreenHorizontal.y, 0f, 1f);
							if(playerConverserDistanceWidth != shortestX)
								v3ScreenHorizontal.x = 1f - v3ScreenHorizontal.x;
							
							v3ScreenHorizontal.y = Mathf.Round(v3ScreenHorizontal.y);

						}
						else
						{
							//Point to top of screen (use player distance to bottom to make sure the tracker goes past the top boundary
							v3ScreenHorizontal.y *= -1;
							//Round the y to either 0 or 1 (Top or Bottom of the screen)
							v3ScreenHorizontal.x = Mathf.Clamp (v3Screen.x, 0f, 1f);
							v3ScreenHorizontal.y = Mathf.Clamp (v3Screen.y, 0f, 1f);
							v3ScreenHorizontal.y = Mathf.Round(v3Screen.y);

						}					
						//Set the tracker's position
						horizontalTracker.transform.position = Camera.main.ViewportToWorldPoint (v3ScreenHorizontal);
						horizontalTracker.transform.localScale = new Vector3(horizontalTrackerScale, horizontalTracker.transform.localScale.y,horizontalTracker.transform.localScale.z);

					}
					else
						horizontalTracker.renderer.enabled = false;

					//Check tracking on right and left of screen
					if(shortestX < drawDistanceX && !(v3Screen.x > -0.01f && v3Screen.x < 1.01f))
					{
						verticalTracker.renderer.enabled = true;
						var verticalTrackerScale = Mathf.Abs(shortestX - (Camera.main.orthographicSize * Camera.main.aspect));
						//Debug.Log(verticalTrackerScale);
						//Debug.Log(playerConverserDistanceWidth);
						if(playerConverserDistanceWidth == shortestX)
						{	
							//Round the x to either 0 or 1 (Left or Right of the screen)
							v3ScreenVertical.x = Mathf.Clamp (v3ScreenVertical.x, 0f, 1f);
							v3ScreenVertical.y = Mathf.Clamp (v3ScreenVertical.y, 0f, 1f);
							if(playerConverserDistanceHeight != shortestY)
							{
									v3ScreenVertical.y = 1f - v3ScreenVertical.y;
							}
							v3ScreenVertical.x = Mathf.Round(v3ScreenVertical.x);
							//Debug.Log("Player To Converser: "+v3ScreenVertical);
							//Debug.Log ("Here");
						}
						else
						{


							//Point to boundary of world
							v3ScreenVertical.x *= -1;
							//Round the y to either 0 or 1 (Top or Bottom of the screen)
							v3ScreenVertical.x = Mathf.Clamp(v3ScreenVertical.x, 0f, 1f);
							v3ScreenVertical.y = Mathf.Clamp(v3ScreenVertical.y, 0f, 1f);
							v3ScreenVertical.x = Mathf.Round(v3ScreenVertical.x);
						}					
						//Set the tracker's position
						verticalTracker.transform.position = Camera.main.ViewportToWorldPoint (v3ScreenVertical);
						verticalTracker.transform.localScale = new Vector3(verticalTracker.transform.localScale.x, verticalTracker.transform.localScale.y, verticalTrackerScale);					
					}
					else
						verticalTracker.renderer.enabled = false;
				}
				else
				{
					verticalTracker.renderer.enabled = false;
					horizontalTracker.renderer.enabled = false;
				}
			}

		}

	
	}

	private void PlayerDistanceToBoundaries()
	{
		//Top Boundary
		playerDistanceToTop = Mathf.Abs(topBoundary.transform.position.y - transform.position.y);

		//Bottom Boundary
		playerDistanceToBottom = Mathf.Abs(bottomBoundary.transform.position.y - transform.position.y);

		//Left Boundary
		playerDistanceToLeft = Mathf.Abs(leftBoundary.transform.position.x - transform.position.x);

		//Right Boundary
		playerDistanceToRight = Mathf.Abs(rightBoundary.transform.position.x - transform.position.x);

	}

	private void ConverserDistanceToBoundaries(GameObject converser)
	{
		//Top Boundary
		converserDistanceToTop = Mathf.Abs(topBoundary.transform.position.y - converser.transform.position.y);// + Camera.main.orthographicSize;

		if(converserDistanceToTop < (Camera.main.orthographicSize))
			converserDistanceToTop += (Camera.main.orthographicSize);
		
		//Bottom Boundary
		converserDistanceToBottom = Mathf.Abs(bottomBoundary.transform.position.y - converser.transform.position.y);//+ Camera.main.orthographicSize;


		if(converserDistanceToBottom < (Camera.main.orthographicSize))
			converserDistanceToBottom += (Camera.main.orthographicSize);
		
		//Left Boundary
		converserDistanceToLeft = Mathf.Abs(leftBoundary.transform.position.x - converser.transform.position.x);// + (Camera.main.orthographicSize * Camera.main.aspect);

		if(converserDistanceToLeft < (Camera.main.orthographicSize * Camera.main.aspect))
			converserDistanceToLeft += (Camera.main.orthographicSize * Camera.main.aspect);
		
		//Right Boundary
		converserDistanceToRight = Mathf.Abs(rightBoundary.transform.position.x - converser.transform.position.x);// + (Camera.main.orthographicSize * Camera.main.aspect);

		if(converserDistanceToRight < (Camera.main.orthographicSize * Camera.main.aspect))
			converserDistanceToRight += (Camera.main.orthographicSize * Camera.main.aspect);

	}

	
}
