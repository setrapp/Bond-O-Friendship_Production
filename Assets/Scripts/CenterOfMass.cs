using UnityEngine;
using System.Collections;

public class CenterOfMass : MonoBehaviour {

	public Rigidbody body;
	public GameObject centerOfMass;
	public Vector3 localOffset;

	void Start()
	{
		if (body == null)
		{
			body = GetComponent<Rigidbody>();
		}

		UpdateCenterOfMass();
	}

	public void UpdateCenterOfMass()
	{
		if (centerOfMass == null)
		{
			centerOfMass = gameObject;
		}

		body.centerOfMass = transform.InverseTransformPoint(centerOfMass.transform.position + centerOfMass.transform.TransformDirection(localOffset));
	}
}
