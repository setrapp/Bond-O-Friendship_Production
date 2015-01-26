using UnityEngine;
using System.Collections;

public class triggerBlock : MonoBehaviour {

	public bool triggered = false;
	private Color myColor;
	private float triggerTime;
	public bool allDone = false;

	// Use this for initialization
	void Start () {
		GetComponent<Renderer>().material.color = Color.cyan;
		myColor = GetComponent<Renderer>().material.color;
		triggerTime = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if(triggered == true)
		{
			GetComponent<Renderer>().material.color = Color.black;
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

	void OnTriggerEnter(Collider collide)
	{
		if(collide.gameObject.tag == "Fluff")
		{
			triggered = true;
			triggerTime = 0.3f;
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
