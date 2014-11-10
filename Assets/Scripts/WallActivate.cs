using UnityEngine;
using System.Collections;

public class WallActivate : MonoBehaviour {
	public GameObject stageCrash;
	public GameObject wall1;
	public GameObject wall2;
	public GameObject wall3;
	public GameObject wall4;

	// Use this for initialization
	void Start () {
		stageCrash = GameObject.Find("StageCrash");
	
	}
	
	// Update is called once per frame
	void Update () {
		if (stageCrash.GetComponent<StageCrash>().open == true)
			{
				wall1.GetComponent<Collider>().enabled = true;
				wall2.GetComponent<Collider>().enabled = true;
				wall3.GetComponent<Collider>().enabled = true;
				wall4.GetComponent<Collider>().enabled = true;
			}

		}

}
