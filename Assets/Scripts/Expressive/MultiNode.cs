using UnityEngine;
using System.Collections;

public class MultiNode : MonoBehaviour {

	public GameObject MiniNode1;
	public GameObject MiniNode2;
	public GameObject MiniNode3;
	public GameObject MiniNode4;
	public GameObject MiniNode5;
	public GameObject MiniNode6;
	public GameObject MiniNode7;
	public GameObject MiniNode8;

	private Color endColor;
	public Color myColor;

	public bool allActivated;

	private float r;
	private float g;
	private float b;
	private float a;
	
	// Use this for initialization
	void Start () {
		myColor = new Color((191.0f/255.0f),(105.0f/255.0f),(255.0f/255.0f),1.0f);
		r = 0.25f;
		g = 0.0f;
		b = 0.75f;
		a = 1.0f;
		
		allActivated = false;
		
	}
	
	// Update is called once per frame
	void Update () {
		   endColor = new Color (r,g,b,a);
		
		if(MiniNode1.GetComponent<MiniNode>().activated &&
		   MiniNode2.GetComponent<MiniNode>().activated &&
		   MiniNode3.GetComponent<MiniNode>().activated &&
		   MiniNode4.GetComponent<MiniNode>().activated &&
		   MiniNode5.GetComponent<MiniNode>().activated &&
		   MiniNode6.GetComponent<MiniNode>().activated &&
		   MiniNode7.GetComponent<MiniNode>().activated &&
		   MiniNode8.GetComponent<MiniNode>().activated)
		{
			allActivated = true;
			//print ("All activated");
		}
		
		if(allActivated == true)
		{
			GetComponent<Renderer>().material.color = endColor;
			a -= Time.deltaTime;
			MiniNode1.GetComponent<MiniNode>().turnoff = true;
			MiniNode2.GetComponent<MiniNode>().turnoff = true;
			MiniNode3.GetComponent<MiniNode>().turnoff = true;
			MiniNode4.GetComponent<MiniNode>().turnoff = true;
			MiniNode5.GetComponent<MiniNode>().turnoff = true;
			MiniNode6.GetComponent<MiniNode>().turnoff = true;
			MiniNode7.GetComponent<MiniNode>().turnoff = true;
			MiniNode8.GetComponent<MiniNode>().turnoff = true;


		}
		
	}
}