using UnityEngine;
using System.Collections;

public class JustMove : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		transform.Translate(Vector3.right*2.0f*Time.deltaTime);
	}
}
