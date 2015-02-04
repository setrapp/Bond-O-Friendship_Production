using UnityEngine;
using System.Collections;

public class LittleEmptyTrigger : MonoBehaviour {

	private GameObject pushable;
	public bool triggered = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerExit (Collider collide)
	{
		if (collide.gameObject.tag == "Pushable")
		{
			//pushable = collide.gameObject;
			//pushable.GetComponent<FadeOut>().fadeNow = true;
			triggered = true;
		}
	}

}
