using UnityEngine;
using System.Collections;

public class ColorCredits : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider col) {
		GetComponent<Renderer>().material.color = new Color(0, 1, 0);
	}
}
