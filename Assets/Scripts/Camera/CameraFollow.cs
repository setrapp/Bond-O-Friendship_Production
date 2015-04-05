using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	//[HideInInspector]
	public Transform player1;
	//[HideInInspector]
	public Transform player2;
	public float smoothness = 20;
    private float smoothCombine = 8f;
	private Vector3 mainTargetPosition;


	public Camera childMainCamera;
    public Camera otherCamera;
	public bool splitScreen = false;
	private float centeringDistance = 10f;
    private float splitterDistance = .75f;
	private Vector3 cameraOffset = new Vector3(0, 0, -100);

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

    void Start()
    {
        line = pivot.transform.FindChild("Line").gameObject;
    }

    void FixedUpdate()
    {
        playerDistanceX = CameraSplitter.Instance.playerDistanceX;
        playerDistanceY = CameraSplitter.Instance.playerDistanceY;
        largerPlayerDistance = playerDistanceX > playerDistanceY ? playerDistanceX : playerDistanceY;

        //create the offset
        betweenPlayers = player2.position - player1.position;
        mainTargetPosition = player1.position + (betweenPlayers.normalized * centeringDistance);

        #region Change This
        if (largerPlayerDistance < .5f)
        {
            if(!CameraSplitter.Instance.split)
                lineColor.a = 0;
        }
        else if((largerPlayerDistance > .5f) && (largerPlayerDistance < 1f))
        {
            if (largerPlayerDistance < .6f)
                lineColor.a = .2f;
            if (largerPlayerDistance > .6f && largerPlayerDistance < .7f)
                lineColor.a = .4f;
            if (largerPlayerDistance > .8f && largerPlayerDistance < .9f)
                lineColor.a = .6f;
            if (largerPlayerDistance > .9f)
                lineColor.a = .8f;
        }
        else
        {
            lineColor.a = 1.0f;
        }
        line.renderer.material.color = lineColor;
        //Debug.Log(line.renderer.material.color);
        #endregion

        if (largerPlayerDistance > splitterDistance)
        {
            transform.position = Vector3.Lerp(transform.position, mainTargetPosition + cameraOffset, 1 / smoothness);
        }
        else
        {
            mainTargetPosition = (player1.position + player2.position) / 2;
            transform.position = Vector3.Lerp(transform.position, mainTargetPosition + cameraOffset, 1 / smoothCombine);
            
        }

        #region Layer Masks
        //Resize and Rotate Masks////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ResizeMask();
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
		float camHeight = childMainCamera.orthographicSize;
		float camAspect = childMainCamera.aspect;
		if (camHeight != currentCamHeight || camAspect != currentCamAspect)
		{
			currentCamHeight = camHeight;
			currentCamAspect = camAspect;

			float maskHeight = Mathf.Sqrt(1 + Mathf.Pow(childMainCamera.aspect, 2)) * 3;	

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
