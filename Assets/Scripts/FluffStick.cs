using UnityEngine;
using System.Collections;

public class FluffStick : MonoBehaviour {
	public float attachOffset = 0.3f;

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Pulse")
		{
			other.SendMessage("AttachTo", gameObject);	
		}
	}
}
