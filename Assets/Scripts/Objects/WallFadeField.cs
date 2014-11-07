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
		if(collide.gameObject.tag == "Converser")
		{
			//if(collide.gameObject.GetComponent<PartnerLink>().isConnected == true)
		//	{
			//	fadenow = true;
			//}
		}
	}
}
