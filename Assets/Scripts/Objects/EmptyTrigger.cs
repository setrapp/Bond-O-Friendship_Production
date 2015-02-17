using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EmptyTrigger : MonoBehaviour {
	public int collisions = 0;
	public GameObject listener;
	private bool waitFrame;

	void Start()
	{
		waitFrame = true;
	}
	
	void Update()
	{
		if (collisions == 0 && !waitFrame)
		{
			if (listener != null)
			{
				listener.SendMessage("TriggerEmpty", gameObject);
			}
		}
		waitFrame = false;
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
