using UnityEngine;
using System.Collections;

public class RotateSimple : MonoBehaviour {

	public float rotSpeed;
	
	// Use this for initialization
	void Start () {
		//rotSpeed = 5.0f;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.forward*Time.deltaTime*rotSpeed);
	}
}
