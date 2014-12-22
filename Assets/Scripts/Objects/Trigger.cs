using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour {

	public bool pOneTriggered = false;
	public bool pTwoTriggered = false;
	public bool nullTriggered = false;
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

			if(pOneTriggered || pTwoTriggered || nullTriggered)
			{
				triggered = true;
				timeCount -= Time.deltaTime;
				myCenter.transform.localScale = new Vector3((timeCount/2)*centerScale.x,centerScale.y,(timeCount/2)*centerScale.z); 
			}
		}

		if(!pOneTriggered && !pTwoTriggered && !nullTriggered)
		{
			triggered = false;

		}

		if(timeCount <= 0)
		{
			pTwoTriggered = false;
			pOneTriggered = false;
			nullTriggered = false;
			GetComponent<Renderer>().material.color = myColor;
			myCenter.transform.localScale = centerScale;
			timeCount = 2.0f;

		}


	}
	void OnCollisionEnter(Collision collide)
	{
		if(collide.collider.gameObject.tag == "Fluff")
		{
			Fluff fluff = collide.collider.gameObject.GetComponent<Fluff>();
			if (fluff != null)
			{
				if(fluff.creator != null && fluff.creator.gameObject == Globals.Instance.player1.gameObject)
				{
					//print("Collide");
					GetComponent<Renderer>().material.color = collide.collider.gameObject.GetComponent<Fluff>().creator.attachmentColor;
					pOneTriggered = true;
					pTwoTriggered = false;
				}
				else if (fluff.creator != null && fluff.creator.gameObject == Globals.Instance.player2.gameObject)
				{
					//print("Collide");
					GetComponent<Renderer>().material.color = collide.collider.gameObject.GetComponent<Fluff>().creator.attachmentColor;
					pOneTriggered = false;
					pTwoTriggered = true;
				}
				else
				{
					nullTriggered = true;
				}
			}
			fluff.PopFluff();
		}
	}
}
