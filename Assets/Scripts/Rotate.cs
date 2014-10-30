using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

	private float rotSpeed;

	// Use this for initialization
	void Start () {
		rotSpeed = 2.0f;
	}
	
	// Update is called once per frame
	void Update () {

		transform.Rotate(Vector3.forward*Time.deltaTime*rotSpeed);
	
	}
}
