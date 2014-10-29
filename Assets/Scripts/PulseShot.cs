using UnityEngine;
using System.Collections;

public class PulseShot : MonoBehaviour {
	public PartnerLink partnerLink;
	public GameObject pulsePrefab;
	private GameObject pulse;
	public ParticleSystem pulseParticlePrefab;
	private ParticleSystem pulseParticle;
	private float pulseScale;
	public float basePulseSize = 0.5f;
	public PulseShot lastPulseAccepted;
	public bool volleyOnlyFirst = true;
	public int volleys;

	void Start()
	{
		if (partnerLink == null)
		{
			partnerLink = GetComponent<PartnerLink>();
		}
	}

	public void Shoot(Vector3 pulseTarget, float pulseCapacity)
	{
		// Create pulse.
		pulse = Instantiate(pulsePrefab, transform.position, Quaternion.identity) as GameObject;
		MovePulse movePulse = pulse.GetComponent<MovePulse>();
		movePulse.target = pulseTarget;
		movePulse.creator = this;
		movePulse.capacity = pulseCapacity;
		movePulse.volleys = volleys + 1;
		movePulse.volleyPartner = lastPulseAccepted;
		pulse.transform.localScale = new Vector3(basePulseSize + pulseCapacity + pulseScale, basePulseSize + pulseCapacity + pulseScale, basePulseSize + pulseCapacity + pulseScale);
		pulseScale = 0;
		pulse.renderer.material.color = GetComponent<PartnerLink>().headRenderer.material.color;

		// Create particle trail behind pulse.
		pulseParticle = (ParticleSystem)Instantiate(pulseParticlePrefab, pulse.transform.position, Quaternion.identity);
		pulseParticle.transform.parent = pulse.transform;
		pulseParticle.transform.forward = transform.position - pulseTarget;
		pulseParticle.startColor = GetComponent<PartnerLink>().headRenderer.material.color;
		pulseParticle.startSpeed = pulseTarget.magnitude;
		Destroy(pulseParticle.gameObject, 2.0f);
		Destroy(pulse, 10.0f);

		// If only the first pulse can be volleyed to create a connection, ignore last pulse accepted for future shots.
		if (volleyOnlyFirst)
		{
			lastPulseAccepted = null;
		}
	}
}
