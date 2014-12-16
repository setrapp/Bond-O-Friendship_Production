using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour {

	public bool pOneTriggered = false;
	public bool pTwoTriggered = false;
	public bool triggered = false;
	private Color myColor;
	public GameObject myCenter;
	private Vector3 centerScale;
	public bool timedTrigger = false;
	public float timeCount;

	// Use this for initialization
	void Start () {
		myColor = new Color(0.8f,0.7f,0.5f,1.0f);
		GetComponent<Renderer>().material.color = myColor;
		timeCount = 2.0f;
		centerScale = myCenter.transform.localScale;

	}
	
	// Update is called once per frame
	void Update () {

		if(timedTrigger)
		{

			if(pOneTriggered || pTwoTriggered)
			{
				timeCount -= Time.deltaTime;
				myCenter.transform.localScale = new Vector3((timeCount/2)*centerScale.x,centerScale.y,(timeCount/2)*centerScale.z); 
			}
		}

		if(pOneTriggered || pTwoTriggered)
		{
			triggered = true;
		}
		else if(!pOneTriggered && !pTwoTriggered)
		{
			triggered = false;

		}

		if(timeCount <= 0)
		{
			pTwoTriggered = false;
			pOneTriggered = false;
			GetComponent<Renderer>().material.color = myColor;
			myCenter.transform.localScale = centerScale;
			timeCount = 2.0f;

		}


	}
	void OnCollisionEnter(Collision collide)
	{
		//Debug.Log(collide.collider.GetComponent<MovePulse>().creator.name);
		if(collide.collider.gameObject.tag == "Pulse")
		{
			MovePulse mover = collide.collider.gameObject.GetComponent<MovePulse>();
			if(mover.creator != null && mover.creator.name == "Player 1")
			{
				//print("Collide");
				GetComponent<Renderer>().material.color = collide.collider.gameObject.GetComponent<MovePulse>().creator.attachmentColor;
				pOneTriggered = true;
				pTwoTriggered = false;
			}
			if(mover.creator != null && mover.creator.name == "Player 2")
			{
				//print("Collide");
				GetComponent<Renderer>().material.color = collide.collider.gameObject.GetComponent<MovePulse>().creator.attachmentColor;
				pOneTriggered = false;
				pTwoTriggered = true;
			}
		}
	}
}
