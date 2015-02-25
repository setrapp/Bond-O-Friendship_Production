using UnityEngine;
using System.Collections;

public class TriggerWall : MonoBehaviour {

	
	private Color myColor;
	private float alpha;
	private bool fadenow;
	public GameObject trigger1;
	public GameObject trigger2;
	
	// Use this for initialization
	void Start () {
		fadenow = false;
		alpha = 0.7f;
	}
	
	// Update is called once per frame
	void Update () {
		myColor = new Color(0.8f,0.8f,0.8f,alpha);
		GetComponent<Renderer>().material.color = myColor;

		if(trigger1.GetComponent<Trigger>().triggered)
			if(trigger2 == null)
		{
			FadeNow ();
		}
		else if( trigger2 != null && trigger2.GetComponent<Trigger>().triggered)
		{
			FadeNow ();
		}
		
		if(fadenow == true)
		{
			alpha -= Time.deltaTime/2;
			//alpha = timer;
		}
		if(alpha <= 0)
			GetComponent<Collider>().enabled = false;
	}
	
	public void FadeNow()
	{
		fadenow = true;
	}
	
}
