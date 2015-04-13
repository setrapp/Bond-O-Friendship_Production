using UnityEngine;
using System.Collections;

public class FloatMoving : MonoBehaviour {
	public CharacterComponents character;
	public float trailShrinkRate = 1.0f;
	public MovementStats startingStats;
	public MovementStats loneFloatStats;
	public MovementStats perBondFloatBonus;
	public int maxBondBonuses;
	public LayerMask ignoreLayers;
	private bool wasFloating = false;
	public bool collided;
	public bool Floating
	{
		get { return wasFloating; }
	}

	void Awake()
	{
		if (character == null)
		{
			character = GetComponent<CharacterComponents>();
		}
	}

	void Start()
	{
		startingStats = new MovementStats();
		startingStats.acceleration = character.mover.acceleration;
		startingStats.handling = character.mover.handling;
		if (character.mover.body != null)
		{
			startingStats.bodyDrag = character.mover.body.drag;
		}
		startingStats.bodylessDampening = character.mover.bodylessDampening;
		startingStats.sideTrailTime = character.leftTrail.time;
		startingStats.midTrailTime = character.midTrail.time;
		startingStats.attractRange = character.attractor.attractRange;
		startingStats.maxAbsorbReact = character.fluffStickRoot.maxPullForce;
	}

	void Update()
	{
		if (!wasFloating && character.midTrail.time > startingStats.midTrailTime)
		{
			float shrinkFactor = Time.deltaTime * trailShrinkRate;
			if (character.midTrail.time - startingStats.midTrailTime < shrinkFactor)
			{
				shrinkFactor = 1;
			}
			character.leftTrail.time -= (character.leftTrail.time - startingStats.sideTrailTime) * shrinkFactor;
			character.midTrail.time -= (character.midTrail.time - startingStats.midTrailTime) * shrinkFactor;
			character.rightTrail.time -= (character.rightTrail.time - startingStats.sideTrailTime) * shrinkFactor;
		}

		RaycastHit hit;
		if(Physics.Raycast(transform.position, Vector3.forward, out hit, Mathf.Infinity, ~ignoreLayers))
		{
			if (wasFloating)
			{
				character.mover.acceleration = startingStats.acceleration;
				character.mover.handling = startingStats.handling;
				if (character.mover.body != null)
				{
					character.mover.body.drag = startingStats.bodyDrag;
				}
				character.mover.bodylessDampening = startingStats.bodylessDampening;
				character.attractor.attractRange = startingStats.attractRange;
				character.fluffStickRoot.maxPullForce = startingStats.maxAbsorbReact;
				wasFloating = false;
			}
		}
		else if (!wasFloating)
		{
			wasFloating = true;
			ApplyFloatStats();

			// Ensure that players have fluffs while floating.
			/*if (character.fluffHandler.naturalFluffCount <= 0)
			{
				character.fluffHandler.naturalFluffCount = 1;
			}*/
		}
	}

	private void ApplyFloatStats()
	{
		if (wasFloating)
		{
			character.mover.acceleration = loneFloatStats.acceleration;
			character.mover.handling = loneFloatStats.handling;
			if (character.mover.body != null)
			{
				character.mover.body.drag = loneFloatStats.bodyDrag;
			}
			character.mover.bodylessDampening = loneFloatStats.bodylessDampening;
			character.leftTrail.time = character.rightTrail.time = loneFloatStats.sideTrailTime;
			character.midTrail.time = loneFloatStats.midTrailTime;
			character.attractor.attractRange = loneFloatStats.attractRange;
			character.fluffStickRoot.maxPullForce = loneFloatStats.maxAbsorbReact;

			int bondBonusCount = Mathf.Min(character.bondAttachable.bonds.Count, maxBondBonuses);
			if (bondBonusCount > 0)
			{
				character.mover.acceleration += perBondFloatBonus.acceleration * bondBonusCount;
				character.mover.handling += perBondFloatBonus.handling * bondBonusCount;
				if (character.mover.body != null)
				{
					character.mover.body.drag += perBondFloatBonus.bodyDrag * bondBonusCount;
				}
				character.mover.bodylessDampening += perBondFloatBonus.bodylessDampening * bondBonusCount;
				character.leftTrail.time = character.rightTrail.time += perBondFloatBonus.sideTrailTime * bondBonusCount;
				character.midTrail.time += perBondFloatBonus.midTrailTime * bondBonusCount;
				character.attractor.attractRange = perBondFloatBonus.attractRange * bondBonusCount;
				character.fluffStickRoot.maxPullForce = perBondFloatBonus.maxAbsorbReact * bondBonusCount;
			}
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.tag == "Character")
		{
			collided = true;
			BondAttachable partner = collision.collider.GetComponent<BondAttachable>();
			if (Floating && partner != null && !character.bondAttachable.IsBondMade(partner))
			{
				character.bondAttachable.AttemptBond(partner, transform.position, true);
			}
		}

	}

	private void BondMade(BondAttachable bondPartner)
	{
		ApplyFloatStats();
	}

	private void BondBroken(BondAttachable disconnectedPartner)
	{
		ApplyFloatStats();
	}
}

[System.Serializable]
public class MovementStats
{
	public float acceleration;
	public float handling;
	public float bodyDrag;
	public float bodylessDampening;
	public float sideTrailTime;
	public float midTrailTime;
	public float attractRange;
	public float maxAbsorbReact;
}
