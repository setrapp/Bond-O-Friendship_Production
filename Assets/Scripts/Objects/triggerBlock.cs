using UnityEngine;
using System.Collections;

public class triggerBlock : MonoBehaviour {

	public bool triggered = false;
	public bool stopped = false;
	private Color myColor;
	private float triggerTime;
	public bool allDone = false;

	// Use this for initialization
	void Start () {
		myColor = new Color((192.0f/255.0f),(22.0f/255.0f),(233.0f/255.0f),1.0f);
		triggerTime = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if(triggered == true)
		{
			//GetComponent<Renderer>().material.color = Color.black;
			if(triggerTime > 0)
			triggerTime -= Time.deltaTime;
		}
		if(triggerTime <= 0)
		{
			triggered = false;
		}
		if(triggered == false)
		{
			GetComponent<Renderer>().material.color = myColor;
		}
		if(allDone == true)
		{
			GetComponent<Renderer>().material.color = Color.black;
		}
	}

	//void OnTriggerEnter(Collider collide)
	void AttachFluff(Fluff fluff)
	{
		if(fluff.gameObject.tag == "Fluff")
		{
			triggered = true;
			triggerTime = 0.3f;
			if (fluff.creator != null)
			{
				GetComponent<Renderer>().material.color = fluff.creator.attachmentColor;
			}
		}

		if(fluff.gameObject.tag != "Fluff" && fluff.gameObject.tag == "Character")
		{
			stopped = true;
		}
	}

	void OnTriggerStay(Collider collide)
	{
		//Debug.Log ("hi");
		if(collide.gameObject.tag != "Fluff" && collide.gameObject.tag == "Character")
		{
			stopped = true;
		}
	}

	void OnTriggerExit(Collider collide)
	{
		if(collide.gameObject.tag != "Fluff" && collide.gameObject.tag == "Character")
		{
			stopped = false;
		}
	}

	void UnTrigger()
	{
		triggered = false;
	}
	public void AllDone()
	{
		allDone = true;
	}
}
