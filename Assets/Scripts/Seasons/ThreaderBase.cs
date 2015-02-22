using UnityEngine;
using System.Collections;

public class ThreaderBase : MonoBehaviour {

	public bool activated;
	public bool trigger;

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
			activated = true;
		}
		if(collide.gameObject.tag == "Fluff" && collide.gameObject.GetComponent<Fluff>().moving == true)
		{
		}
		
		
	}
}
