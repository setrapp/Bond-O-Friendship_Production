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
	[HideInInspector]
	public float fillScale = 1;
	public bool empty;
	public GameObject attachPoint;
	public float minScale;

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
		fillRenderer.transform.localScale = new Vector3(fillScale, fillScale, fillScale);

		// Move attach point to edge near partner.
		attachPoint.transform.position = transform.position + (partner.transform.position - transform.position).normalized * transform.localScale.magnitude * 0.2f;

		// Stay above minScale.
		if (transform.localScale.x < minScale)
		{
			transform.localScale = new Vector3(minScale, minScale, minScale);
		}
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

	private void OnTriggerEnter(Collider other)
	{
		// If colliding with partner, reconnect.
		if (!connection.connected && other.gameObject == partner.gameObject)
		{
			connection.connected = true;
		}
	}
}
