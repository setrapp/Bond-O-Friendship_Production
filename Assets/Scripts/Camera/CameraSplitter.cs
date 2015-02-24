using UnityEngine;
using System.Collections;

public class CameraSplitter : MonoBehaviour {
	private static CameraSplitter instance;
	public static CameraSplitter Instance
	{
		get
		{
 			if (instance == null)
			{
				instance = GameObject.FindGameObjectWithTag("CameraSystem").GetComponent<CameraSplitter>();
			}
			return instance;
		}
	}
	public bool splittable = true;
	public bool split = false;
	private bool wasSplit = false;
	public float splitDistance;
	public float combineDistance;
	public CameraFollow combinedCameraSystem;
	public CameraFollow player1CameraSystem;
	public CameraFollow player2CameraSystem;
	//[HideInInspector]
	public Camera combinedCamera;
	//[HideInInspector]
	public Camera splitCamera1;
	//[HideInInspector]
	public Camera splitCamera2;
	public GameObject player1;
	public GameObject player2;
	private bool justAltered;
	public AudioListener audioListener;

	void Start()
	{
		if (Globals.Instance != null)
		{
			player1 = Globals.Instance.player1.gameObject;
			player2 = Globals.Instance.player2.gameObject;
			combinedCameraSystem.transform.position = (player1.transform.position + player2.transform.position) / 2;
			player1CameraSystem.transform.position = player1.transform.position;
			player2CameraSystem.transform.position = player2.transform.position;

			combinedCameraSystem.player1 = player1CameraSystem.player1 = player2CameraSystem.player2 = player1.transform;
			combinedCameraSystem.player2 = player1CameraSystem.player2 = player2CameraSystem.player1 = player2.transform;

			combinedCamera = combinedCameraSystem.GetComponentInChildren<Camera>();
			splitCamera1 = player1CameraSystem.GetComponentInChildren<Camera>();
			splitCamera2 = player2CameraSystem.GetComponentInChildren<Camera>();
		}
		wasSplit = split;
		CheckSplit(true);
	}

	void Update()
	{
		//if (splittable)
		{
			CheckSplit(false);
		}
		

		audioListener.transform.position = (player1.transform.position + player2.transform.position) / 2;
	}

	private void CheckSplit(bool forceCheck)
	{
		Vector3 testPlayerOne;
		Vector3 testPlayerTwo;
		float splitUpperBound = 0.9f;
		float splitLowerBound = 0.1f;
		float combineUpperBound = 0.85f;
		float combineLowerBound = 0.15f;

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

		if (split != wasSplit || forceCheck)
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

	public Camera GetFollowingCamera(GameObject player)
	{
		if (split && (player == player1 || player == player2))
		{
			if (player == player1)
			{
				return player1CameraSystem.GetComponentInChildren<Camera>();
			}
			else
			{
				return player2CameraSystem.GetComponentInChildren<Camera>();
			}
		}
		else
		{
			return combinedCameraSystem.GetComponentInChildren<Camera>();
		}
	}

	public void JumpToPlayers()
	{
		if (Globals.Instance != null && Globals.Instance.player1 != null && Globals.Instance.player2 != null)
		{
			Vector3 oldCamPos = transform.position;
			Vector3 newCamPos = ((Globals.Instance.player1.transform.position + Globals.Instance.player2.transform.position) / 2);
			CameraSplitter.Instance.transform.position = new Vector3(newCamPos.x, newCamPos.y, oldCamPos.z);
		}
	}
}
