using UnityEngine;
using System.Collections;

public class MembraneLink : BondLink {
	public Membrane membrane;
	public BondAttachable bondAttachable;
	public SpringJoint jointShaping;

	void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.tag == "Character")
		{
			BondAttachable partner = collision.collider.GetComponent<BondAttachable>();
			if (bondAttachable != null && partner != null && (membrane.preferNewBonds || !membrane.IsBondMade(partner)))
			{
				if (membrane.preferNewBonds)
				{
					membrane.BreakInnerBond(partner);
				}
				bondAttachable.AttemptBond(partner, transform.position, true);
			}
		}
	}

	void AttachFluff(Fluff fluff)
	{
		if (fluff != null && fluff.moving && (fluff.attachee == null || fluff.attachee.gameObject == gameObject))
		{
			if (!membrane.IsBondMade(fluff.creator))
			{
				bondAttachable.AttemptBond(fluff.creator, transform.position, true);
			}
			fluff.PopFluff();
		}
	}
}
