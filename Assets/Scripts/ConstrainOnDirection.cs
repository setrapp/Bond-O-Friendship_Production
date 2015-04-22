using UnityEngine;
using System.Collections;

public class ConstrainOnDirection : MonoBehaviour {

	public Vector3 constrainToDirection = new Vector3(0, 0, 1);
	public Space directionSpace = Space.Self;
	private Vector3 oldPosition;
	private Vector3 usableDirection;
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
				transform.TransformDirection(constrainToDirection);
			}

			transform.position = oldPosition + Helper.ProjectVector(transform.position - oldPosition, usableDirection);

			/*if (body != null && body.velocity.sqrMagnitude > 0)
			{
				body.velocity = Helper.ProjectVector(body.velocity, usableDirection);
			}*/
		}
		
	}
}
