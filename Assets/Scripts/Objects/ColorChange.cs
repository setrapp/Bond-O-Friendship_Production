using UnityEngine;
using System.Collections;

public class ColorChange : MonoBehaviour {

	private Color startColor;
	private Color endColor;
	private bool changing = false;
	private float red = 0.358f;
	private float green = 0.390f;
	private float blue = 0.350f;
	//private float colorFloat = 1.0f;

	// Use this for initialization
	void Start () {
		endColor = new Color(red, green, blue, 1.0f);
	}
	
	// Update is called once per frame
	void Update () {
		if(changing == true)
		{
			if(red <= 0.358f)
				red += Time.deltaTime * 0.1f;
			if(green <= 0.390f)
				green += Time.deltaTime * 0.1f;
			if(blue <= 0.350f)
				blue += Time.deltaTime * 0.1f;
			endColor = new Color(red, green, blue, 1.0f);
			gameObject.GetComponent<Renderer>().material.color = endColor;
		}
		else
		{
			if(red >= 0.195f)
				red -= Time.deltaTime * 0.1f;
			if(green >= 0.221f)
				green -= Time.deltaTime * 0.1f;
			if(blue >= 0.188f)
				blue -= Time.deltaTime * 0.1f;
			endColor = new Color(red, green, blue, 1.0f);
			gameObject.GetComponent<Renderer>().material.color = endColor;
		}

	}

	void OnTriggerEnter(Collider col){
		if(col.transform.tag == "Character" || col.transform.tag == "Fluff")
			changing = true;
	}

	void OnTriggerExit(Collider col){
		if(col.transform.tag == "Character" || col.transform.tag == "Fluff")
		{
			changing = false;
		}
	}
}
