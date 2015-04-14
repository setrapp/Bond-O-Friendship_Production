using UnityEngine;
using System.Collections;

public class ResetSpinPad : MonoBehaviour {

	private SpinPad spinPad;

	// Use this for initialization
	void Start () {
		spinPad = GetComponent<SpinPad>();
	}
	
	// Update is called once per frame
	void Update () {
		//if(spinPad.portionComplete >= 1.0f)
		//	StartCoroutine(spinPad.ResetToStart());
	}
}
