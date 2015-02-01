using UnityEngine;
using System.Collections;

public class CanvasBehavior : MonoBehaviour {

	public GameObject player1;
	public GameObject player2;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider collide)
	{
		if(collide.gameObject.name == "Player 1")
		{
			player1 = collide.gameObject;
			player1.GetComponent<Paint>().painting = true;
			print ("Paint");
		}
		if(collide.gameObject.name == "Player 2")
		{
			player2 = collide.gameObject;
			player2.GetComponent<Paint>().painting = true;
			print ("Paint");
		}
	}

	void OnTriggerExit (Collider collide)
	{
		if(collide.gameObject.name == "Player 1")
		{
			player1 = collide.gameObject;
			player1.GetComponent<Paint>().painting = false;
			print ("Paintfalse");
		}
		if(collide.gameObject.name == "Player 2")
		{
			player2 = collide.gameObject;
			player2.GetComponent<Paint>().painting = false;
			print ("Paint");
		}
	}
}
