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
			otmLastPosition = startTracking == true ? objectToMirror.transform.localPosition : otmLastPosition;

			if (otmLastPosition != Vector3.zero)
			{
				Vector3 otmCurrentPosition;
				Vector3 modifyPosition;
				otmCurrentPosition = objectToMirror.transform.localPosition;
				modifyPosition = otmCurrentPosition - otmLastPosition;

				/*if (mirrorX)
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
				}*/


				transform.localPosition += modifyPosition;
				if (startTracking)
					transform.localPosition = new Vector3(transform.localPosition.x, objectToMirror.transform.localPosition.y, transform.localPosition.z);

				otmLastPosition = objectToMirror.transform.localPosition;
				startTracking = false;
			}
		}

	}




}
