using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public bool isMainCamera;
	//[HideInInspector]
	public Transform player1;
	//[HideInInspector]
	public Transform player2;
	public Vector3 centerOffset;

	private Vector3 mainTargetPosition;
    private Vector3 betweenPlayers = Vector3.zero;
    private float centeringDistance = 0.0f;
    [HideInInspector]
    //public Vector3 cameraOffset = new Vector3(0, 0, -100);

	public Camera childMainCamera;

	public GameObject pivot;
	[HideInInspector]
    public GameObject line;
    private GameObject camMask;
	private float lineWidth = 0.0f;
    
    private Color lineColor = Color.white;

    private float currentCamHeight = 0f;
    private float currentCamWidth = 0f;

    public float dampTime = .15f;
    public float dampingMultiplyer = .15f;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        line = pivot.transform.FindChild("Line").gameObject;
        camMask = pivot.transform.FindChild("Mask").gameObject;
    }

    void Update()
    {
        if (CameraSplitter.Instance.followPlayers) 
		{
            MoveCamera();
            
        }
		FadeSplitScreenLine();
		RotateAndResizeMasks();
    }

    private void MoveCamera()
    {
        centeringDistance = CameraSplitter.Instance.splitterDistanceInWorldSpace / 2.0f;

        CheckPlayers();
        betweenPlayers = player2.position - player1.position;
        mainTargetPosition = CameraSplitter.Instance.split ? (player1.position + (betweenPlayers.normalized * centeringDistance)) : ((player1.position + player2.position) / 2);
        mainTargetPosition += centerOffset;
        mainTargetPosition.z = transform.position.z;

        if (CameraSplitter.Instance.split)
        {
            if (CameraSplitter.Instance.playerDistance < CameraSplitter.Instance.cameraDampDistanceInWorldSpace)
            {
                float distanceShift = CameraSplitter.Instance.cameraDampDistanceInWorldSpace - CameraSplitter.Instance.playerDistance;
                float splitShift = CameraSplitter.Instance.cameraDampDistanceInWorldSpace - CameraSplitter.Instance.splitterDistanceInWorldSpace;
                dampTime = (1.0f - Mathf.Clamp((distanceShift / splitShift), 0.0f, 1.0f)) * dampingMultiplyer;
            }
            else
            {
                dampTime = dampingMultiplyer;
            }
        }
        else
        {
            if (CameraSplitter.Instance.playerDistance > CameraSplitter.Instance.splitLineFadeDistanceInWorldSpace)
            {
                float distanceShift = CameraSplitter.Instance.playerDistance - CameraSplitter.Instance.splitLineFadeDistanceInWorldSpace;
                float splitShift = CameraSplitter.Instance.splitterDistanceInWorldSpace - CameraSplitter.Instance.splitLineFadeDistanceInWorldSpace;
                dampTime = (1.0f - Mathf.Clamp((distanceShift / splitShift), 0.0f, 1.0f)) * dampingMultiplyer;
            }
            else
            {
                dampTime = dampingMultiplyer;
            }
        }

        transform.position = Vector3.SmoothDamp(transform.position, mainTargetPosition, ref velocity, dampTime);


    }
    private void FadeSplitScreenLine()
    {
        if (CameraSplitter.Instance.playerDistance > CameraSplitter.Instance.splitLineFadeDistanceInWorldSpace) {
			float distanceShift = CameraSplitter.Instance.playerDistance - CameraSplitter.Instance.splitLineFadeDistanceInWorldSpace;
			float splitShift = CameraSplitter.Instance.splitterDistanceInWorldSpace - CameraSplitter.Instance.splitLineFadeDistanceInWorldSpace;
			lineColor.a = Mathf.Clamp((distanceShift / splitShift), 0.0f, 1.0f);
			lineWidth = Mathf.Clamp ((distanceShift / splitShift), 0.0f, 1.0f);
		} 
		else 
		{
			lineColor.a = 0.0f;
			lineWidth= 0.0f;
		}

		line.transform.localScale = Vector3.Lerp (new Vector3 (0.0f, line.transform.localScale.y, line.transform.localScale.z), new Vector3 (0.03f, line.transform.localScale.y, line.transform.localScale.z), lineWidth);
        line.GetComponent<Renderer>().material.color = lineColor;
    }

    private void RotateAndResizeMasks()
    {
		CheckPlayers();

        if (childMainCamera.orthographic)
            ResizeMaskOrthographic();
        else
            ResizeMaskPerspective();

        Quaternion rotation = new Quaternion();
        if (isMainCamera)
            rotation = Quaternion.FromToRotation(Vector3.up, Vector3.Cross(player1.transform.position - player2.transform.position, Vector3.forward));
        else
            rotation = Quaternion.FromToRotation(Vector3.up, Vector3.Cross(player2.transform.position - player1.transform.position, Vector3.forward));

        Vector3 rotationEuler = rotation.eulerAngles;
        rotationEuler.x = rotationEuler.y = 0;
        pivot.transform.rotation = Quaternion.Euler(rotationEuler);
    }

    void ResizeMaskOrthographic()
	{
		float camHeight = 2f * childMainCamera.orthographicSize;
        float camWidth = camHeight * childMainCamera.aspect;
        ResizeMask(camHeight, camWidth);
	}

    void ResizeMaskPerspective()
    {
        Vector3 heading = pivot.transform.position - childMainCamera.transform.position;
        float distance = Vector3.Dot(heading, childMainCamera.transform.forward);
        float camHeight = 2.0f * (distance - 1f) * Mathf.Tan(childMainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float camWidth = camHeight * childMainCamera.aspect;
        ResizeMask(camHeight, camWidth);
    }

    void ResizeMask(float camHeight, float camWidth)
    {
        if (camHeight != currentCamHeight || camWidth != currentCamWidth)
        {
            float maskHeight = Mathf.Sqrt(Mathf.Pow(camHeight, 2) + Mathf.Pow(camWidth, 2)) / 10f;

            //Scale and Position the mask
            camMask.transform.localScale = new Vector3(maskHeight / 2f, 1f, maskHeight);
            if (isMainCamera)               
                camMask.transform.localPosition = new Vector3(5f * (maskHeight / 2f), 0f, 0f);
            else
                camMask.transform.localPosition = new Vector3(-5f * (maskHeight / 2f), 0f, 0f);
            //Scale the dividing line
            line.transform.localScale = new Vector3(line.transform.localScale.x, 1f, maskHeight);

            currentCamHeight = camHeight;
            currentCamWidth = camWidth;
        }
    }

	private void CheckPlayers()
	{
		if (player1 == null || player2 == null)
		{
			CameraSplitter.Instance.SetPlayers();
		}
	}
}
