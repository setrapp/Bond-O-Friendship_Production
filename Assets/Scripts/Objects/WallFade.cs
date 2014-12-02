using UnityEngine;
using System.Collections;

public class WallFade : MonoBehaviour {
	
	private Color myColor;
	private float alpha;
	private bool fadenow;

	// Use this for initialization
	void Start () {
		fadenow = false;
		alpha = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
		myColor = new Color(0.6f,0.6f,0.6f,alpha);
		GetComponent<Renderer>().material.color = myColor;

		if(fadenow == true)
		{
			alpha -= Time.deltaTime/2;
			//alpha = timer;
		}
	}

	public void FadeNow()
	{
		fadenow = true;
	}

}
