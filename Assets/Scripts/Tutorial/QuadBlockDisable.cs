using UnityEngine;
using System.Collections;

public class QuadBlockDisable : MonoBehaviour {

	public GameObject qBlock1;
	public GameObject qBlock2;
	public GameObject qBlock3;
	public GameObject qBlock4;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider collide)
	{
		//print("Hit");
		if(collide.gameObject.name == "Player 1" || collide.gameObject.name == "Player 2")

		{
			qBlock1.GetComponent<Collider>().enabled = false;
			qBlock2.GetComponent<Collider>().enabled = false;
			qBlock3.GetComponent<Collider>().enabled = false;
			qBlock4.GetComponent<Collider>().enabled = false;

		}
	}
}
