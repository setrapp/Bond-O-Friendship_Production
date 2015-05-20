using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThreadPadElement : MonoBehaviour {

	public bool activated;
	public float bondCount;
	public List<GameObject> bondLinks = new List<GameObject>();
	public Rigidbody body;
	public Bond threadedbond = null;
	public GameObject Activator;
	public GameObject destination;
	public GameObject triIsland;
	public GameObject triDest;
	public GameObject bigIsland;
	public GameObject bigDest;

	private Vector3 triPosition;
	private Vector3 bigPosition;
	private float maxDistance;
	private float maxTriDistance;
	private float maxBigDistance;

	// Use this for initialization
	void Start () {
		activated = false;
		bondCount = 0;
		maxDistance = Vector3.Distance(gameObject.transform.position, Activator.transform.position);
		maxTriDistance = Vector3.Distance(triIsland.transform.position, triDest.transform.position);
		maxBigDistance = Vector3.Distance(bigIsland.transform.position, bigDest.transform.position);

		if (body == null)
		{
			body = GetComponent<Rigidbody>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		//if(Vector3.Distance(gameObject.transform.position, Activator.transform.position) < 1.6f)
		//	GetComponent<SpringJoint>().spring = 0;

		triPosition = triIsland.transform.position;
		triPosition.y -= 1 - ((Vector3.Distance(gameObject.transform.position, Activator.transform.position)/maxDistance)*maxTriDistance)/Vector3.Distance(triIsland.transform.position, triDest.transform.position);
		triIsland.transform.position = triPosition;

		bigPosition = bigIsland.transform.position;
		bigPosition.y += 1 - ((Vector3.Distance(gameObject.transform.position, Activator.transform.position)/maxDistance)*maxBigDistance)/Vector3.Distance(bigIsland.transform.position, bigDest.transform.position);
		bigIsland.transform.position = bigPosition;

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
			activated = true;
	}

	void OnTriggerExit(Collider collide)
	{
		if(collide.gameObject == Activator)
			activated = false;
	}
}
