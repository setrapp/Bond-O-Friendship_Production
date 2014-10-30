using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour {

	public bool pOneTriggered = false;
	public bool pTwoTirggered = false;
	
	// Use this for initialization
	void Start () {
		renderer.material.color = Color.white;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter(Collider collide)
	{
		if(collide.gameObject.tag == "Pulse")
		{
			if(collide.gameObject.GetComponent<MovePulse>().creator.name == "Player 1")
			{
				renderer.material.color = collide.gameObject.renderer.material.color;
				pOneTriggered = true;
				pTwoTirggered = false;
			}
			if(collide.gameObject.GetComponent<MovePulse>().creator.name == "Player 2")
			{
				renderer.material.color = collide.gameObject.renderer.material.color;
				pOneTriggered = false;
				pTwoTirggered = true;
			}
		}
	}
}
