using UnityEngine;
using System.Collections;

public class Attractor : MonoBehaviour {
	public CharacterComponents character;
	[HideInInspector]
	public bool attracting = false;
	public float attractRange = 5;
	public float attractSpeed = 100;
	public bool bondAttract = true;
	public float bondOffsetFactor = 0.9f;
	private bool slowing = false;
	private bool wasSlowing = false;
	public float attractSlowingFactor = 0.15f;
	private ParticleSystem attractParticles;
	public ParticleSystem attractionPrefab;

	void Awake()
	{
		if (character == null)
		{
			character = GetComponent<CharacterComponents>();
		}
	}

	void Update()
	{
		// Slow movement if attracting fluffs.
		slowing = attracting && !character.floatMove.Floating;
		if (slowing != wasSlowing)
		{
			if (slowing)
			{
				character.mover.externalSpeedMultiplier -= attractSlowingFactor;
			}
			else
			{
				character.mover.externalSpeedMultiplier += attractSlowingFactor;
			}
			wasSlowing = slowing;
		}

		// Keep attraction particles in correct position.
		if (attractParticles != null)
		{
			attractParticles.transform.position = transform.position;
		}
	}

	public void AttractFluffs()
	{
		attracting = true;
		
		// If the attraction feedback is not already being presented, present it.
		if (attractParticles == null)
		{
			attractParticles = (ParticleSystem)Instantiate(attractionPrefab);
			attractParticles.transform.position = transform.position;
			attractParticles.startColor = GetComponent<BondAttachable>().attachmentColor / 2;
			attractParticles.startColor = new Color(attractParticles.startColor.r, attractParticles.startColor.g, attractParticles.startColor.b, 0.1f);
		}

		// Attempt to pull in fluffs.
		GameObject[] fluffArray = GameObject.FindGameObjectsWithTag("Fluff");
		foreach (GameObject liveFluffObject in fluffArray)
		{
			Fluff liveFluff = liveFluffObject.GetComponent<Fluff>();
			if (liveFluff != null)
			{
				bool fluffAttachedToSelf = (liveFluff.attachee != null && liveFluff.attachee.gameObject == gameObject);
				if (!fluffAttachedToSelf)
				{
					float fluffSqrDist = (liveFluff.transform.position - transform.position).sqrMagnitude;
					Vector3 attractOffset = Vector3.zero;
					// If the fluff is too far to be absorbed directly and absorption through the bond is enabled, attempt bond absorption.
					if (fluffSqrDist > Mathf.Pow(character.attractor.attractRange, 2) && bondAttract)
					{
						float nearSqrDist = fluffSqrDist;
						for (int i = 0; i < character.bondAttachable.bonds.Count; i++)
						{
							// Only check bond distance to fluff if the bond is at least as long as the distance from this to the fluff.
							if (Mathf.Pow(character.bondAttachable.bonds[i].BondLength, 2) >= fluffSqrDist)
							{
								Vector3 nearBond = character.bondAttachable.bonds[i].NearestPoint(liveFluffObject.transform.position);
								float sqrDist = (liveFluffObject.transform.position - nearBond).sqrMagnitude;
								if (sqrDist < nearSqrDist)
								{
									nearSqrDist = sqrDist;
									attractOffset = (nearBond - transform.position) * bondOffsetFactor;
								}
							}
						}
						fluffSqrDist = nearSqrDist;
					}
					if (fluffSqrDist <= Mathf.Pow(character.attractor.attractRange, 2))
					{
						liveFluff.Pull(gameObject, attractOffset, attractSpeed * Time.deltaTime);
					}
				}
			}
		}
		
	}
	
	public void StopAttracting()
	{
		attracting = false;

		if (attractParticles != null)
		{
			attractParticles.startColor = Color.Lerp(attractParticles.startColor, new Color(0, 0, 0, 0), 0.5f);
			Destroy(attractParticles.gameObject, 1.0f);
		}
	}

	void OnDrawGizmos()
	{
		if (attracting)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, attractRange);
		}
	}
}
