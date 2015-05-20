using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThreadPad : MonoBehaviour {


	[SerializeField]
	public List<ThreadPadElement> myThreaders;
	[SerializeField]
	public List<GameObject> Posts;

	public float desiredbondLength;
	public float defaultbondlength;
	public bool wasThreading;
	private Bond playerBond = null;
	public bool solved;

	/*
	public GameObject element1;
	public GameObject element2;
	public GameObject element3;*/

	private Color postColor;
	private float alpha;
	private float blue;
	
	// Use this for initialization
	void Start () {
		solved = false;
		alpha = 0.6f;
		blue = 0.5f;
		
	}
	
	// Update is called once per frame
	void Update () {
		if (renderer != null)
		{
			postColor = new Color(0.5f, 0.5f, blue, alpha);
			renderer.material.color = postColor;
		}
		
		bool anyThreader = false;
		bool allThreaders = true;
		
		
		for(int i = 0;i < myThreaders.Count; i++)
		{
			//Debug.Log("hi");
			if(myThreaders[i].bondCount > 0)
			{
				//Debug.Log ("long");
				anyThreader = true;
				if(playerBond == null)
				{
					playerBond = myThreaders[i].threadedbond;
				}
			}
			else
			{
				allThreaders = false;
			}
		}
		
		if(anyThreader == true && !wasThreading)
		{
			if(playerBond != null)
			{
				defaultbondlength = playerBond.stats.maxDistance;
				playerBond.stats.maxDistance = desiredbondLength;
			}
		}
		else if(anyThreader == false && wasThreading)
		{
			if(playerBond != null)
			{
				playerBond.stats.maxDistance = defaultbondlength;
				playerBond = null;
			}
		}
		
		wasThreading = anyThreader;
		
		if(allThreaders == true)
		{
			//solved = true;
		}

		if(myThreaders[0].activated == true && myThreaders[1].activated == true && myThreaders[2].activated == true)
		{
			solved = true;
			myThreaders[0].transform.position = new Vector3(myThreaders[0].destination.transform.position.x, myThreaders[0].destination.transform.position.y, myThreaders[0].transform.position.z);
			if (myThreaders[0].body != null)
			{
				myThreaders[0].body.isKinematic = true;
			}
			myThreaders[1].transform.position = new Vector3(myThreaders[1].destination.transform.position.x, myThreaders[1].destination.transform.position.y, myThreaders[1].transform.position.z);
			if (myThreaders[1].body != null)
			{
				myThreaders[1].body.isKinematic = true;
			}
			myThreaders[2].transform.position = new Vector3(myThreaders[2].destination.transform.position.x, myThreaders[2].destination.transform.position.y, myThreaders[2].transform.position.z);
			if (myThreaders[2].body != null)
			{
				myThreaders[2].body.isKinematic = true;
			}
		}

		for(int i = 0;i < Posts.Count; i++)
		{
			Posts[i].renderer.material.color = postColor;
		}

		if(solved)
		{
			blue = 1.0f;
			alpha -= Time.deltaTime*1.0f;
		}

		if(alpha <= 0)
		{
			if (collider != null)
			{
				gameObject.collider.enabled = false;
			}
		}
		
	}
}
