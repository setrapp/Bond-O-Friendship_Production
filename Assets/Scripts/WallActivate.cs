using UnityEngine;
using System.Collections;

public class WallActivate : MonoBehaviour {
	public GameObject stageCrash;
	public GameObject wall1;
	public GameObject wall2;
	public GameObject wall3;
	public GameObject wall4;
	public GameObject wall5;
	public GameObject wall6;
	public GameObject wall7;
	// Use this for initialization
	void Start () {
		stageCrash = GameObject.Find("StageCrash");
	
	}
	
	// Update is called once per frame
	void Update () {
		if (stageCrash.GetComponent<StageCrash>().open == true)
		{
			if (wall1 != null)
			{
				wall1.GetComponent<Collider>().enabled = true;
			}
			if (wall2 != null)
			{
				wall2.GetComponent<Collider>().enabled = true;
			}
			if (wall3 != null)
			{
				wall3.GetComponent<Collider>().enabled = true;
			}
			if (wall4 != null)
			{
				wall4.GetComponent<Collider>().enabled = true;
			}
			if (wall5 != null)
			{
			    wall5.GetComponent<Collider>().enabled = true;
			}
			if (wall6 != null)
			{
				wall6.GetComponent<Collider>().enabled = true;
			}
			if (wall7 != null)
			{
				wall7.GetComponent<Collider>().enabled = true;
			}

			}

		}

}
