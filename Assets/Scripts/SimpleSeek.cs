using UnityEngine;
using System.Collections;

public class SimpleSeek : MonoBehaviour {
	public SimpleMover mover;
	public PartnerLink partnerLink;
	public Tracer tracer;
	protected Collider tailTrigger;

	protected virtual void Start()
	{
		if (mover == null)
		{
			mover = GetComponent<SimpleMover>();
		}
		if (tracer == null)
		{
			tracer = GetComponent<Tracer>();
		}
		if (partnerLink == null)
		{
			partnerLink = GetComponent<PartnerLink>();
		}
	}

	public virtual void SeekPartner()
	{
		if (partnerLink != null && partnerLink.partner != null)
		{
			Vector3 toPartner = partnerLink.partner.transform.position - transform.position;
			mover.Accelerate(toPartner);
		}
	}
}
