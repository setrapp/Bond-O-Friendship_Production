using UnityEngine;
using System.Collections;

public class PullApart : MonoBehaviour {

	private GameObject player1;
	private GameObject player2;

	// Use this for initialization
	void Start () {
		player1 = GameObject.Find("Player 1");
		player2 = GameObject.Find("Player 2");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
