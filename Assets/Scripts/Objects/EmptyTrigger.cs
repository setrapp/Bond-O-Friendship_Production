using UnityEngine;
using System.Collections;

public class EmptyTrigger : MonoBehaviour {
	private int collisions = 0;
	public GameObject listener;

	void Update()
	{
		if (collisions == 0)
		{
			if (listener != null)
			{
				listener.SendMessage("TriggerEmpty", gameObject);
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Object" || other.tag == "Pushable")
		{
			collisions++;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Object" || other.tag == "Pushable")
		{
			collisions--;
		}
	}
}
