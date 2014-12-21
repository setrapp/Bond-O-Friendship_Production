using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BondAttachable : MonoBehaviour {
	public Rigidbody body;
	public bool handleFluffAttachment = true;
	public bool bondAtFluffPoint = true;
	public Color attachmentColor;
	public GameObject bondPrefab;
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
	}

	public void AttachFluff(Fluff fluff)
	{
		if (handleFluffAttachment && fluff != null)
		{
			AttemptBond(fluff.creator, fluff.transform.position);
		}
	}

	public Bond AttemptBond(BondAttachable connectionPartner, Vector3 contactPosition, bool forceBond = false)
	{
		Bond newBond = null;
		if (connectionPartner == null || connectionPartner == this)
		{
			return newBond;
		}

		if (connectionPartner.gameObject != gameObject)
		{
			volleys = 1;
			volleyPartner = connectionPartner;
			if (connectionPartner.volleyPartner == this)
			{
				volleys = connectionPartner.volleys + 1;
			}

			if (forceBond || volleys >= volleysToBond)
			{
				// If enough volleys have been passed, and the volleyers are not already connected, establish a new connection.
				if (!IsBondMade(connectionPartner))
				{
					Vector3 connectionPoint = transform.position;
					if (bondAtFluffPoint)
					{
						connectionPoint = contactPosition;
					}

					newBond = ((GameObject)Instantiate(bondPrefab, Vector3.zero, Quaternion.identity)).GetComponent<Bond>();
					bonds.Add(newBond);
					connectionPartner.bonds.Add(newBond);
					BondStatsHolder statsHolder = GetComponent<BondStatsHolder>();
					if (statsHolder != null && statsHolder.stats != null)
					{
						newBond.stats = statsHolder.stats;
					}

					// TODO this should be able to happen in reverse order (pulling attachments is buggy).
					//newBond.AttachPartners(this, connectionPoint, connectionPartner, connectionPartner.transform.position);
					newBond.AttachPartners(connectionPartner, connectionPartner.transform.position, this, connectionPoint);
					volleys = 0;
					connectionPartner.volleys = 0;
				}
			}
		}

		return newBond;
	}

	private void OnDestroy()
	{
		for (int i = 0; i < bonds.Count; )
		{
			bonds[i].BreakBond();
		}
	}

	public bool IsBondMade(BondAttachable partner)
	{
		if (partner == null)
		{
			return bonds.Count > 0;
		}

		bool connectionAlreadyMade = false;
		for (int i = 0; i < bonds.Count && !connectionAlreadyMade; i++)
		{
			if ((bonds[i].attachment1.attachee == this && bonds[i].attachment2.attachee == partner) || (bonds[i].attachment2.attachee == this && bonds[i].attachment1.attachee == partner))
			{
				connectionAlreadyMade = true;
			}
		}
		return connectionAlreadyMade;
	}
}
