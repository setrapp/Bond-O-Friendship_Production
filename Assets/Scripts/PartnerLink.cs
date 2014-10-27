using UnityEngine;
using System.Collections;

public class PartnerLink : MonoBehaviour {
	public bool isPlayer = false;
	public PartnerLink partner;
	public Renderer headRenderer;
	public Renderer fillRenderer;
	public LineRenderer partnerLine;
	public PulseShot pulseShot;
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
	public float maxScale;
	public float normalScale;
	public float preChargeScale;
	public float scaleRestoreRate;
	public float endChargeRestoreRate;
	public bool chargingPulse = false;
	public int volleysToConnect = 2;

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
		if (pulseShot == null)
		{
			pulseShot = GetComponent<PulseShot>();
		}

		fillRenderer.material.color = headRenderer.material.color;
	}
	
	void Update()
	{
		// Fill based on the amount drained by connection
		fillRenderer.transform.localScale = new Vector3(fillScale, fillScale, fillScale);

		// Move attach point to edge near partner.
		attachPoint.transform.position = transform.position + (partner.transform.position - transform.position).normalized * transform.localScale.magnitude * 0.2f;

		// Record scale before starting charge.
		if (!chargingPulse && preChargeScale < transform.localScale.x)
		{
			preChargeScale = transform.localScale.x;
		}

		// Restore scale up to normal, if below it and not charging.
		if (transform.localScale.x < normalScale && !chargingPulse)
		{
			// If scale is less than the scale before starting charge, scale up to that first.
			if (transform.localScale.x < preChargeScale)
			{
				transform.localScale = new Vector3(Mathf.Min(transform.localScale.x + endChargeRestoreRate * Time.deltaTime, preChargeScale), Mathf.Min(transform.localScale.y + endChargeRestoreRate * Time.deltaTime, normalScale), Mathf.Min(transform.localScale.z + endChargeRestoreRate * Time.deltaTime, normalScale));
			}
			else
			{
				transform.localScale = new Vector3(Mathf.Min(transform.localScale.x + scaleRestoreRate * Time.deltaTime, normalScale), Mathf.Min(transform.localScale.y + scaleRestoreRate * Time.deltaTime, normalScale), Mathf.Min(transform.localScale.z + scaleRestoreRate * Time.deltaTime, normalScale));
			}
		}

		// Stay within scale bounds.
		if (transform.localScale.x < minScale)
		{
			transform.localScale = new Vector3(minScale, minScale, minScale);
		}
		else if (transform.localScale.x > maxScale)
		{
			transform.localScale = new Vector3(maxScale, maxScale, maxScale);
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
		// If colliding with a pulse, accept it.
		if (other.gameObject.tag == "Pulse")
		{
			
			MovePulse pulse = other.GetComponent<MovePulse>();
			if (pulse != null && (pulse.creator == null || pulse.creator != pulseShot))
			{
				transform.localScale += new Vector3(pulse.capacity, pulse.capacity, pulse.capacity);
				pulseShot.volleys = 1;
				if (pulse.volleyPartner != null && pulse.volleyPartner == pulseShot)
				{
					pulseShot.volleys = pulse.volleys;
				}
				if (pulseShot.volleys >= volleysToConnect && !connection.connected)
				{
					connection.connected = true;
				}
				pulseShot.lastPulseAccepted = pulse.creator;
				Destroy(pulse.gameObject);
			}
		}
	}
}
