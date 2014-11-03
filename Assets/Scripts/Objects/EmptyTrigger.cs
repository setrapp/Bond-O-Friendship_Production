using UnityEngine;
using System.Collections;

public class EmptyTrigger : MonoBehaviour {
	private int collisions = 0;
	private bool collisionsChecked = false;
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
		if (other.tag == "Object")
		{
			collisions++;
			collisionsChecked = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Object")
		{
			collisions--;
			collisionsChecked = true;
		}
	}
}
