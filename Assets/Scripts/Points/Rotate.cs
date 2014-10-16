using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {


	public float rotSpeed;
	private Vector3 rotDir;

	// Use this for initialization
	void Start () {
	
		rotSpeed = 6;
		rotDir = new Vector3(0,0,-1);

	}
	
	// Update is called once per frame
	void Update () {

		transform.Rotate(rotDir * rotSpeed * Time.deltaTime);
	
	}
}
