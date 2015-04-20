using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	//[HideInInspector]
	public Transform player1;
	//[HideInInspector]
	public Transform player2;
	public float smoothness = 10;
	private Vector3 mainTargetPosition;


	public Camera childMainCamera;
    public Camera otherCamera;
	private float centeringDistance = 0.0f;
	public Vector3 cameraOffset = new Vector3(0, 0, -100);

	public GameObject pivot;
    private GameObject line;
	private float currentCamHeight = 0f;
	private float currentCamAspect = 0f;
	public bool isCamera1;

    private float playerDistanceX = 0f;
    private float playerDistanceY = 0f;
    private Vector3 betweenPlayers = Vector3.zero;

    private float largerPlayerDistance = 0f;
    private Color lineColor = Color.white;

    private float zViewportDistance = 0f;


    void Start()
    {
        line = pivot.transform.FindChild("Line").gameObject;
    }

    void FixedUpdate()
    {
        //Set the centeringDistance
        centeringDistance = CameraSplitter.Instance.splitterDistanceInWorldSpace / 2.0f;

        //create the offset
        betweenPlayers = player2.position - player1.position;
        mainTargetPosition = player1.position + (betweenPlayers.normalized * centeringDistance);



        if (CameraSplitter.Instance.playerDistance > CameraSplitter.Instance.splitLineDistanceInWorldSpace)
        {
            float distanceShift = CameraSplitter.Instance.playerDistance - CameraSplitter.Instance.splitLineDistanceInWorldSpace;
            float splitShift = CameraSplitter.Instance.splitterDistanceInWorldSpace - CameraSplitter.Instance.splitLineDistanceInWorldSpace;

            lineColor.a = Mathf.Clamp((distanceShift / splitShift), 0.0f, 1.0f);
        }
        else
            lineColor.a = 0.0f;

        line.renderer.material.color = lineColor;
        

        if (!CameraSplitter.Instance.merging)
        {
            transform.position = Vector3.Lerp(transform.position, mainTargetPosition + cameraOffset, 1/smoothness);
        }
        else
        {
            mainTargetPosition = (player1.position + player2.position) / 2;
            transform.position = Vector3.Lerp(transform.position, mainTargetPosition + cameraOffset, 1/smoothness);
            
        }

        #region Layer Masks
        //Resize and Rotate Masks////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        if (childMainCamera.isOrthoGraphic)
            ResizeMask();
        else
            ResizeMaskPerspective();
        if (isCamera1)
        {
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, Vector3.Cross(player1.transform.position - player2.transform.position, Vector3.forward));
            Vector3 rotationEuler = rotation.eulerAngles;
            rotationEuler.x = rotationEuler.y = 0;
            pivot.transform.rotation = Quaternion.Euler(rotationEuler);
        }
        else
        {
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, Vector3.Cross(player2.transform.position - player1.transform.position, Vector3.forward));
            Vector3 rotationEuler = rotation.eulerAngles;
            rotationEuler.x = rotationEuler.y = 0;
            pivot.transform.rotation = Quaternion.Euler(rotationEuler);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
    }

    void FadeLine()
    {
        
    }

	void ResizeMask()
	{
		float camHeight = 2f * childMainCamera.orthographicSize;
		float camAspect = childMainCamera.aspect;
        float camWidth = camHeight * camAspect;
		if (camHeight != currentCamHeight || camAspect != currentCamAspect)
		{
			currentCamHeight = camHeight;
			currentCamAspect = camAspect;

            float maskHeight = Mathf.Sqrt(Mathf.Pow(camHeight, 2) + Mathf.Pow(camWidth, 2)) /10;	

			if (isCamera1)
			{
				Transform camMask = pivot.transform.FindChild("Mask").transform;
				camMask.localScale = new Vector3(maskHeight / 2f, 1f, maskHeight);
				camMask.localPosition = new Vector3(5f * (maskHeight / 2f), 0f, 0f);
			}
			else
			{
				Transform camMask = pivot.transform.FindChild("Mask").transform;
				camMask.localScale = new Vector3(maskHeight / 2f, 1f, maskHeight);
				camMask.localPosition = new Vector3(-5f * (maskHeight / 2f), 0f, 0f);
				
			}

			Transform dividerLine = pivot.transform.FindChild("Line").transform;
			dividerLine.localScale = new Vector3(.03f, 1f, maskHeight);
		}
	}

    void ResizeMaskPerspective()
    {
        Vector3 heading = pivot.transform.position - childMainCamera.transform.position;
        float distance = Vector3.Dot(heading, childMainCamera.transform.forward);

        float camHeight = 2.0f * (distance - 1f) * Mathf.Tan(childMainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float camAspect = childMainCamera.aspect;
        float camWidth = camHeight * camAspect;
        
        if (camHeight != currentCamHeight || camAspect != currentCamAspect)
        {
            currentCamHeight = camHeight;
            currentCamAspect = camAspect;

            float maskHeight = Mathf.Sqrt(Mathf.Pow(camHeight, 2) + Mathf.Pow(camWidth, 2)) / 10f;

            if (isCamera1)
            {
                Transform camMask = pivot.transform.FindChild("Mask").transform;
                camMask.localScale = new Vector3(maskHeight / 2f, 1f, maskHeight);
                camMask.localPosition = new Vector3(5f * (maskHeight / 2f), 0f, 0f);
            }
            else
            {
                Transform camMask = pivot.transform.FindChild("Mask").transform;
                camMask.localScale = new Vector3(maskHeight / 2f, 1f, maskHeight);
                camMask.localPosition = new Vector3(-5f * (maskHeight / 2f), 0f, 0f);

            }

            Transform dividerLine = pivot.transform.FindChild("Line").transform;
            dividerLine.localScale = new Vector3(.03f, 1f, maskHeight);
        }
    }
}
