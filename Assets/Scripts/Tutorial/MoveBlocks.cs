using UnityEngine;
using System.Collections;

public class MoveBlocks : MonoBehaviour {

	public SpinPad spinPad;
	public float moveDistance;
	private float startX;

	// Use this for initialization
	void Start () {
		startX = transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		if(name == "Door Key Right")
			transform.position = new Vector3(startX + spinPad.portionComplete * moveDistance, transform.position.y, 1.0f);
		if(name == "Door Key Left")
			transform.position = new Vector3(startX + -spinPad.portionComplete * moveDistance, transform.position.y, 1.0f);
	}
}
