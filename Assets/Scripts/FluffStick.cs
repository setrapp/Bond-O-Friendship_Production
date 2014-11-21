using UnityEngine;
using System.Collections;

public class FluffStick : MonoBehaviour {
	public Rigidbody pullableBody;
	public float pullMass = -1;

	void Start()
	{
		if (pullableBody == null)
		{
			pullableBody = GetComponent<Rigidbody>();
		}
		if (pullMass < 0 && pullableBody != null)
		{
			pullMass = pullableBody.mass;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Pulse")
		{
			other.SendMessage("AttachTo", this);
		}
	}
}
