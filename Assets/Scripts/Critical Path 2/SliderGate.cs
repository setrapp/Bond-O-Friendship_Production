using UnityEngine;
using System.Collections;

public class SliderGate : MonoBehaviour {
	
	public GameObject gate;
	private Color startColor;
	
	// Use this for initialization
	void Start () {
		startColor = GetComponent<Renderer> ().material.color;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider col) {
			gate.SetActive (false);
			GetComponent<Renderer> ().material.color = Color.black;
	}
	void OnTriggerExit(Collider col) {
		GetComponent<Renderer> ().material.color = startColor;
		
		gate.SetActive (true);
	}
}
