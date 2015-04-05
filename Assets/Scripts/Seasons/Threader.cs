using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Threader : MonoBehaviour {

	public bool activated;
	private Color myColor;
	public float r;
	private float g;
	private float b;
	private float a;
	//public GameObject Exit;
	//public GameObject Base;
	public float bondCount;
	public List<GameObject> bondLinks = new List<GameObject>();
	//public List<Bond> bonds = new List<Bond>();
	public Bond threadedbond = null;

	public GameObject smallripplePrefab;
	private GameObject smallrippleObj;

	public bool rippleShot;

		// Use this for initialization
	void Start () {

		rippleShot = false;
		activated = false;
		r = 0.5f;
		g = 0.5f;
		b = 0.5f;
		a = 1.0f;
		bondCount = 0; 



	}
	
	// Update is called once per frame
	void Update () {
		for(int i=0;i<bondLinks.Count;i++)
		{
			//Debug.Log(bondLinks[i]);
			if(bondLinks[i] == null)
			{
				bondLinks.RemoveAt(i);
				i--;
			}
		}


		bondCount = bondLinks.Count;

		if(bondCount == 0)
		{
			activated = false;
			threadedbond = null;
		}
		else if(bondCount > 0)
		{
			activated = true;
		}

		myColor = new Color(r,g,b,a);
		GetComponent<Renderer>().material.color = myColor;

		if(activated == true)
		{
			if(r<0.9)
			{
				r += Time.deltaTime;
				//print ("activated");
			}
			if(g<0.6)
			{
				r += Time.deltaTime;
				//print ("activated");
			}

		}
		else if(activated == false)
		{
			if(r>0.5)
			{
				r -= Time.deltaTime;
			}
			if(g>0.5)
			{
				r += Time.deltaTime;
				//print ("activated");
			}
		}
	}
	void OnTriggerEnter(Collider collide)
	{
		if(LayerMask.LayerToName(collide.gameObject.layer) == "Bond" )
		{
			Bond bond = collide.gameObject.GetComponentInParent<Bond>();
			BondAttachable player1 = Globals.Instance.player1.character.bondAttachable;
			BondAttachable player2 = Globals.Instance.player2.character.bondAttachable;
			if(bond != null && (bond.attachment1.attachee == player1 || bond.attachment2.attachee == player1) || (bond.attachment1.attachee == player2 || bond.attachment2.attachee == player2))
			{
				bondLinks.Add(collide.gameObject);
				threadedbond = bond;
			}
			//print(bondCount);

		}
		if(collide.gameObject.tag == "Fluff" && collide.gameObject.GetComponent<Fluff>().moving == true)
		{
		}
	}
	void OnTriggerExit(Collider collide)
	{
		if(LayerMask.LayerToName(collide.gameObject.layer) == "Bond" )
		{
			while(bondLinks.Contains(collide.gameObject))
			{
				bondLinks.Remove(collide.gameObject);
			}
			//print(bondCount)
		}
	}

	void MiniFire()
	{
		if(rippleShot == false)
		{
			smallrippleObj = Instantiate(smallripplePrefab,transform.position,Quaternion.identity) as GameObject;
			smallrippleObj.GetComponent<RingPulse>().scaleRate = 8.0f;
			smallrippleObj.GetComponent<RingPulse>().lifeTime = 1.5f;
			smallrippleObj.GetComponent<RingPulse>().alpha = 1.0f;
			smallrippleObj.GetComponent<RingPulse>().alphaFade = 0.7f;
			smallrippleObj.GetComponent<RingPulse>().mycolor = Color.white;
			//smallrippleObj.GetComponent<RingPulse>().smallRing = true;
			rippleShot = true;
		}
		//print ("fire!");
	}


}
