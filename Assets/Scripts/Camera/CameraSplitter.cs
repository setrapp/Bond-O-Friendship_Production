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


    public float splitterDistance = .5f;
    public float splitterDistanceInWorldSpace = 0.0f;

    public float splitLineFadeDistance = .5f;
    public float splitLineFadeDistanceInWorldSpace = 0.0f;

	public CameraFollow player1CameraSystem;
	public CameraFollow player2CameraSystem;
	//[HideInInspector]
	public Camera splitCamera1;
	//[HideInInspector]
	public Camera splitCamera2;

	public GameObject player1;
	public GameObject player2;
	public AudioListener audioListener;

    public float playerDistance;
    private float zViewPortDistance = 0.0f;


	void Start()
	{
		if (Globals.Instance != null)
		{
			player1 = Globals.Instance.player1.gameObject;
			player2 = Globals.Instance.player2.gameObject;

            player1CameraSystem.player1 = player2CameraSystem.player2 = player1.transform;
            player1CameraSystem.player2 = player2CameraSystem.player1 = player2.transform;

			splitCamera1 = player1CameraSystem.GetComponentInChildren<Camera>();
			splitCamera2 = player2CameraSystem.GetComponentInChildren<Camera>();
		}
		wasSplit = split;
		CheckSplit(true);
	}

	void Update()
	{
		if (splittable)
			CheckSplit(false);	

		audioListener.transform.position = (player1.transform.position + player2.transform.position) / 2;
	}

    void SetSplitDistanceInWorldSpace()
    {
        Vector3 viewportCornerPoint = player1CameraSystem.childMainCamera.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, zViewPortDistance));
        Vector3 viewportSplitPoint = player1CameraSystem.childMainCamera.ViewportToWorldPoint(new Vector3(0.0f, splitterDistance, zViewPortDistance));
        Vector3 fadeLineWorldPoint = player1CameraSystem.childMainCamera.ViewportToWorldPoint(new Vector3(0.0f, splitLineFadeDistance, zViewPortDistance));

        splitterDistanceInWorldSpace = (Mathf.Abs(viewportSplitPoint.y - viewportCornerPoint.y));
        splitLineFadeDistanceInWorldSpace = (Mathf.Abs(fadeLineWorldPoint.y - viewportCornerPoint.y));
    }

    private void SetzViewPortDistance()
    {
       zViewPortDistance = player1CameraSystem.childMainCamera.WorldToViewportPoint(player1.transform.position).z;
    }

	private void CheckSplit(bool forceCheck)
	{
        SetzViewPortDistance();
        SetSplitDistanceInWorldSpace();
        playerDistance = Vector3.Distance(player1.transform.position, player2.transform.position);
        split = playerDistance > splitterDistanceInWorldSpace;

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
        if (split && player == player2)
            return player2CameraSystem.GetComponentInChildren<Camera>();
        else
            return player1CameraSystem.GetComponentInChildren<Camera>();
	}

	public void JumpToPlayers()
	{
		if (Globals.Instance != null && Globals.Instance.player1 != null && Globals.Instance.player2 != null)
		{
			Vector3 oldCamPos = transform.position;
			Vector3 newCamPos = ((player1.transform.position + player2.transform.position) / 2);
			CameraSplitter.Instance.transform.position = new Vector3(newCamPos.x, newCamPos.y, oldCamPos.z);
		}
	}
}
