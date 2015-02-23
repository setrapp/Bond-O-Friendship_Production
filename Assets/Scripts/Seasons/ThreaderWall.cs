using UnityEngine;
using System.Collections;

public class ThreaderWall : MonoBehaviour {

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
		myColor = new Color(0.2f,0.0f,0.7f,alpha);
		GetComponent<Renderer>().material.color = myColor;
		
		if(trigger1.GetComponent<ThreadParent>().solved)
			if(trigger2 == null)
		{
			FadeNow ();
		}
		else if(trigger2 != null && trigger2.GetComponent<ThreadParent>().solved)
		{
			FadeNow ();
		}
		
		if(fadenow == true)
		{
			alpha -= Time.deltaTime/2;
			//alpha = timer;
		}
		if(alpha <= 0)
			Destroy(gameObject);
	}
	
	public void FadeNow()
	{
		fadenow = true;
	}
	
}
