using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BondAttachable : MonoBehaviour {
	public Rigidbody body;
	public bool handleFluffAttachment = true;
	public bool bondAtContactPoint = true;
	public Color attachmentColor;
	public GameObject bondPrefab;
	public BondStatsHolder bondOverrideStats;
	[SerializeField]
	public List<Bond> bonds;
	public int volleysToBond;
	public int volleys = 0;
	public BondAttachable volleyPartner;

	void Awake()
	{
		if (body == null)
		{
			body = GetComponent<Rigidbody>();
		}
		if (bondOverrideStats == null)
		{
			bondOverrideStats = GetComponent<BondStatsHolder>();
		}
	}

	public void AttachFluff(Fluff fluff)
	{
		if (handleFluffAttachment && fluff != null)
		{
			AttemptBond(fluff.creator, fluff.transform.position);
		}
	}

	public Bond AttemptBond(BondAttachable bondPartner, Vector3 contactPosition, bool forceBond = false)
	{
		if (bondOverrideStats.stats.maxDistance <= 0)
		{
			return null;
		}

		Bond newBond = null;
		if (bondPartner == null || bondPartner == this)
		{
			return newBond;
		}

		if (bondPartner.gameObject != gameObject)
		{
			volleys = 1;
			volleyPartner = bondPartner;
			if (bondPartner.volleyPartner == this)
			{
				volleys = bondPartner.volleys + 1;
			}

			if (forceBond || volleys >= volleysToBond)
			{
				// If enough volleys have been passed, and the volleyers are not already connected, establish a new bond.
				if (!IsBondMade(bondPartner))
				{
					Vector3 bondPoint = transform.position;
					if (bondAtContactPoint)
					{
						bondPoint = contactPosition;
					}

					newBond = ((GameObject)Instantiate(bondPrefab, Vector3.zero, Quaternion.identity)).GetComponent<Bond>();
					bonds.Add(newBond);
					bondPartner.bonds.Add(newBond);
					if (bondOverrideStats == null)
					{
						bondOverrideStats = GetComponent<BondStatsHolder>();
					}
					if (bondOverrideStats != null && bondOverrideStats.stats != null)
					{
						newBond.stats.Overwrite(bondOverrideStats.stats);
					}

					newBond.AttachPartners(this, bondPoint, bondPartner, bondPartner.transform.position);
					volleys = 0;
					bondPartner.volleys = 0;
				}
			}
		}

		return newBond;
	}

	private void OnDestroy()
	{
		for (int i = 0; i < bonds.Count; )
		{
			bonds[i].BreakBond(true);
		}
	}

	public bool IsBondMade(BondAttachable partner = null)
	{
		if (partner == null)
		{
			return bonds.Count > 0;
		}

		bool bondAlreadyMade = false;
		for (int i = 0; i < bonds.Count && !bondAlreadyMade; i++)
		{
			if ((bonds[i].attachment1.attachee == this && bonds[i].attachment2.attachee == partner) || (bonds[i].attachment2.attachee == this && bonds[i].attachment1.attachee == partner))
			{
				bondAlreadyMade = true;
			}
		}
		return bondAlreadyMade;
	}

	public void BreakBond(BondAttachable partner)
	{
		for (int i = 0; i < bonds.Count; i++)
		{
			if (partner == null || (bonds[i].attachment1.attachee == this && bonds[i].attachment2.attachee == partner) || (bonds[i].attachment2.attachee == this && bonds[i].attachment1.attachee == partner))
			{
				bonds[i].BreakBond();
			}
		}
	}

	public void RequestFluff(Bond bondRequesting)
	{
		SendMessage("SendFluffToBond", bondRequesting, SendMessageOptions.DontRequireReceiver);
	}
}
