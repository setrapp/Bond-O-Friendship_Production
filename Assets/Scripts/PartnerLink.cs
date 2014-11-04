using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartnerLink : MonoBehaviour {
	public bool isPlayer = false;
	public Renderer headRenderer;
	public Renderer fillRenderer;
	public TrailRenderer trail;
	public PulseShot pulseShot;
	public float partnerLineSize = 0.25f;
	[HideInInspector]
	public SimpleMover mover;
	[HideInInspector]
	public Tracer tracer;
	public GameObject connectionPrefab;
	[SerializeField]
	public List<SimpleConnection> connections;
	[HideInInspector]
	public float fillScale = 1;
	public bool empty;
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
		if (pulseShot == null)
		{
			pulseShot = GetComponent<PulseShot>();
		}

		fillRenderer.material.color = headRenderer.material.color;
	}
	
	void Update()
	{
		// Fill based on the amount drained by connection
		/*if (connections == null || connections.Count < 1)
		{
			fillScale = 0;
		}*/
		fillScale = 1;
		fillRenderer.transform.localScale = new Vector3(fillScale, fillScale, fillScale);
		
		//TODO This is temporary.
		//transform.localScale = new Vector3(1, 1, 1);

		// Record scale before starting charge.
		if (!chargingPulse && preChargeScale < transform.localScale.x)
		{
			preChargeScale = transform.localScale.x;
		}

		// TODO Scaling up between colliders near parallel gets body stuck inside. Fix or remove scaling.

		// Restore scale up to normal, if below it and not charging.
		if (transform.localScale.x < normalScale && !chargingPulse)
		{
			// If scale is less than the scale before starting charge, scale up to that first.
			if (transform.localScale.x < preChargeScale)
			{
				float actualRestoreRate = endChargeRestoreRate * transform.localScale.x;
				transform.localScale = new Vector3(Mathf.Min(transform.localScale.x + actualRestoreRate * Time.deltaTime, preChargeScale), Mathf.Min(transform.localScale.y + actualRestoreRate * Time.deltaTime, normalScale), Mathf.Min(transform.localScale.z + actualRestoreRate * Time.deltaTime, normalScale));
			}
			else
			{
				float actualRestoreRate = scaleRestoreRate * transform.localScale.x;
				transform.localScale = new Vector3(Mathf.Min(transform.localScale.x + actualRestoreRate * Time.deltaTime, normalScale), Mathf.Min(transform.localScale.y + actualRestoreRate * Time.deltaTime, normalScale), Mathf.Min(transform.localScale.z + actualRestoreRate * Time.deltaTime, normalScale));
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

		trail.startWidth = transform.localScale.x;
	}

	private void OnTriggerEnter(Collider other)
	{
		// If colliding with a pulse, accept it.
		if (other.gameObject.tag == "Pulse" && chargingPulse)
		{
			MovePulse pulse = other.GetComponent<MovePulse>();
			if (pulse != null)// && (pulse.creator == null || pulse.creator != pulseShot))
			{
				//transform.localScale += new Vector3(pulse.capacity, pulse.capacity, pulse.capacity);
				pulseShot.volleys = 1;
				if (pulse.volleyPartner != null && pulse.volleyPartner == pulseShot)
				{
					pulseShot.volleys = pulse.volleys;
				}
				if (pulseShot.volleys >= volleysToConnect)
				{
					bool connectionAlreadyMade = false;
					for (int i = 0; i < connections.Count && !connectionAlreadyMade; i++)
					{
						if ((connections[i].attachment1.partner == this && connections[i].attachment2.partner == pulse.creator.partnerLink) || (connections[i].attachment2.partner == this && connections[i].attachment1.partner == pulse.creator.partnerLink))
						{
							connectionAlreadyMade = true;
						}
					}
					if (!connectionAlreadyMade)
					{
						SimpleConnection newConnection = ((GameObject)Instantiate(connectionPrefab, Vector3.zero, Quaternion.identity)).GetComponent<SimpleConnection>();
						connections.Add(newConnection);
						pulse.creator.partnerLink.connections.Add(newConnection);
						newConnection.AttachPartners(pulse.creator.partnerLink, this);
					}
				}
				pulseShot.lastPulseAccepted = pulse.creator;
				//Destroy(pulse.gameObject);
				pulse.EndPass();
				pulse.transform.parent = pulseShot.fluffSpawn.fluffContainer.transform;
				Quaternion fluffRotation = Random.rotation;
				fluffRotation.x = fluffRotation.y = 0;
				pulse.transform.rotation = fluffRotation;
				pulse.transform.position = pulseShot.fluffSpawn.fluffContainer.transform.position + pulse.transform.up * pulseShot.fluffSpawn.spawnOffset;
				pulseShot.fluffSpawn.fluffs.Add(pulse.gameObject);
			}
		}
	}
}
