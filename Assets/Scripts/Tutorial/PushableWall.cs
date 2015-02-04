using UnityEngine;
using System.Collections;

public class PushableWall : MonoBehaviour {

	
	private Color myColor;
	private float alpha;
	private bool fadenow;
	public GameObject trigger1;
	//public GameObject trigger2;
	
	// Use this for initialization
	void Start () {
		fadenow = false;
		alpha = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
		myColor = new Color(0.6f,0.6f,0.6f,alpha);
		GetComponent<Renderer>().material.color = myColor;
		
		if(trigger1.GetComponent<LittleEmptyTrigger>().triggered)
		{
			FadeNow ();
		}
		
		if (fadenow == true)
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
