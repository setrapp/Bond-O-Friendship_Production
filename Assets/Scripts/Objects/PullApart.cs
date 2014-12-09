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
		if(player1.GetComponent<PartnerLink>().absorbing == true && player2.GetComponent<PartnerLink>().absorbing == true)
		{
	//		if(Vector3.Distance(player1.transform.position, transform.position) < 7.0f 
		}
	}
}
