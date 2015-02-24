using UnityEngine;
using System.Collections;

public class Triborder : MonoBehaviour {

	public GameObject waitPad;
	public GameObject waitPad2;
	public GameObject waitPad3;
	private WaitPad pad;
	private WaitPad pad2;
	private WaitPad pad3;
	

	void Start () {
		if (waitPad != null)
			pad = waitPad.GetComponent<WaitPad>();
		if (waitPad2 != null)
			pad2 = waitPad2.GetComponent<WaitPad>();
		if (waitPad3 != null)
			pad3 = waitPad3.GetComponent<WaitPad>();
	}

	void Update () {
	
		if((pad != null && pad.activated) || (pad2 != null && pad2.activated) || (pad3 != null && pad3.activated))
		{
			gameObject.GetComponent<Collider>().enabled = false;
			pad = null;
			pad2 = null;
			pad3 = null;
		}
	}
}
