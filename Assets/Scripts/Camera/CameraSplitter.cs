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
    public bool merging = true;
	private bool wasSplit = false;
    public float splitterDistance = .5f;
    public float splitterDistanceInWorldSpace = 0.0f;

    public float splitLineDistance = .5f;
    public float splitLineDistanceInWorldSpace = 0.0f;

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
    public float playerDistance;

    public Vector3 testPlayerOne;
    public Vector3 testPlayerTwo;


    private float cameraDistanceX;
    private float cameraDistanceY;
    private Vector3 mainCameraLocation;
    private Vector3 secondaryCameraLocation;

    private float zViewPortDistance = 0.0f;


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

    void GetSplitDistanceInWorldSpace()
    {
        Vector3 testing = player1CameraSystem.childMainCamera.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, zViewPortDistance));
        Vector3 testingg = player1CameraSystem.childMainCamera.ViewportToWorldPoint(new Vector3(0.0f, splitterDistance, zViewPortDistance));
        Vector3 fadeLineWorldPoint = player1CameraSystem.childMainCamera.ViewportToWorldPoint(new Vector3(0.0f, splitLineDistance, zViewPortDistance));

        splitterDistanceInWorldSpace = (Mathf.Abs(testingg.y - testing.y));
        splitLineDistanceInWorldSpace = (Mathf.Abs(fadeLineWorldPoint.y - testing.y));
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

        

        zViewPortDistance = testPlayerOne.z;
        GetSplitDistanceInWorldSpace();
        playerDistanceX = Mathf.Abs(player1.transform.position.x - player2.transform.position.x);
        playerDistanceY = Mathf.Abs(player1.transform.position.y - player2.transform.position.y);

        playerDistance = Vector3.Distance(player1.transform.position, player2.transform.position);
       // Debug.Log("Heading " + distance);
        //Debug.Log("V3: " + );

        mainCameraLocation = splitCamera1.transform.position;
        secondaryCameraLocation = splitCamera2.transform.position;
        //mainCameraLocation = splitCamera1.WorldToViewportPoint(splitCamera1.transform.position);
        //secondaryCameraLocation = splitCamera1.WorldToViewportPoint(splitCamera2.transform.position);
        cameraDistanceX = Mathf.Abs(mainCameraLocation.x - secondaryCameraLocation.x);
        cameraDistanceY = Mathf.Abs(mainCameraLocation.y - secondaryCameraLocation.y);

        //Debug.Log(splitterDistanceInWorldSpace);

        if (split && merging)
        {
           //Debug.Log("X: " + cameraDistanceX);// + " 2nd: " + secondaryCameraLocation.x);
           //Debug.Log("Y: " + cameraDistanceY);
        }

        //Split apart based on player distance, turn split off once the cameras are aligned
        if ((playerDistance > splitterDistanceInWorldSpace))
        {
            split = true;
            merging = false;
        } 
        else if (cameraDistanceX < .1f && cameraDistanceY < .1f && split)
        {
            split = false;
        }
        else
            merging = true;

        //Toggle split screen on and off
		if (split != wasSplit || forceCheck)
		{
            player1CameraSystem.pivot.transform.FindChild("Mask").gameObject.SetActive(split);
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
