using UnityEngine;
using System.Collections;

public class ConstrainOnDirection : MonoBehaviour {

	public Vector3 constrainToDirection = new Vector3(0, 0, 1);
	public Space directionSpace = Space.Self;
	private Vector3 oldPosition;
	public Rigidbody body;

	void Start()
	{
		if (body == null)
		{
			body = GetComponent<Rigidbody>();
		}
		oldPosition = transform.position;
	}

	void Update()
	{
		if (transform.position != oldPosition)
		{
			Vector3 usableDirection = constrainToDirection;
			if (directionSpace == Space.Self)
			{
				usableDirection = transform.TransformDirection(constrainToDirection);
			}

			transform.position = oldPosition + Helper.ProjectVector(usableDirection, transform.position - oldPosition);


			if (body != null && body.velocity.sqrMagnitude > 0)
			{
				body.velocity = Helper.ProjectVector(usableDirection, body.velocity);
			}
		}
	}

	void OnDrawGizmos()
	{
		Vector3 usableDirection = constrainToDirection;
		if (directionSpace == Space.Self)
		{
			usableDirection = transform.TransformDirection(constrainToDirection);
		}
		Gizmos.color = Color.white;
		Gizmos.DrawLine(transform.position, transform.position + (usableDirection * 10));
	}
}
