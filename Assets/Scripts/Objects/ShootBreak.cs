using UnityEngine;
using System.Collections;

public class ShootBreak : MonoBehaviour {

	public GameObject player1; 
	public GameObject player2;
	//public GameObject pointer;
	//private Vector3 pointerPos;
	//private Vector3 pointerRot;
	private Transform p1Trans;
	private Transform p2Trans;
	private float pOneDist;
	private float pTwoDist;


	// Use this for initialization
	void Start () {
		player1 = GameObject.Find("Player 1");
		player2 = GameObject.Find("Player 2");
		p1Trans = player1.transform;
		p2Trans = player2.transform;
	}
	
	// Update is called once per frame
	void Update () {
		pOneDist = Vector3.Distance(transform.position, player1.transform.position);
		pTwoDist = Vector3.Distance(transform.position, player2.transform.position);

		if(pOneDist < 20.0f)
		{
			if(pOneDist < pTwoDist)
			transform.LookAt(p1Trans);
		}

		if(pTwoDist < 20.0f)
		{
			if(pTwoDist < pOneDist)
			transform.LookAt(p2Trans);
		}

		print(transform.rotation);

		print(pOneDist);

	

	}
}
