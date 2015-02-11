using UnityEngine;
using System.Collections;

public class Expand : MonoBehaviour {

	public float xSpeed;
	private Vector3 xVector;
	private float x;
	private float y;
	private float z;

	// Use this for initialization
	void Start () {
		xSpeed = 500.0f;
		x = 1.0f;
		y = 0.001f;
		z = 1.0f;


	}
	
	// Update is called once per frame
	void Update () {
		xVector = new Vector3(x * xSpeed * Time.deltaTime, y, z * xSpeed * Time.deltaTime);
		transform.localScale = xVector;

	}
}
