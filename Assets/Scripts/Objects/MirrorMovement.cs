using UnityEngine;
using System.Collections;

public class MirrorMovement : MonoBehaviour {

    public GameObject objectToMirror;

    public bool mirrorX;
    public bool mirrorY;
    public bool mirrorZ;

    public bool useLateUpdate;


    private Vector3 otmLastPosition;

    private bool startTracking = true;

	// Use this for initialization
	void Start () {
        
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (objectToMirror != null)
        {
			otmLastPosition = startTracking == true ? objectToMirror.transform.localPosition : otmLastPosition;

            if(startTracking)
                gameObject.GetComponent<MirrorPaint>().playerToMirror = objectToMirror;

            if (otmLastPosition != Vector3.zero)
            {
                Vector3 otmCurrentPosition;
                Vector3 modifyPosition;
				otmCurrentPosition = objectToMirror.transform.localPosition;
                modifyPosition = otmCurrentPosition - otmLastPosition;

               // Debug.Log("Last Pos: " + otmLastPosition);
                //Debug.Log("Current Pos: " + otmCurrentPosition);
                //Debug.Log("Modify Pos: " + modifyPosition);
                if (mirrorX)
                {
                    modifyPosition.x = -modifyPosition.x;
                }
                if (mirrorY)
                {
                    modifyPosition.y = -modifyPosition.y;
                }
                if (mirrorZ)
                {
                    modifyPosition.z = -modifyPosition.z;
                }


				transform.localPosition += modifyPosition;
                    if (startTracking)
						transform.localPosition = new Vector3(transform.localPosition.x, objectToMirror.transform.localPosition.y, transform.localPosition.z);

					otmLastPosition = objectToMirror.transform.localPosition;
                startTracking = false;
            }
        }
        else
        {
            startTracking = true;
            gameObject.GetComponent<MirrorPaint>().playerToMirror = null;
        }
	
	}

    

   
}
