using UnityEngine;
using System.Collections;

public class SimpleConnection : MonoBehaviour {
	public LineRenderer lineRenderer;
	public GameObject attachPoint1;
	public GameObject attachPoint2;
	public float maxDistance;

	void Update()
	{
		lineRenderer.SetPosition(0, attachPoint1.transform.position);
		lineRenderer.SetPosition(1, attachPoint2.transform.position);
	}
}
