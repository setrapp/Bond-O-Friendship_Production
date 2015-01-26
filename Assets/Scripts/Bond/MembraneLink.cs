using UnityEngine;
using System.Collections;

public class MembraneLink : BondLink {
	public Membrane membrane;
	public BondAttachable bondAttachable;
	public SpringJoint[] jointsShaping;

	void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.tag == "Character")
		{
			if (membrane != null && membrane.extraStats.bondOnContact)
			{
				AttempBond(collision.collider.GetComponent<BondAttachable>(), collision.contacts[0].point);
			}
		}
	}

	void AttachFluff(Fluff fluff)
	{
		if (fluff != null && fluff.moving && (fluff.attachee == null || fluff.attachee.gameObject == gameObject))
		{
			if (membrane != null && membrane.extraStats.bondOnFluff)
			{
				AttempBond(fluff.creator, fluff.transform.position);
			}
			fluff.PopFluff();
		}
	}

	private void AttempBond(BondAttachable partner, Vector3 contactPosition)
	{
		if (membrane != null && partner != null && (membrane.preferNewBonds || !membrane.IsBondMade(partner)))
		{
			// Use the attachable provided by the membrane, to allow endpoint links to be handled differently.
			BondAttachable linkAttachable = membrane.FindLinkAttachable(this);
			if (linkAttachable != null)
			{
				if (membrane.preferNewBonds)
				{
					membrane.BreakInnerBond(partner);
				}
				linkAttachable.AttemptBond(partner, contactPosition, true);
			}
		}
	}
}
