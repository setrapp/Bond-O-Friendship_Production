using UnityEngine;
using System.Collections;

public class MapMover : MonoBehaviour {
	
	private Vector3 p1StartPos;
	private Vector3 p2StartPos;
	private Vector3 selfStartPos;

	// Use this for initialization
	void Start () {
		p1StartPos = Globals.Instance.player1.gameObject.transform.position;
		p2StartPos = Globals.Instance.player2.gameObject.transform.position;
		selfStartPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(name == "MapMover1")
			transform.position = selfStartPos + (p1StartPos + Globals.Instance.player1.gameObject.transform.position)*0.05f;
		if(name == "MapMover2")
			transform.position = selfStartPos + (p2StartPos + Globals.Instance.player2.gameObject.transform.position)*0.05f;
	}
}
