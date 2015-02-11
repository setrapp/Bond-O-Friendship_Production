using UnityEngine;
using System.Collections;

public class TripleTrigger : MonoBehaviour {

	public GameObject trigger1;
	public GameObject trigger2;
	public GameObject trigger3;
	private Color myColor;
	private float timer;
	private bool onetrig = false;
	private bool twotrig = false;
	private bool threetrig = false;
	public GameObject door;
	//private bool alltrig = false;
		
	// Use this for initialization
	void Start () {
		timer = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
		myColor = new Color(0.6f,0.6f,0.6f,timer);
		door.GetComponent<Renderer>().material.color = myColor;
		if(trigger1.GetComponent<Trigger>().pOneTriggered == true || trigger1.GetComponent<Trigger>().pTwoTriggered)
		{
			onetrig = true;
		}
		if(trigger2.GetComponent<Trigger>().pOneTriggered == true || trigger2.GetComponent<Trigger>().pTwoTriggered)
		{
			twotrig = true;
		}
		if(trigger3.GetComponent<Trigger>().pOneTriggered == true || trigger3.GetComponent<Trigger>().pTwoTriggered)
		{
			threetrig = true;
		}

		if(onetrig == true && twotrig == true && threetrig == true)
		{
			door.GetComponent<Collider>().enabled = false;
			timer -= Time.deltaTime/2;
			BroadcastMessage("FadeNow",SendMessageOptions.DontRequireReceiver);

		}

	}
}
