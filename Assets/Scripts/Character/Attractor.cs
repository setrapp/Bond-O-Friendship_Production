using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

		if (!Globals.Instance.autoAttractor)
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
		}
		else
		{
			bool pullingFluffs = AttemptFluffPull();
			if (pullingFluffs)
			{
				AttractFluffs(false);
			}
			else if (!pullingFluffs && attracting)
			{
				StopAttracting();
			}
		}

		// Keep attraction particles in correct position.
		if (attractParticles != null)
		{
			attractParticles.transform.position = transform.position;
		}
	}

	public void AttractFluffs(bool attemptFluffPull = true)
	{
		attracting = true;
		
		// If the attraction feedback is not already being presented, present it.
		if (attractParticles == null)
		{
			/*attractParticles = (ParticleSystem)Instantiate(attractionPrefab);
			attractParticles.transform.position = transform.position + new Vector3(0, 0, 0.2f);
			attractParticles.startColor = GetComponent<BondAttachable>().attachmentColor;*/
		}

		// If desired, attempt to pull in fluffs.
		if (attemptFluffPull)
		{
			AttemptFluffPull();
		}
		
	}
	
	public void StopAttracting()
	{
		attracting = false;

		if (attractParticles != null)
		{
			attractParticles.startColor = Color.Lerp(attractParticles.startColor, new Color(0, 0, 0, 0), 1.0f);
			Destroy(attractParticles.gameObject, 1.0f);
		}
	}

	private bool AttemptFluffPull()
	{
		if (Globals.Instance == null || Globals.Instance.allFluffs == null)
		{
			return false;
		}

		bool pullingFluff = false;

		List<Fluff> allFluffs = Globals.Instance.allFluffs;
		foreach (Fluff liveFluff in allFluffs)
		{
			if (liveFluff != null)
			{
				bool fluffAttachedToSelf = (liveFluff.attachee != null && liveFluff.attachee.gameObject == gameObject);
				bool ignoringAttract = !liveFluff.attractable || (liveFluff.attachee != null && liveFluff.attachee.possessive);
				Color pullColor = character.colors.attachmentColor;
				if (!fluffAttachedToSelf && !ignoringAttract && liveFluff.gameObject.activeSelf)
				{
					float fluffSqrDist = (liveFluff.transform.position - transform.position).sqrMagnitude;
					Vector3 attractOffset = Vector3.zero;

					bool foundRequiredAttractor = liveFluff.soleAttractor == null || liveFluff.soleAttractor == gameObject;

					// If the fluff is too far to be absorbed directly and absorption through the bond is enabled, attempt bond absorption.
					if (fluffSqrDist > Mathf.Pow(character.attractor.attractRange, 2) && bondAttract)
					{
						float nearSqrDist = fluffSqrDist;
						for (int i = 0; i < character.bondAttachable.bonds.Count; i++)
						{
							// Only check bond distance to fluff if the bond is at least as long as the distance from this to the fluff.
							if (Mathf.Pow(character.bondAttachable.bonds[i].BondLength, 2) >= fluffSqrDist)
							{
								Vector3 nearBond = character.bondAttachable.bonds[i].NearestPoint(liveFluff.transform.position);
								float sqrDist = (liveFluff.transform.position - nearBond).sqrMagnitude;
								if (sqrDist < nearSqrDist)
								{
									nearSqrDist = sqrDist;
									attractOffset = (nearBond - transform.position) * bondOffsetFactor;
									pullColor = new Color(1, 1, 1, 0);
								}
							}
						}
						fluffSqrDist = nearSqrDist;
					}
					if (foundRequiredAttractor && fluffSqrDist <= Mathf.Pow(character.attractor.attractRange, 2))
					{
						liveFluff.Pull(gameObject, attractOffset, attractSpeed * Time.deltaTime, pullColor);
						pullingFluff = true;
					}
				}
			}
		}

		return pullingFluff;
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
