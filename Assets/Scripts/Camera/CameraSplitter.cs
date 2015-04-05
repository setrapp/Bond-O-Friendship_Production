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
	public CameraFollow player1CameraSystem;
	public CameraFollow player2CameraSystem;
	//[HideInInspector]
	public Camera splitCamera1;
	//[HideInInspector]
	public Camera splitCamera2;

	public GameObject player1;
	public GameObject player2;
	public AudioListener audioListener;

    public float playerDistanceX;
    public float playerDistanceY;

    public Vector3 testPlayerOne;
    public Vector3 testPlayerTwo;


    private float cameraDistanceX;
    private float cameraDistanceY;
    private Vector3 mainCameraLocation;
    private Vector3 secondaryCameraLocation;


	void Start()
	{
		if (Globals.Instance != null)
		{
			player1 = Globals.Instance.player1.gameObject;
			player2 = Globals.Instance.player2.gameObject;
            player1CameraSystem.transform.position = (player1.transform.position + player2.transform.position) / 2;
			player2CameraSystem.transform.position = player2.transform.position;


            player1CameraSystem.player1 = player1CameraSystem.player1 = player2CameraSystem.player2 = player1.transform;
            player1CameraSystem.player2 = player1CameraSystem.player2 = player2CameraSystem.player1 = player2.transform;

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
		if (!split)
		{
            testPlayerOne = player1CameraSystem.childMainCamera.WorldToViewportPoint(player1.transform.position);
            testPlayerTwo = player1CameraSystem.childMainCamera.WorldToViewportPoint(player2.transform.position);
		}
		else
		{
			testPlayerOne = player2CameraSystem.childMainCamera.WorldToViewportPoint(player1.transform.position);
			testPlayerTwo = player1CameraSystem.childMainCamera.WorldToViewportPoint(player2.transform.position);
		}

        playerDistanceX = Mathf.Abs(testPlayerOne.x - testPlayerTwo.x);
        playerDistanceY = Mathf.Abs(testPlayerOne.y - testPlayerTwo.y);

        mainCameraLocation = splitCamera1.WorldToViewportPoint(splitCamera1.transform.position);
        secondaryCameraLocation = splitCamera1.WorldToViewportPoint(splitCamera2.transform.position);
        cameraDistanceX = Mathf.Abs(mainCameraLocation.x - secondaryCameraLocation.x);
        cameraDistanceY = Mathf.Abs(mainCameraLocation.y - secondaryCameraLocation.y);

        //Split apart based on player distance, turn split off once the cameras are aligned
        if (playerDistanceX > .5f || playerDistanceY > .5f)
            split = true;
        else if (cameraDistanceX < .001f && cameraDistanceY < .001f)
        {
            split = false;
        }

        //Toggle split screen on and off
		if (split != wasSplit || forceCheck)
		{
            player1CameraSystem.pivot.gameObject.SetActive(split);
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
			return player1CameraSystem.GetComponentInChildren<Camera>();
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
