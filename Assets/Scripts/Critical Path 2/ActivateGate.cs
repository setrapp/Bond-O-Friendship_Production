using UnityEngine;
using System.Collections;

public class ActivateGate : MonoBehaviour {

	public GameObject gate;
	private int playersOnPad = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider col) {
		if (col.tag == "Character" && playersOnPad < 2)
						playersOnPad++;

		if (playersOnPad == 2) {
						gate.SetActive (false);
						GetComponent<Renderer> ().material.color = Color.black;
				}
		}
	void OnTriggerExit(Collider col) {
		if (col.tag == "Character" && playersOnPad > 0) {
						playersOnPad--;
						GetComponent<Renderer> ().material.color = Color.white;
				}

		gate.SetActive (true);
	}
}
