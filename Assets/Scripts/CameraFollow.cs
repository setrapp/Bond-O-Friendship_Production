using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public Transform player1;
    public Transform player2;
    public float smoothness = 20;
	private Vector3 mainTargetPosition;
	private Vector3 splitTargetPosition;


	//public GameObject splitScreenCam;
	public bool splitScreen = false;
	public float testFloat = 5;
	private Vector3 cameraOffset = new Vector3(0, 0, -10);

	public GameObject pivot;
	private float currentCamHeight = 0f;
	public bool isCamera1;

	void Update()
	{
		//create the offset
		Vector3 betweenPlayers = (player2.position - player1.position);
		if (splitScreen)
		{
			Vector3 playerOneVC = Camera.main.WorldToViewportPoint(player1.transform.position);
			//Vector3 playerTwoVC = splitScreenCam.camera.WorldToViewportPoint(player2.transform.position);
			//Debug.Log(playerOneVC);

			//Vector3 playerOneSplitCam = splitScreenCam.camera.ViewportToWorldPoint(playerOneVC);
			//Vector3 playerTwoMainCam = Camera.main.ViewportToWorldPoint(playerTwoVC);
			//Debug.Log(playerOneSplitCam);


			mainTargetPosition = player1.position + (betweenPlayers.normalized * testFloat) + cameraOffset;
			transform.position = Vector3.Lerp(transform.position, mainTargetPosition, 1 / smoothness);

			ResizeMask();
			pivot.transform.rotation = Quaternion.FromToRotation(Vector3.up, Vector3.Cross(player1.transform.position - player2.transform.position, Vector3.forward));

			//splitTargetPosition = new Vector3(player2.position.x, player2.position.y, -10) + test * testFloat;
			//splitScreenCam.transform.position = Vector3.Lerp(splitScreenCam.transform.position, splitTargetPosition, 1 / smoothness);

		}
		else
		{

			//Vector3 test = splitScreen ? Vector3.Normalize(player1.position - player2.position) :Vector3.zero;

			mainTargetPosition =  player1.position + (betweenPlayers / 2) + cameraOffset;
			transform.position = Vector3.Lerp(transform.position, mainTargetPosition, 1 / smoothness);
		}

		
		//transform.position = Vector3.MoveTowards(transform.position, targetPosition, 1 / smoothness);
	}

	/*void Update2 () {
		//create the offset
		Vector3 test = splitScreen ? Vector3.Normalize(player1.position - player2.position) :Vector3.zero;
		if(splitScreen)
		{
			Vector3 playerOneVC = Camera.main.WorldToViewportPoint(player1.transform.position);
			Vector3 playerTwoVC = splitScreenCam.camera.WorldToViewportPoint(player2.transform.position);
			//Debug.Log(playerOneVC);

			Vector3 playerOneSplitCam = splitScreenCam.camera.ViewportToWorldPoint(playerOneVC);
			Vector3 playerTwoMainCam = Camera.main.ViewportToWorldPoint(playerTwoVC);
			//Debug.Log(playerOneSplitCam);


			mainTargetPosition = new Vector3(player1.position.x, player1.position.y, -10) - test * testFloat;
			transform.position = Vector3.Lerp(transform.position, mainTargetPosition, 1/smoothness);

			splitTargetPosition = new Vector3(player2.position.x, player2.position.y, -10)+ test * testFloat;
			splitScreenCam.transform.position = Vector3.Lerp(splitScreenCam.transform.position, splitTargetPosition, 1/smoothness);

		}
		else
		{

		//Vector3 test = splitScreen ? Vector3.Normalize(player1.position - player2.position) :Vector3.zero;

			mainTargetPosition = new Vector3((player1.position.x + player2.position.x) / 2, (player1.position.y + player2.position.y) / 2, -10) - test * testFloat;
			transform.position = Vector3.Lerp(transform.position, mainTargetPosition, 1/smoothness);
			splitScreenCam.transform.position = transform.position;
		}
        //transform.position = Vector3.MoveTowards(transform.position, targetPosition, 1 / smoothness);
	}*/


	void ResizeMask()
	{
		float camHeight = Camera.main.orthographicSize;

		if (camHeight != currentCamHeight)
		{
			currentCamHeight = camHeight;

			Vector3 viewPort = new Vector3(0, 0, -10);
			Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(viewPort);
			viewPort = new Vector3(1, 1, -10);
			Vector3 topRight = Camera.main.ViewportToWorldPoint(viewPort);

			float maskHeight = Mathf.Sqrt(1 + Mathf.Pow(Camera.main.aspect, 2));
			//Debug.Log(maskHeight);



			if (isCamera1)
			{
				Transform camMask = pivot.transform.FindChild("Mask").transform;
				camMask.localScale = new Vector3(maskHeight / 2f, 1f, maskHeight);
				camMask.localPosition = new Vector3(5f * (maskHeight / 2f), 0f, 0f);

				Transform dividerLine = pivot.transform.FindChild("Line").transform;
				dividerLine.localScale = new Vector3(.01f, 1f, maskHeight);
			}
			else
			{
				Transform camMask = pivot.transform.FindChild("Mask").transform;
				camMask.localScale = new Vector3(maskHeight / 2f, 1f, maskHeight);
				camMask.localPosition = new Vector3(-5f * (maskHeight / 2f), 0f, 0f);
			}
		}
	}
}
