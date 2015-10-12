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
			if(myThreaders[i].bondCount > 0 || myThreaders[i].players.Count > 0)
			{
				anyThreader = true;
				if(playerBond == null && myThreaders[i].threadedbond != null)
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
			// Extend bond based on the altered per fluff length of the threader.
			if (playerBond != null && bondExtensionPerFluff >= 0)
			{
				playerBond.stats.extensionPerFluff = bondExtensionPerFluff;
				playerBond.stats.maxDistance = Globals.Instance.Player1.character.bondAttachable.bondOverrideStats.stats.maxDistance + (playerBond.fluffsHeld.Count * playerBond.stats.extensionPerFluff);
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
				ReturnBond(playerBond);
			}
			/*if (playerBond != null && desiredbondLength > playerBond.stats.maxDistance)
			{
				playerBond.stats.maxDistance = defaultbondlength;
				playerBond = null;
			}*/
		}

		// If the bond is holding fewer fluffs than the minimum, fake the bond holding exactly the minimum (this allows control during wet season).
		if (anyThreader && playerBond != null && playerBond.fluffsHeld.Count < minBondFluffCount)
		{
			playerBond.stats.maxDistance = playerBond.attachment1.attachee.bondOverrideStats.stats.maxDistance + (minBondFluffCount * playerBond.stats.extensionPerFluff);
		}

		wasThreading = anyThreader;

		if(allThreaders == true)
		{
			//playerBond.stats.maxDistance = defaultbondlength;
			solved = true;
			ReturnBond(playerBond);
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

	private void ReturnBond(Bond playerBond)
	{
		if (playerBond == null)
		{
			return;
		}
		// Return bond extention per fluff to normal size when leaving threader.
		playerBond.stats.extensionPerFluff = playerBond.attachment1.attachee.bondOverrideStats.stats.extensionPerFluff;
		playerBond.stats.maxDistance = Globals.Instance.Player1.character.bondAttachable.bondOverrideStats.stats.maxDistance + (playerBond.fluffsHeld.Count * playerBond.stats.extensionPerFluff);
		Globals.Instance.Player1.GetComponent<SeasonPlayerReaction>().BondSeasonReact(playerBond);
	}
}
