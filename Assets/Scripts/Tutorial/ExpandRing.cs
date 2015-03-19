using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExpandRing : MonoBehaviour {

	public SpinPad spinPad;
	public float maxSeparationDistance;
	private float startX;
	private float startY;
	[SerializeField]
	public List<Collider> ignoreCollisions;

	// Use this for initialization
	void Start () {
		startX = transform.position.x;
		startY = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(spinPad.portionComplete);
		if(name == "QuarterRing1")
			transform.position = new Vector3(startX + -spinPad.portionComplete * maxSeparationDistance, startY + spinPad.portionComplete * maxSeparationDistance, 0);
		if(name == "QuarterRing2")
			transform.position = new Vector3(startX +-spinPad.portionComplete * maxSeparationDistance, startY + -spinPad.portionComplete * maxSeparationDistance, 0);
		if(name == "QuarterRing3")
			transform.position = new Vector3(startX +spinPad.portionComplete * maxSeparationDistance, startY + -spinPad.portionComplete * maxSeparationDistance, 0);
		if(name == "QuarterRing4")
			transform.position = new Vector3(startX +spinPad.portionComplete * maxSeparationDistance, startY + spinPad.portionComplete * maxSeparationDistance, 0);
	}

	/*void OnCollisionEnter(Collision col)
	{
		if (!ignoreCollisions.Contains(col.collider))
		{
			spinPad.spinInhibitors++;
		}
	}

	void OnCollisionExit(Collision col)
	{
		if (!ignoreCollisions.Contains(col.collider))
		{
			spinPad.spinInhibitors--;
		}
	}*/
}
