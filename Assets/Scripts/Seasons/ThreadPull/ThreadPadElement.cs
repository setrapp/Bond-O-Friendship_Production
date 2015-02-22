using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThreadPadElement : MonoBehaviour {

	public bool activated;
	public float bondCount;
	public List<GameObject> bondLinks = new List<GameObject>();
	public Bond threadedbond = null;
	public GameObject Activator;

	// Use this for initialization
	void Start () {
		activated = false;
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
			//activated = false;
			threadedbond = null;
		}
		else if(bondCount > 0)
		{
			//activated = true;
		}
	}

	void OnCollisionEnter(Collision collide)
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

	}
	void OnCollisionExit(Collision collide)
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
	void OnTriggerEnter(Collider collide)
	{
		if(collide.gameObject == Activator)
		{
			activated = true;
			//print ("Activated");
		}
	}

	void OnTriggerExit(Collider collide)
	{
		if(collide.gameObject == Activator)
		{
			activated = false;
			//print ("DeActivated");
		}
	}
}
