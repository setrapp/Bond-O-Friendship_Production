using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EmptyTrigger : MonoBehaviour {
	[SerializeField]
	public List<Collider> checkedColliders;
	public int collisions = 0;
	public GameObject listener;
	private bool waitFrame;
	private bool sentEmpty = false;

	void Start()
	{
		waitFrame = true;
	}
	
	void Update()
	{
		if (collisions == 0 && !waitFrame && !sentEmpty)
		{
			if (listener != null)
			{
				listener.SendMessage("TriggerEmpty", gameObject);
			}
			sentEmpty = true;
		}
		waitFrame = false;
	}

	void OnTriggerEnter(Collider other)
	{
		if (checkedColliders.Contains(other))
		{
			collisions++;
			sentEmpty = false;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (checkedColliders.Contains(other))
		{
			collisions--;
			sentEmpty = false;
		}
	}
}
