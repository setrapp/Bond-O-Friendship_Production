using UnityEngine;
using System.Collections;

public class PartnerLink : MonoBehaviour {
	public bool isPlayer = false;
	public PartnerLink partner;
	public Renderer headRenderer;
	public Renderer fillRenderer;
	public LineRenderer partnerLine;
	public float partnerLineSize = 0.25f;
	[HideInInspector]
	public SimpleMover mover;
	[HideInInspector]
	public Tracer tracer;
	public SimpleConnection connection;
	public bool empty;
	public GameObject attachPoint;

	void Awake()
	{
		if (mover == null)
		{
			mover = GetComponent<SimpleMover>();
		}
		if (tracer == null)
		{
			tracer = GetComponent<Tracer>();
		}
		if (partnerLine == null)
		{
			partnerLine = GetComponent<LineRenderer>();
		}

		fillRenderer.material.color = headRenderer.material.color;
	}
	
	void Update()
	{
		// Fill based on the amount drained by connection
		float fillScale = 1 - connection.drained;
		fillRenderer.transform.localScale = new Vector3(fillScale, fillScale, fillScale);
		empty = (fillScale <= 0);

		// Move attach point to edge near partner.
		attachPoint.transform.position = transform.position + (partner.transform.position - transform.position).normalized * transform.localScale.magnitude * 0.2f;
	}

	public void SetPartner(PartnerLink partner)
	{
		this.partner = partner;
		
		if (partner != null)
		{
			SendMessage("LinkPartner", SendMessageOptions.DontRequireReceiver);
		}
		else
		{
			SendMessage("UnlinkPartner", SendMessageOptions.DontRequireReceiver);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		// If colliding with partner, reconnect.
		if (!connection.connected && other.gameObject == partner.gameObject)
		{
			connection.connected = true;
		}
	}
}
