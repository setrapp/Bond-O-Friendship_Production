using UnityEngine;
using System.Collections;

public class Triborder : MonoBehaviour {

	public GameObject waitPad;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if(waitPad.GetComponent<WaitPad>().activated)
		{
			gameObject.GetComponent<Collider>().enabled = false;
		}
	}
}
