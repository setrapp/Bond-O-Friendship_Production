using UnityEngine;
using System.Collections;

public class FlufflessPass : MonoBehaviour {

	public ParticleSystem particles;
	public CharacterComponents characterComponents;
	private float defaultParticleSpeed = 0;

	void Awake()
	{
		if (particles == null)
		{
			particles = GetComponent<ParticleSystem>();
		}
		
		if (particles != null && characterComponents != null)
		{
			defaultParticleSpeed = particles.startSpeed;
			particles.startColor = characterComponents.colors.attachmentColor;
		}
	}

	public void Play()
	{
		particles.startSpeed = defaultParticleSpeed + characterComponents.mover.velocity.magnitude;
		if (particles != null)
		{
			particles.Play();
		}
	}
}
