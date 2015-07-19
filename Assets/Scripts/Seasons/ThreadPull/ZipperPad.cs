using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZipperPad : MonoBehaviour {


	[SerializeField]
	public List<ZipperPadElement> myThreaders;
	[SerializeField]
	public List<ZipperResponder> responders; 

	public float desiredbondLength;
	public float defaultbondlength;
	public bool wasThreading;
	private Bond playerBond = null;
	public bool solved;

	void Start () {
		solved = false;

		// Ensure that responder destination is at the same z as the responding object.
		for (int i = 0; i < responders.Count; i++)
		{
			responders[i].startPosition = responders[i].responderObject.transform.position;
			responders[i].destination.transform.position = new Vector3(responders[i].destination.transform.position.x, responders[i].destination.transform.position.y, responders[i].responderObject.transform.position.z);
		}
	}

	void Update () {
		
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
			/*if (playerBond != null && desiredbondLength > playerBond.stats.maxDistance)
			{
				defaultbondlength = playerBond.stats.maxDistance;
				playerBond.stats.maxDistance = desiredbondLength;
			}*/
		}
		else if(anyThreader == false && wasThreading)
		{
			/*if (playerBond != null && desiredbondLength > playerBond.stats.maxDistance)
			{
				playerBond.stats.maxDistance = defaultbondlength;
				playerBond = null;
			}*/
		}
		
		wasThreading = anyThreader;

		if (!solved) {
			bool allSolved = true;
			for (int i = 0; i < myThreaders.Count && allSolved; i++)
			{
				if (!myThreaders[i].activated)
				{
					allSolved = false;
				}
			}

			if (allSolved)
			{
				//playerBond.stats.maxDistance = defaultbondlength;
				solved = true;
				Helper.FirePulse(transform.position, Globals.Instance.defaultPulseStats);

				for (int i = 0; i < myThreaders.Count; i++)
				{
					myThreaders[i].transform.position = myThreaders[i].destination.transform.position;
					if (myThreaders[i].body != null)
					{
						myThreaders[i].body.isKinematic = true;
					}
				}

				for (int i = 0; i < responders.Count; i++)
				{
					responders[i].responderObject.transform.position = responders[i].destination.transform.position;//new Vector3(responders[i].destination.transform.position.x, responders[i].destination.transform.position.y, responders[i].responderObject.transform.position.z);
				}
			}
			else
			{
				float progress = 0;
				for (int i = 0; i < myThreaders.Count; i++)
				{
					progress += myThreaders[i].progress / myThreaders.Count;
				}

				for (int i = 0; i < responders.Count; i++)
				{
					responders[i].responderObject.transform.position = (responders[i].startPosition * (1 - progress)) + (responders[i].destination.transform.position * progress);
				}
			}
		}

		/*if (solved && playerBond != null && playerBond.stats.maxDistance <= defaultbondlength)
		{
			playerBond.stats.maxDistance = defaultbondlength;
			defaultbondlength = -1;
		}*/
	}
}

[System.Serializable]
public class ZipperResponder
{
	public GameObject responderObject;
	public GameObject destination;
	[HideInInspector]
	public Vector3 startPosition;
}
