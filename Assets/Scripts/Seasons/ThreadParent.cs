using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ThreadParent : MonoBehaviour {

	[SerializeField]
	public List<Threader> myThreaders;
	public float desiredbondLength;
	private float defaultbondlength;
	public bool wasThreading;
	private Bond playerBond = null;
	public float bondExtensionPerFluff = -1;
	public float minBondFluffCount = -1;
	public bool solved;
	private int landsFull = 0;
	public List<GameObject> landCompleteActivatees;
	// Use this for initialization
	void Start () {
		solved = false;

	}
	
	// Update is called once per frame
	void Update () {
	
		bool anyThreader = false;
		bool allThreaders = true;


		for(int i = 0;i < myThreaders.Count; i++)
		{
			if(myThreaders[i].bondCount > 0)
			{
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
			if (playerBond != null && bondExtensionPerFluff >= 0)
			{
				playerBond.stats.extensionPerFluff = bondExtensionPerFluff;
			}
			/*if (playerBond != null && desiredbondLength > playerBond.stats.maxDistance)
			{
				defaultbondlength = playerBond.stats.maxDistance;
				playerBond.stats.maxDistance = desiredbondLength;
			}*/
		}
		else if(anyThreader == false && wasThreading)
		{
			if (playerBond != null)
			{
				if (bondExtensionPerFluff >= 0)
				{
					playerBond.stats.extensionPerFluff = playerBond.attachment1.attachee.bondOverrideStats.stats.extensionPerFluff;
				}

				if (playerBond.fluffsHeld.Count < minBondFluffCount)
				{

				}
			}
			/*if (playerBond != null && desiredbondLength > playerBond.stats.maxDistance)
			{
				playerBond.stats.maxDistance = defaultbondlength;
				playerBond = null;
			}*/
		}

		if (anyThreader && playerBond != null && playerBond.fluffsHeld.Count < minBondFluffCount)
		{
			playerBond.stats.maxDistance = playerBond.attachment1.attachee.bondOverrideStats.stats.maxDistance + (minBondFluffCount * playerBond.stats.extensionPerFluff);
		}

		wasThreading = anyThreader;

		if(allThreaders == true)
		{
			//playerBond.stats.maxDistance = defaultbondlength;
			solved = true;
			BroadcastMessage("MiniFire", SendMessageOptions.DontRequireReceiver);
		}
	
		/*if (solved && playerBond != null && playerBond.stats.maxDistance <= defaultbondlength)
		{
			playerBond.stats.maxDistance = defaultbondlength;
			defaultbondlength = -1;
		}*/
	
	}

	public void LandFull()
	{
		// React to all spawned land being full.
		landsFull++;
		if (landsFull == myThreaders.Count)
		{
			for (int i = 0; i < landCompleteActivatees.Count; i++)
			{
				landCompleteActivatees[i].gameObject.SetActive(true);
			}
		}
	}
}
