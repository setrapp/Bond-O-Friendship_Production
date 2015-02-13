using UnityEngine;
using System.Collections;

public class FadeOut : MonoBehaviour {
	
	private float timer;
	private Color myColor;
	public bool fadeNow = false;

	// Use this for initialization
	void Start () {
		timer = 1.0f;
	
	}
	
	// Update is called once per frame
	void Update () {
		myColor = new Color(1.0f, 1.0f, 1.0f, timer);
		GetComponent<Renderer>().material.color = myColor;
		if(fadeNow == true)
		{
			timer -= Time.deltaTime;
		}
		if(timer <= 0)
		{
			Destroy(gameObject);
		}
	
	}

	void OnTriggerExit(Collider collide)
	{
		if(collide.gameObject.name == "EmptyTrigger")
		{
			//print ("Fade");
			fadeNow = true;
		}
	}
}

