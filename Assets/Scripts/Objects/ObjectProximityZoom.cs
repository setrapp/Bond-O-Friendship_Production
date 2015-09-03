using UnityEngine;
using System.Collections;

public class ObjectProximityZoom : MonoBehaviour {

	public GameObject zoomControl1;
	public GameObject zoomControl2;
	public float endZoom;
	private float startDistance;
	public float endDistance;
	private Vector3 oldPosition1;
	private Vector3 oldPosition2;

	void Start()
	{
		startDistance = (zoomControl1.transform.position - zoomControl2.transform.position).magnitude;
		oldPosition1 = zoomControl1.transform.position;
		oldPosition2 = zoomControl2.transform.position;
	}

	void Update()
	{
		if (zoomControl1 != null && zoomControl2 != null) 
		{

			if (oldPosition1 != zoomControl1.transform.position || oldPosition2 != zoomControl2.transform.position) {
				float startZoom = CameraSplitter.Instance.startPos.z;
				float zoomProgress = ((zoomControl1.transform.position - zoomControl2.transform.position).magnitude - startDistance) / (endDistance - startDistance);
				float currentZoom = ((1 - zoomProgress) * startZoom) + (zoomProgress * endZoom);
				Vector3 cameraPos = CameraSplitter.Instance.transform.position;
				cameraPos.z = currentZoom;
				CameraSplitter.Instance.transform.position = cameraPos;
			}

			oldPosition1 = zoomControl1.transform.position;
			oldPosition2 = zoomControl2.transform.position;
		}
	}
}
