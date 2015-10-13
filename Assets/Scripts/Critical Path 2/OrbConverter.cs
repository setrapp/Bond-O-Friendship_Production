using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrbConverter : MonoBehaviour {
	
	
	public GameObject orbPrefab;
	public GameObject orbShooter;

	public Vector3 orbLaunchForce;

	
	//Editor Testing.
	public bool fire = false;
	private bool lastFire = false;
	
	// Use this for initialization
	void Start () {
	}
	
	
	
	void Update()
	{
		if(fire != lastFire)
		{
			if(GetComponent<FluffStickRoot>().attachedSticks.Count == 2)
				ConvertToOrb();
		}
		
		lastFire = fire;
	}

	public void ConvertToOrb () {

		GameObject orbObject = (GameObject)Instantiate (orbPrefab);

		orbObject.transform.position = orbShooter.transform.position;

		orbObject.GetComponent<Rigidbody> ().AddForce (orbLaunchForce);
		}

	
}
