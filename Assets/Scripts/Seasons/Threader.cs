using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Threader : MonoBehaviour {

	public Renderer threaderRenderer;
	public bool activated;
	private bool wasActivated;
	private Color myColor;
	public float r;
	private float g;
	private float b;
	private float a;
	//public GameObject Exit;
	//public GameObject Base;
	public float bondCount;
	public List<GameObject> bondLinks = new List<GameObject>();
	public List<GameObject> players = new List<GameObject>();
	//public List<Bond> bonds = new List<Bond>();
	public Bond threadedbond = null;
	public bool waitingOnFull = false;

	public GameObject smallripplePrefab;
	private GameObject smallrippleObj;

	private ThreadParent threadParent;

	public bool rippleShot;
	private float rotateSpeed = 0;
	public float nonActiveRotateSpeed = 25;
	public float activeRotateSpeed = 60;

	// Use this for initialization
	void Start () {
		wasActivated = activated;

		if (threaderRenderer == null)
		{
			threaderRenderer = GetComponent<Renderer>();
		}

		rippleShot = false;
		activated = false;
		r = 0.8f;
		g = 0.8f;
		b = 0.8f;
		a = 1.0f;
		bondCount = 0; 

		threadParent = transform.parent.GetComponent<ThreadParent>();

		rotateSpeed = nonActiveRotateSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		if (waitingOnFull && threadParent != null && threadParent.solved)
		{
			LandFull();
		}

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

		if(bondCount <= 0 && players.Count <= 0)
		{
			activated = false;
			threadedbond = null;
			if (wasActivated)
			{
				SendMessage("UnplayNote", SendMessageOptions.DontRequireReceiver);
			}
		}
		else
		{
			activated = true;
			if (!wasActivated)
			{
				SendMessage("PlayNote", SendMessageOptions.DontRequireReceiver);
			}
		}

		if (activated)
		{
			rotateSpeed = Mathf.Min(rotateSpeed + Time.deltaTime * (activeRotateSpeed - nonActiveRotateSpeed), activeRotateSpeed);
		}
		else
		{
			rotateSpeed = Mathf.Max(rotateSpeed - Time.deltaTime * (activeRotateSpeed - nonActiveRotateSpeed), nonActiveRotateSpeed);
		}

		transform.Rotate (0, 0, rotateSpeed * Time.deltaTime);

		wasActivated = activated;
	}
	void OnTriggerEnter(Collider collide)
	{
		Bond bond = null;
		if (LayerMask.LayerToName(collide.gameObject.layer) == "Character")
		{
			if (!players.Contains(collide.gameObject))
			{
				players.Add(collide.gameObject);
			}
			BondAttachable attachable = collide.GetComponent<BondAttachable>();
			if (attachable != null)
			{
				for (int i = 0; i < attachable.bonds.Count && bond == null; i++)
				{
					if (attachable.bonds[i].OtherPartner(attachable) == Globals.Instance.Player1.character.bondAttachable || attachable.bonds[i].OtherPartner(attachable) == Globals.Instance.Player2.character.bondAttachable)
					{
						bond = attachable.bonds[i];
					}
				}
			}
		}

		if(LayerMask.LayerToName(collide.gameObject.layer) == "Bond")
		{
			bond = collide.gameObject.GetComponentInParent<Bond>();
		}

		if (bond != null) 
		{
			BondAttachable player1 = Globals.Instance.Player1.character.bondAttachable;
			BondAttachable player2 = Globals.Instance.Player2.character.bondAttachable;
			if(bond != null && (bond.attachment1.attachee == player1 || bond.attachment2.attachee == player1) || (bond.attachment1.attachee == player2 || bond.attachment2.attachee == player2))
			{
				if (LayerMask.LayerToName(collide.gameObject.layer) == "Bond")
				{
					bondLinks.Add(collide.gameObject);
				}
				threadedbond = bond;
			}
			//print(bondCount);

		}
	}
	void OnTriggerExit(Collider collide)
	{
		if (players.Contains(collide.gameObject))
		{
			players.Remove(collide.gameObject);
		}

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
			//smallrippleObj.GetComponent<RingPulse>().alphaFade = 0.7f;
			smallrippleObj.GetComponent<RingPulse>().mycolor = Color.white;
			//smallrippleObj.GetComponent<RingPulse>().smallRing = true;
			rippleShot = true;
		}
		//print ("fire!");
	}

	void LandFull()
	{
		waitingOnFull = true;
		if (threadParent.solved)
		{
			waitingOnFull = false;
			threadParent.LandFull();
		}
	}

	void LandNotFull()
	{
		waitingOnFull = false;
	}
}
