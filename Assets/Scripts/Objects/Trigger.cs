using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour {

	public bool pOneTriggered = false;
	public bool pTwoTriggered = false;
	private Color myColor;
	
	// Use this for initialization
	void Start () {
		myColor = new Color(0.8f,0.7f,0.5f,1.0f);
		GetComponent<Renderer>().material.color = myColor;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter(Collider collide)
	{
		if(collide.gameObject.tag == "Pulse")
		{
			MovePulse mover = collide.gameObject.GetComponent<MovePulse>();
			if(mover.creator != null && mover.creator.name == "Player 1")
			{
				//print("Collide");
				GetComponent<Renderer>().material.color = collide.gameObject.GetComponent<MovePulse>().creator.partnerLink.headRenderer.material.color;
				pOneTriggered = true;
				pTwoTriggered = false;
			}
			if(mover.creator != null && mover.creator.name == "Player 2")
			{
				//print("Collide");
				GetComponent<Renderer>().material.color = collide.gameObject.GetComponent<MovePulse>().creator.partnerLink.headRenderer.material.color;
				pOneTriggered = false;
				pTwoTriggered = true;
			}
		}
	}
}
