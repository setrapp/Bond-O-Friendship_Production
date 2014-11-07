using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour {

	public bool pOneTriggered = false;
	public bool pTwoTirggered = false;
	
	// Use this for initialization
	void Start () {
		GetComponent<Renderer>().material.color = Color.cyan;
	
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
				GetComponent<Renderer>().material.color = collide.gameObject.GetComponent<Renderer>().material.color;
				pOneTriggered = true;
				pTwoTirggered = false;
			}
			if(collide.gameObject.GetComponent<MovePulse>().creator.name == "Player 2")
			{
				GetComponent<Renderer>().material.color = collide.gameObject.GetComponent<Renderer>().material.color;
				pOneTriggered = false;
				pTwoTirggered = true;
			}
		}
	}
}
