using UnityEngine;
using System.Collections;

public class SimpleSeek : MonoBehaviour {
	public SimpleMover mover;
	public PartnerLink partnerLink;
	public Tracer tracer;
	public Tail tail;
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
		if (tail != null)
		{
			tailTrigger = tail.trigger;
		}
	}

	public virtual void SeekPartner()
	{
		if (partnerLink != null && partnerLink.Partner != null)
		{
			Vector3 toPartner = partnerLink.Partner.transform.position - transform.position;
			mover.Accelerate(toPartner);
		}
	}
}
