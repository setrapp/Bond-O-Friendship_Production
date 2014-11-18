using UnityEngine;
using System.Collections;

public class CubePushBack : MonoBehaviour {


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision col){
		//Debug.Log (col.gameObject.name);
		if(col.gameObject.name == "Simple Connection(Clone)")
		{
			//col.gameObject.GetComponent<SimpleConnection>().attachment1.partner.GetComponent<SimpleMover>().externalSpeedMultiplier = 0.80f;
			//col.gameObject.GetComponent<SimpleConnection>().attachment2.partner.GetComponent<SimpleMover>().externalSpeedMultiplier = 0.80f;
		}
	}

	void OnCollisionExit(Collision col){
		if(col.gameObject.name == "Simple Connection(Clone)")
		{
			//col.gameObject.GetComponent<SimpleConnection>().attachment1.partner.GetComponent<SimpleMover>().externalSpeedMultiplier = 1.0f;
			//col.gameObject.GetComponent<SimpleConnection>().attachment2.partner.GetComponent<SimpleMover>().externalSpeedMultiplier = 1.0f;
		}
	}
}
