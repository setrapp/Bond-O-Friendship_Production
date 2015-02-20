using UnityEngine;
using System.Collections;

public class ThreaderExit : MonoBehaviour {

	public bool activated;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider collide)
	{
		if(LayerMask.LayerToName(collide.gameObject.layer) == "Bond" )
		{
			activated = false;
			print("ExitFalse");
		}
	}

	void OnTriggerExit(Collider collide)
	{
		if(LayerMask.LayerToName(collide.gameObject.layer) == "Bond" )
		{
			activated = true;
			print("ExitTrue");
		}
	}
}
