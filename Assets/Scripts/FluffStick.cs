using UnityEngine;
using System.Collections;

public class FluffStick : MonoBehaviour {
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Pulse")
		{
			other.SendMessage("AttachTo", gameObject);	
		}
	}
}
