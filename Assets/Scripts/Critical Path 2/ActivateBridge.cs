using UnityEngine;
using System.Collections;

public class ActivateBridge : MonoBehaviour {

	public GameObject bridge;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter (Collider col) {
		if (col.tag == "Character")
						bridge.SetActive (true);
		GetComponent<Renderer> ().material.color = Color.black;
	}
}
