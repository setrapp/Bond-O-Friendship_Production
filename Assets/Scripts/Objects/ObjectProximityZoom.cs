using UnityEngine;
using System.Collections;

public class ObjectProximityZoom : MonoBehaviour {

	public GameObject zoomControl1;
	public GameObject zoomControl2;
	public float endZoom;
	private float startDistance;
	public float endDistance;
	private Vector3 oldLocalPosition1;
	private Vector3 oldLocalPosition2;

	void Start()
	{
		startDistance = (zoomControl1.transform.position - zoomControl2.transform.position).magnitude;
		oldLocalPosition1 = zoomControl1.transform.localPosition;
		oldLocalPosition2 = zoomControl2.transform.localPosition;
	}

	void Update()
	{
		if (zoomControl1 != null && zoomControl2 != null) 
		{

			if (oldLocalPosition1 != zoomControl1.transform.localPosition || oldLocalPosition2 != zoomControl2.transform.localPosition)
			{
				float startZoom = CameraSplitter.Instance.startPos.z;
				float zoomProgress = ((zoomControl1.transform.position - zoomControl2.transform.position).magnitude - startDistance) / (endDistance - startDistance);
				float currentZoom = ((1 - zoomProgress) * startZoom) + (zoomProgress * endZoom);
				Vector3 cameraPos = CameraSplitter.Instance.transform.position;
				cameraPos.z = currentZoom;
				CameraSplitter.Instance.transform.position = cameraPos;
			}

			oldLocalPosition1 = zoomControl1.transform.localPosition;
			oldLocalPosition2 = zoomControl2.transform.localPosition;
		}
	}
}
