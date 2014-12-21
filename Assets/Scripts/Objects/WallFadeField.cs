using UnityEngine;
using System.Collections;

public class WallFadeField : MonoBehaviour {

	public bool fadenow = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter(Collider collide)
	{
		if(collide.gameObject.tag == "Character")
		{
			//if(collide.gameObject.GetComponent<CharacterComponents>().isConnected == true)
		//	{
			//	fadenow = true;
			//}
		}
	}
}
