using UnityEngine;
using System.Collections;

public class PushPad : MonoBehaviour {
	private bool activated = false;
	public bool open = false;
	private float timer;
	private float red;
	private float newRed;
	private Color myColor;
	public GameObject pad;
	public GameObject door;
	private float doorTimer;
	private Color doorColor;
	private float alpha;

	// Use this for initialization
	void Start () {
		timer = 2.0f;
		doorTimer = 1.0f;
		red = 0.4f;
		alpha = 1.0f;
	
	}
	
	// Update is called once per frame
	void Update () {

		//print (timer);
		myColor = new Color(red, 0.1f, 0.3f,1.0f);
		doorColor = new Color(0.2f,0.2f,0.2f,alpha);
		pad.GetComponent<Renderer>().material.color = doorColor;
		door.GetComponent<Renderer>().material.color = doorColor;
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
			timer = 2.0f;
			if(red > 0.4f)
			{
				red -= Time.deltaTime * 2.0f;
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
			door.collider.enabled = false;
			if(alpha > 0)
			alpha -= Time.deltaTime*2.0f;
			//door.renderer.enabled = false;
			//pad.renderer.enabled = false;
		}
	
	}

	void OnTriggerEnter(Collider collide)
	{
		if(collide.gameObject.tag == "Pushable")
		{
			activated = true;
			//print ("on");
		}
	}
	void OnTriggerExit(Collider collide)
	{
		if(collide.gameObject.tag == "Pushable")
		{
			activated = false;
			//print ("off");
		}
	}
}
