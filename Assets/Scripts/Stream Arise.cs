using UnityEngine;
using System.Collections;

public class StreamArise : MonoBehaviour {

	public GameObject myThreadPuzzle;
	private Vector3 myPos;
	public float startZ;
	public float endZ;
	private float myZ;


	// Use this for initialization
	void Start () {

		endZ = transform.position.z;
		myZ = startZ;

	}

	void FixedUpdate () {

		myPos = transform.position;
		My
		transform.position = myPos;

		if (myThreadPuzzle.GetComponent<ThreadParent> ().solved) 
		{

		}

	}

	// Update is called once per frame
	void Update () {
	
	}
}
