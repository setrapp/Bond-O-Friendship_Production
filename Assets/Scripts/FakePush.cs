using UnityEngine;
using System.Collections;

public class FakePush : MonoBehaviour {
	public Rigidbody pushableBody;
	public float pushScale = 1;

	void OnCollisionStay(Collision collision)
	{
		Rigidbody collidedBody = collision.collider.GetComponent<Rigidbody>();
		if (collidedBody != null)
		{
			Vector3 force = (collidedBody.velocity - pushableBody.velocity) * (collidedBody.mass);
			pushableBody.AddForce(force, ForceMode.VelocityChange);
			Debug.Log(force);
		}
	}
}
