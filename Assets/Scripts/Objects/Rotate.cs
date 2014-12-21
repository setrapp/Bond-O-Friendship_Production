using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

	public GameObject waitPad;

	private float rotSpeed;

	// Use this for initialization
	void Start () {
		rotSpeed = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {

		if(waitPad.GetComponent<WaitPad>().activated == true)
		{
			rotSpeed = 2.0f;
		}

		transform.Rotate(Vector3.forward*Time.deltaTime*rotSpeed);

	}
}
