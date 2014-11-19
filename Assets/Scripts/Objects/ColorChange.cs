using UnityEngine;
using System.Collections;

public class ColorChange : MonoBehaviour {

	private Color startColor;
	private Color endColor;
	private float coolDown = 1.0f;
	private bool changing = false;

	// Use this for initialization
	void Start () {
		startColor = gameObject.GetComponent<Renderer>().material.color;
		endColor = new Color(0.9f, 0.9f, 0.9f);
	}
	
	// Update is called once per frame
	void Update () {
		if(changing == true)
			gameObject.GetComponent<Renderer>().material.color = Color.Lerp(startColor, endColor, 2.0f*Time.deltaTime);
		else if(gameObject.GetComponent<Renderer>().material.color != startColor)
			gameObject.GetComponent<Renderer>().material.color = Color.Lerp(endColor, startColor, 2.0f*Time.deltaTime);

	}

	void OnCollisionEnter(Collision col){
		if(col.transform.tag == "Converser" || col.transform.tag == "Pulse")
			changing = true;
	}

	void OnCollisionExit(Collision col){
		if(col.transform.tag == "Converser" || col.transform.tag == "Pulse")
		{
			changing = false;
		}
	}
}
