using UnityEngine;
using System.Collections;

public class MembraneLink : BondLink {
	public BondAttachable bondAttachable;
	public SpringJoint[] jointsShaping;

	void Update()
	{
		if (bondAttachable && bondAttachable.bonds.Count > 0)
		{
			for (int i = 0; i < bondAttachable.bonds.Count; i++)
			{
				Bond bond = bondAttachable.bonds[i];
				MembraneLink nearestLink;
				Membrane membrane = bond as Membrane;
				if (membrane != null)
				{
					Vector3 newPos = membrane.NearestNeighboredPoint(bond.links[1].transform.position, out nearestLink);
					bond.attachment1.position = newPos;
					if (nearestLink != this)
					{
						BondAttachable newAttachee = nearestLink.bondAttachable;
						if (nearestLink == nearestLink.bond.links[0])
						{
							newAttachee = ((Membrane)nearestLink.bond).attachment1FauxLink.bondAttachable;
						}
						else if (nearestLink == nearestLink.bond.links[nearestLink.bond.links.Count - 1])
						{
							newAttachee = ((Membrane)nearestLink.bond).attachment2FauxLink.bondAttachable;
						}
						bond.ReplacePartner(bondAttachable, newAttachee);
					}
				}
			}
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		BondAttachable bondAttachable = collision.collider.GetComponent<BondAttachable>();
		if (bondAttachable != null)
		{
			if (bond != null && ((Membrane)bond).extraStats.bondOnContact)
			{
				AttempBond(bondAttachable, collision.contacts[0].point);
			}
		}
	}

	void AttachFluff(Fluff fluff)
	{
		if (fluff != null && fluff.moving && (fluff.attachee == null || fluff.attachee.gameObject == gameObject))
		{
			if (bond != null && ((Membrane)bond).extraStats.bondOnFluff)
			{
				AttempBond(fluff.creator, fluff.transform.position);
			}
			fluff.PopFluff();
		}
	}

	private void AttempBond(BondAttachable partner, Vector3 contactPosition)
	{
		Membrane membrane = bond as Membrane;

		if (membrane != null && partner != null && (membrane.preferNewBonds || !membrane.IsBondMade(partner)))
		{
			int partnerLayer = (int)Mathf.Pow(2, partner.gameObject.layer);
			if ((partnerLayer & ((Membrane)bond).extraStats.ignoreBondingLayers) != partnerLayer)
			{
				// Use the attachable provided by the membrane, to allow endpoint links to be handled differently.
				BondAttachable linkAttachable = membrane.FindLinkAttachable(this);
				if (linkAttachable != null)
				{
					if (membrane.preferNewBonds)
					{
						membrane.BreakInnerBond(partner);
					}
					bool bonded = linkAttachable.AttemptBond(partner, ((Membrane)bond).NearestPoint(contactPosition), true);
					if (bonded)
					{
						membrane.forceFullDetail = true;
						bondAttachable.bonds[bondAttachable.bonds.Count - 1].stats.manualAttachment1 = true;
						if (membrane.transform.parent != null)
						{
							membrane.transform.parent.SendMessage("MembraneBonding", membrane, SendMessageOptions.DontRequireReceiver);
						}
					}
				}
			}
		}
	}

	private void BondBroken()
	{
		if (bond != null && !((Membrane)bond).IsBondMade())
		{
			bond.forceFullDetail = false;
		}
	}

	public override void RemoveJointToAttachment(){}
}
