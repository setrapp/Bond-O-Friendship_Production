using UnityEngine;
using System.Collections;

public class SplitMaskController : MonoBehaviour {

	public GameObject player1;
	public GameObject player2;

	public bool splitScreen = false;
	public GameObject camera1;
	public GameObject camera2;
	public GameObject splitScreenCam;

	public GameObject pivot;
	public GameObject mainPivot;

	private float currentCamHeight = 0f;

	//private bool 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		ResizeMasks();
		var testPlayerOne = Camera.main.WorldToViewportPoint(player1.transform.position);
		var testPlayerTwo = Camera.main.WorldToViewportPoint(player2.transform.position);

		//Debug.Log(testPlayerOne);
		//Debug.Log(testPlayerTwo);



		if(splitScreen == false)
		{
			mainPivot.SetActive(false);
			/*if(testPlayerOne.x < -0.1f || testPlayerOne.x > 1.1f || testPlayerOne.y < -0.1f || testPlayerOne.y > 1.1f || testPlayerTwo.x < -0.1f || testPlayerTwo.x > 1.1f || testPlayerTwo.y < -0.1f || testPlayerTwo.y > 1.1f)
			{
				splitScreen = true;
				if(!splitScreenCam.activeSelf)
				{
					splitScreenCam.SetActive(true);
				}
				Camera.main.GetComponent<CameraFollow>().splitScreen = true;

				//Debug.Log("Here");
			}*/
		}

		if(splitScreen)
		{
			var testPlayerOneSplit = splitScreenCam.camera.WorldToViewportPoint(player1.transform.position);
			//var testPlayerTwoSplit = Camera.main.WorldToViewportPoint(player2.transform.position);

			if(!mainPivot.activeSelf)
				mainPivot.SetActive(true);

			if((testPlayerTwo.x > -0.01f && testPlayerTwo.x < 1.01f && testPlayerTwo.y > -0.01f && testPlayerTwo.y < 1.01f))// || (testPlayerOneSplit.x > -0.01f && testPlayerOneSplit.x < 1.01f && testPlayerOneSplit.y > -0.01f && testPlayerOneSplit.y < 1.01f))
			{
				//Debug.Log(splitScreen);

				splitScreen = false;
				splitScreenCam.SetActive(false);
				//splitScreenCam.transform.position += new Vector3(100f, 100f, -10f);
				//Camera.main.GetComponent<CameraFollow>().splitScreen = false;
			}

			pivot.transform.rotation = Quaternion.FromToRotation(Vector3.up, Vector3.Cross(player1.transform.position - player2.transform.position, Vector3.forward));
			mainPivot.transform.rotation = Quaternion.FromToRotation(Vector3.up, Vector3.Cross(player1.transform.position - player2.transform.position, Vector3.forward));

		}
	}

	void ResizeMasks()
	{
		float camHeight = Camera.main.orthographicSize;

		if(camHeight != currentCamHeight)
		{
			currentCamHeight = camHeight;

			Vector3 viewPort= new Vector3(0,0,-10);
			Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(viewPort);
			viewPort = new Vector3(1,1,-10);
			Vector3 topRight = Camera.main.ViewportToWorldPoint(viewPort);

			float maskHeight = Mathf.Sqrt(1 + Mathf.Pow(Camera.main.aspect, 2));
			Debug.Log(maskHeight);


		

			Transform mainCamMask = mainPivot.transform.FindChild("Mask").transform;
			mainCamMask.localScale = new Vector3(maskHeight/2f, 1f, maskHeight);
			mainCamMask.localPosition = new Vector3(5f * (maskHeight/2f), 0f, 0f);

			Transform splitCamMask = pivot.transform.FindChild("Mask").transform;
			splitCamMask.localScale = new Vector3(maskHeight/2f, 1f, maskHeight);
			splitCamMask.localPosition = new Vector3(-5f *(maskHeight/2f), 0f, 0f);
			
			Transform dividerLine = pivot.transform.FindChild("Line").transform;
			dividerLine.localScale = new Vector3(.01f, 1f, maskHeight);
			
			
		}
	}



}
