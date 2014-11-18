using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour {

	public bool pOneTriggered = false;
	public bool pTwoTirggered = false;
	private Color myColor;
	
	// Use this for initialization
	void Start () {
		myColor = new Color(0.9f,0.4f,2.0f,1.0f);
		GetComponent<Renderer>().material.color = myColor;
	
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
				//print("Collide");
				GetComponent<Renderer>().material.color = collide.gameObject.GetComponent<MovePulse>().creator.partnerLink.headRenderer.material.color;
				pOneTriggered = true;
				pTwoTirggered = false;
			}
			if(collide.gameObject.GetComponent<MovePulse>().creator.name == "Player 2")
			{
				//print("Collide");
				GetComponent<Renderer>().material.color = collide.gameObject.GetComponent<MovePulse>().creator.partnerLink.headRenderer.material.color;
				pOneTriggered = false;
				pTwoTirggered = true;
			}
		}
	}
}
