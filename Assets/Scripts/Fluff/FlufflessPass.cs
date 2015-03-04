using UnityEngine;
using System.Collections;

public class FlufflessPass : MonoBehaviour {

	public ParticleSystem particles;
	public CharacterColors characterColors;

	void Awake()
	{
		if (particles == null)
		{
			particles = GetComponent<ParticleSystem>();
		}
		
		if (particles != null && characterColors != null)
		{
			particles.startColor = characterColors.attachmentColor;
		}
	}

	public void Play()
	{
		if (particles != null)
		{
			particles.Play();
		}
	}
}
