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
				wall1.collider.enabled = true;
				wall2.collider.enabled = true;
				wall3.collider.enabled = true;
				wall4.collider.enabled = true;
			}

		}

}
