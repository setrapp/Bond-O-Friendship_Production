using UnityEngine;
using System.Collections;

public class PulseShot : MonoBehaviour {
	public PartnerLink partnerLink;
	public GameObject pulsePrefab;
	private GameObject pulse;
	public ParticleSystem pulseParticlePrefab;
	private ParticleSystem pulseParticle;
	private float pulseScale;
	public float basePulseSize = 0.25f;
	public PulseShot lastPulseAccepted;
	public bool volleyOnlyFirst = true;
	public int volleys;
	public FloatMoving floatMove;
	public float floatPushBack;
	public FluffSpawn fluffSpawn;

	void Start()
	{
		if (partnerLink == null)
		{
			partnerLink = GetComponent<PartnerLink>();
		}
		if (floatMove == null)
		{
			floatMove = GetComponent<FloatMoving>();
		}
		if (fluffSpawn == null)
		{
			fluffSpawn = GetComponent<FluffSpawn>();
		}
	}

	public void Shoot(Vector3 pulseTarget, float pulseCapacity)
	{
		/*
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
		//pulse.renderer.material.color = GetComponent<PartnerLink>().headRenderer.material.color;
		movePulse.spriteRenderer.color = GetComponent<PartnerLink>().headRenderer.material.color;
		movePulse.trail.material = partnerLink.trail.material;
		pulse.transform.LookAt(pulseTarget, -Vector3.forward);
		*/

		if (fluffSpawn == null || fluffSpawn.fluffs.Count < 1)
		{
			return;
		}

		Vector3 passDir = (pulseTarget - transform.position).normalized;
		int passFluffIndex = 0;
		GameObject passFluff = fluffSpawn.fluffs[0];
		float maxFluffDotPass = Vector3.Dot(passFluff.transform.up, passDir);
		for (int i = 1; i < fluffSpawn.fluffs.Count; i++)
		{
			float fluffDotPass = Vector3.Dot(fluffSpawn.fluffs[i].transform.up, passDir);
			if (fluffDotPass > maxFluffDotPass)
			{
				maxFluffDotPass = fluffDotPass;
				passFluff = fluffSpawn.fluffs[i];
				passFluffIndex = i;
			}
		}
		fluffSpawn.fluffs.RemoveAt(passFluffIndex);

		MovePulse movePulse = passFluff.GetComponent<MovePulse>();
		movePulse.transform.position = transform.position + (passDir * transform.localScale.magnitude);
		movePulse.transform.rotation = Quaternion.LookRotation(passDir, Vector3.Cross(passDir, -Vector3.forward));
		movePulse.transform.parent = transform.parent;
		movePulse.ReadyForPass();
		movePulse.target = pulseTarget;
		movePulse.creator = this;
		movePulse.capacity = pulseCapacity;
		movePulse.volleys = volleys + 1;
		movePulse.volleyPartner = lastPulseAccepted;
		//movePulse.spriteRenderer.color = GetComponent<PartnerLink>().headRenderer.material.color;
		//movePulse.trail.material = partnerLink.trail.material;
		//movePulse.

		// If only the first pulse can be volleyed to create a connection, ignore last pulse accepted for future shots.
		if (volleyOnlyFirst)
		{
			lastPulseAccepted = null;
		}

		// If floating propel away from pulse.
		if (floatMove.Floating)
		{
			Vector3 pulseForce = (((transform.position - pulseTarget).normalized * floatPushBack));
			rigidbody.AddForce(pulseForce, ForceMode.VelocityChange);
			partnerLink.mover.velocity = rigidbody.velocity;
		}
	}
}
