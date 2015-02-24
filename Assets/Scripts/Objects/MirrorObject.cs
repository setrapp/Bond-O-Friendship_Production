using UnityEngine;
using System.Collections;

public class MirrorObject : MonoBehaviour
{

    public GameObject objectToMirror;

    public bool mirrorX;
    public bool mirrorY;
    public bool mirrorZ;

    private Vector3 otmLastPosition;

    private bool startTracking = true;


    // Update is called once per frame
    void Update()
    {
        if (objectToMirror != null)
        {
            otmLastPosition = startTracking == true ? objectToMirror.transform.position : otmLastPosition;

            if (otmLastPosition != Vector3.zero)
            {
                Vector3 otmCurrentPosition;
                Vector3 modifyPosition;
                otmCurrentPosition = objectToMirror.transform.position;
                modifyPosition = otmCurrentPosition - otmLastPosition;

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
                    modifyPosition.y = -modifyPosition.y;
                }


                transform.position += modifyPosition;
                if (startTracking)
                    transform.position = new Vector3(transform.position.x, objectToMirror.transform.position.y, transform.position.z);

                otmLastPosition = objectToMirror.transform.position;
                startTracking = false;
            }
        }

    }




}
