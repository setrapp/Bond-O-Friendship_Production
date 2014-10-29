using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public Transform player1;
    public Transform player2;
    public float smoothness = 20;
	private Vector3 mainTargetPosition;
	private Vector3 splitTargetPosition;


	public GameObject splitScreenCam;
	public bool splitScreen = false;
	public float testFloat = 5;

	void Update () {
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
	}
	
}
