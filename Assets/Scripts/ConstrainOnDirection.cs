using UnityEngine;
using System.Collections;

public class ConstrainOnDirection : MonoBehaviour {

	public Vector3 constrainToDirection = new Vector3(0, 0, 1);
	public Space directionSpace = Space.Self;
	private Vector3 oldPosition;
	private Vector3 oldLocalPosition;
	public Rigidbody body;

	void Start()
	{
		if (body == null)
		{
			body = GetComponent<Rigidbody>();
		}

		oldPosition = transform.position;
		oldLocalPosition = transform.localPosition;
	}

	void FixedUpdate()
	{
		if (transform.position != oldPosition)
		{
			Vector3 usableDirection = constrainToDirection;
			if (directionSpace == Space.Self)
			{
				usableDirection = transform.TransformDirection(constrainToDirection);
				oldPosition = transform.parent.TransformPoint(oldLocalPosition);
			}

			if (body != null && body.velocity.sqrMagnitude > 0 && !body.isKinematic)
			{
				body.MovePosition(oldPosition + Helper.ProjectVector(usableDirection, transform.position - oldPosition));
				body.velocity = Helper.ProjectVector(usableDirection, body.velocity);
			}
			else if (body == null)
			{
				transform.position = oldPosition + Helper.ProjectVector(usableDirection, transform.position - oldPosition);
			}
		}
	}

	public void ResetWithDirection(Vector3 newDirection)
	{
		constrainToDirection = newDirection;
		oldPosition = transform.position;
		oldLocalPosition = transform.localPosition;
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
