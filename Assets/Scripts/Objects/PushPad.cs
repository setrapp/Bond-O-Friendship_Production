using UnityEngine;
using System.Collections;

public class PushPad : MonoBehaviour {
	private bool activated = false;
	public bool open = false;
	private float timer;
	private float red;
	private float newRed;
	private Color myColor;
	private Color postColor;
	public GameObject pad;
	public GameObject door;
	public GameObject track;
	public GameObject post1;
	public GameObject post2;
	//private float doorTimer;
	private Color doorColor;
	private float alpha;
	public GameObject activator;

	// Use this for initialization
	void Start () {
		timer = 1.0f;
		//doorTimer = 1.0f;
		red = 189.0f/255.0f;
		alpha = 0.5f;
	
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(red);
		//print (timer);
		myColor = new Color(red, 201.0f/255.0f,254.0f/255.0f,1.0f);
		doorColor = new Color(1.0f,1.0f,1.0f,alpha);
		postColor = new Color(1.0f,1.0f,1.0f,1.0f);
		pad.GetComponent<Renderer>().material.color = Color.white;
		door.GetComponent<Renderer>().material.color = doorColor;
        if(track != null)
		    track.GetComponent<Renderer>().material.color = doorColor;
		post1.GetComponent<Renderer>().material.color = postColor;
		post2.GetComponent<Renderer>().material.color = postColor;
		GetComponent<Renderer>().material.color = myColor;
		if (activated == true)
		{
			if(timer > 0)
			timer -= Time.deltaTime;
			if(red < 1.0f)
			{
				red += Time.deltaTime;
			}
		}
		if(activated == false)
		{
			timer = 1.0f;
			if(red > 0.4f)
			{
				//red -= Time.deltaTime * 2.0f;
			}
		}
		if (timer <= 0.0f)
		{
			newRed = red;
			myColor = new Color(newRed,0.5f,0.2f,1.0f);
			open = true;
			//print ("true");
		}
		if(open)
		{
			door.GetComponent<Collider>().enabled = false;
			if(alpha > 0)
			alpha -= Time.deltaTime*2.0f;
			//door.renderer.enabled = false;
			//pad.renderer.enabled = false;
		}
	
	}

	void OnTriggerEnter(Collider collide)
	{
		if (collide.gameObject == activator)
		{
			activated = true;
			//print ("on");
		}
	}
	void OnTriggerExit(Collider collide)
	{
		if (collide.gameObject == activator)
		{
			activated = false;
			//print ("off");
		}
	}
}
