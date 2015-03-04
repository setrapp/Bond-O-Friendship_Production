using UnityEngine;
using System.Collections;

public class DoubleNodeContainer : MonoBehaviour {

	public GameObject blueNode;
	public GameObject orangeNode;
	public bool bothActivated;

	// Use this for initialization
	void Start () {

		bothActivated = false;
	
	}
	
	// Update is called once per frame
	void Update () {

		if(blueNode.GetComponent<DoubleNode>().activated && orangeNode.GetComponent<DoubleNode>().activated)
		{
			bothActivated = true;
			//print ("both activated");
		}

		if(bothActivated == true)
		{
			blueNode.GetComponent<DoubleNode>().turnoff = true;
			orangeNode.GetComponent<DoubleNode>().turnoff = true;
		}
	
	}
}
