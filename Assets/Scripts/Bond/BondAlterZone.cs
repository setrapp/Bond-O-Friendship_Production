using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BondAlterZone : MonoBehaviour {
	public bool followPlayerDepth = true;
	public BondStatsHolder bondOverrideStats;
	private List<BondAlterElement> attachablesInside;

	// To avoid colliders exiting the trigger by passing the boundary, ensure that expected colliders always hit the z-top or z-bottom of the trigger.
	// Zones prefer altering bond stats, so if only one of a bond's attachments is within the zone, the zone will alter the bond as if both were inside.

	void Start()
	{
		if (bondOverrideStats == null)
		{
			bondOverrideStats = GetComponent<BondStatsHolder>();
		}

		if (attachablesInside == null)
		{
			attachablesInside = new List<BondAlterElement>();
		}
	}

	void Update()
	{
		// If desired, maintain a depth that the players should hit.
		if (followPlayerDepth)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y, (Globals.Instance.player1.transform.position.z + Globals.Instance.player2.transform.position.z) / 2);
		}
	}

	private int FindAttachableIndex(BondAttachable attachable)
	{
		// Find the altered element that is handling the given attachable.
		int index = -1;
		if (attachable != null)
		{
			for (int i = 0; i < attachablesInside.Count && index < 0; i++)
			{
				if (attachablesInside[i].attachable == attachable)
				{
					index = i;
				}
			}
		}
		return index;
	}

	void OnTriggerEnter(Collider col)
	{
		if (bondOverrideStats == null)
		{
			return;
		}

		// Alter the entering attachable and its existing bonds to conform to the expected values.
		BondAttachable collideAttachable = col.gameObject.GetComponent<BondAttachable>();
		if (collideAttachable != null && FindAttachableIndex(collideAttachable) < 0)
		{
			BondAlterElement newElement = new BondAlterElement();
			newElement.attachable = collideAttachable;
			newElement.defaultStats = new BondStats();
			if (collideAttachable.bondOverrideStats != null)
			{
				newElement.defaultStats.Overwrite(collideAttachable.bondOverrideStats.stats);
				collideAttachable.bondOverrideStats.stats.Overwrite(bondOverrideStats.stats);
			}
			else
			{
				newElement.defaultStats.Overwrite(collideAttachable.bondPrefab.GetComponent<Bond>().stats);
			}

			for (int i = 0; i < collideAttachable.bonds.Count; i++)
			{
				collideAttachable.bonds[i].stats.Overwrite(bondOverrideStats.stats);
			}

			attachablesInside.Add(newElement);
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (bondOverrideStats == null)
		{
			return;
		}

		// Return the leaving attachable and its existing bonds to default settings.
		BondAttachable collideAttachable = col.gameObject.GetComponent<BondAttachable>();
		int index = FindAttachableIndex(collideAttachable);
		if (index >= 0)
		{
			if (collideAttachable.bondOverrideStats != null)
			{
				collideAttachable.bondOverrideStats.stats.Overwrite(attachablesInside[index].defaultStats);
			}

			for (int i = 0; i < collideAttachable.bonds.Count; i++)
			{
				// Only revert bonds when neither attachment is inside the alter zone.
				if (FindAttachableIndex(collideAttachable.bonds[i].OtherPartner(collideAttachable)) < 0)
				{
					collideAttachable.bonds[i].stats.Overwrite(attachablesInside[index].defaultStats);
				}
			}

			attachablesInside.RemoveAt(index);
		}
	}
}

[System.Serializable]
public class BondAlterElement
{
	public BondAttachable attachable;
	public BondStats defaultStats;
}
