using UnityEngine;
using System.Collections;

public class PulseShot : MonoBehaviour {
	public GameObject pulsePrefab;
	private GameObject pulse;
	public ParticleSystem pulseParticlePrefab;
	private ParticleSystem pulseParticle;
	private float pulseScale;
	public float basePulseSize = 0.5f;

	public void Shoot(Vector3 pulseTarget, float pulseCapacity)
	{
		pulse = Instantiate(pulsePrefab, transform.position, Quaternion.identity) as GameObject;
		//pulse.transform.localScale += new Vector3(pulseScale, pulseScale, pulseScale);
		MovePulse movePulse = pulse.GetComponent<MovePulse>();
		movePulse.target = pulseTarget;
		movePulse.creator = this;
		movePulse.capacity = pulseCapacity;
		pulse.transform.localScale = new Vector3(basePulseSize + pulseCapacity + pulseScale, basePulseSize + pulseCapacity + pulseScale, basePulseSize + pulseCapacity + pulseScale);
		pulseScale = 0;
		pulse.renderer.material.color = GetComponent<PartnerLink>().headRenderer.material.color;
		
		pulseParticle = (ParticleSystem)Instantiate(pulseParticlePrefab, pulse.transform.position, Quaternion.identity);
		pulseParticle.transform.parent = pulse.transform;
		pulseParticle.transform.forward = transform.position - pulseTarget;
		pulseParticle.startColor = GetComponent<PartnerLink>().headRenderer.material.color;
		pulseParticle.startSpeed = pulseTarget.magnitude;
		Destroy(pulseParticle.gameObject, 2.0f);
		Destroy(pulse, 10.0f);
	}
}
