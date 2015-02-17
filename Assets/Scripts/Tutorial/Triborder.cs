using UnityEngine;
using System.Collections;

public class Triborder : MonoBehaviour {

	public GameObject waitPad;
	private WaitPad pad;
	

	void Start () {
		if (waitPad != null)
		{
			pad = waitPad.GetComponent<WaitPad>();
		}
	}

	void Update () {
	
		if(pad != null && pad.activated)
		{
			gameObject.GetComponent<Collider>().enabled = false;
			pad = null;
		}
	}
}
