using UnityEngine;
using System.Collections;

public class TriangleFade : MonoBehaviour {

	private float timer;
	private Color myColor;
	public GameObject waitPad;
	
	// Use this for initialization
	void Start () {
		timer = 0.7f;
		
	}
	
	// Update is called once per frame
	void Update () {
		myColor = new Color(0.6f, 0.5f, 0.7f, timer);
		GetComponent<Renderer>().material.color = myColor;
		if(waitPad.GetComponent<WaitPad>().activated)
		{
			timer -= Time.deltaTime;
		}
		if(timer <= 0)
		{
			Destroy(gameObject);
		}
		
	}

}
