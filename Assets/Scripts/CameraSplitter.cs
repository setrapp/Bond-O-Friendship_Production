using UnityEngine;
using System.Collections;

public class CameraSplitter : MonoBehaviour {

	public bool split = false;
	private bool wasSplit = false;
	public float splitDistance;
	public float combineDistance;
	public GameObject combinedCameraSystem;
	public GameObject player1CameraSystem;
	public GameObject player2CameraSystem;
	public GameObject player1;
	public GameObject player2;
	private bool justAltered;

	void Start()
	{
		wasSplit = !split;
		CheckSplit();
	}

	void Update()
	{
		CheckSplit();
	}

	private void CheckSplit()
	{
		Vector3 testPlayerOne;// = Camera.main.WorldToViewportPoint(player1.transform.position);
		Vector3 testPlayerTwo;// = Camera.main.WorldToViewportPoint(player2.transform.position);
		float splitUpperBound = 0.9f;
		float splitLowerBound = 0.1f;
		float combineUpperBound = 0.7f;
		float combineLowerBound = 0.3f;

		if (!split)
		{
			testPlayerOne = combinedCameraSystem.GetComponent<CameraFollow>().childMainCamera.WorldToViewportPoint(player1.transform.position);
			testPlayerTwo = combinedCameraSystem.GetComponent<CameraFollow>().childMainCamera.WorldToViewportPoint(player2.transform.position);
		}
		else
		{
			testPlayerOne = player2CameraSystem.GetComponent<CameraFollow>().childMainCamera.WorldToViewportPoint(player1.transform.position);
			testPlayerTwo = player1CameraSystem.GetComponent<CameraFollow>().childMainCamera.WorldToViewportPoint(player2.transform.position);
		}

		if ((testPlayerTwo.x > combineLowerBound && testPlayerTwo.x < combineUpperBound && testPlayerTwo.y > combineLowerBound && testPlayerTwo.y < combineUpperBound))
		{
			split = false;
		}
		else if (testPlayerOne.x < splitLowerBound || testPlayerOne.x > splitUpperBound || testPlayerOne.y < splitLowerBound || testPlayerOne.y > splitUpperBound || testPlayerTwo.x < splitLowerBound || testPlayerTwo.x > splitUpperBound || testPlayerTwo.y < splitLowerBound || testPlayerTwo.y > splitUpperBound)
		{
			split = true;
		}

		if (split != wasSplit)
		{
			for (int i = 0; i < combinedCameraSystem.transform.childCount; i++)
			{
				combinedCameraSystem.transform.GetChild(i).gameObject.SetActive(!split);
			}
			for (int i = 0; i < player1CameraSystem.transform.childCount; i++)
			{
				player1CameraSystem.transform.GetChild(i).gameObject.SetActive(split);
			}
			for (int i = 0; i < player2CameraSystem.transform.childCount; i++)
			{
				player2CameraSystem.transform.GetChild(i).gameObject.SetActive(split);
			}
		}

		wasSplit = split;
	}
}
