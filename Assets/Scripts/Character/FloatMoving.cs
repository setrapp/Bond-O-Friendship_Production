using UnityEngine;
using System.Collections;

public class FloatMoving : MonoBehaviour {
	public PartnerLink partnerLink;
	public SimpleMover mover;
	public FluffSpawn fluffSpawn;
	public FluffStick fluffStick;
	public MovementStats startingStats;
	public MovementStats loneFloatStats;
	public MovementStats perBondFloatBonus;
	public int maxBondBonuses;
	public LayerMask ignoreLayers;
	private bool wasFloating = false;
	public bool Floating
	{
		get { return wasFloating; }
	}

	void Awake()
	{
		if (mover == null)
		{
			mover = GetComponent<SimpleMover>();
		}
		if (partnerLink == null)
		{
			partnerLink = GetComponent<PartnerLink>();
		}
		if (fluffSpawn == null)
		{
			fluffSpawn = GetComponent<FluffSpawn>();
		}
		if (fluffStick == null)
		{
			fluffStick = GetComponent<FluffStick>();
		}
	}

	void Start()
	{
		startingStats = new MovementStats();
		startingStats.acceleration = mover.acceleration;
		startingStats.handling = mover.handling;
		if (mover.body != null)
		{
			startingStats.bodyDrag = mover.body.drag;
		}
		startingStats.bodylessDampening = mover.bodylessDampening;
		startingStats.sideTrailTime = partnerLink.leftTrail.time;
		startingStats.midTrailTime = partnerLink.midTrail.time;
		startingStats.absorbStrength = partnerLink.absorbStrength;
		startingStats.maxAbsorbReact = fluffStick.maxPullForce;
	}

	void Update()
	{

		RaycastHit hit;
		if(Physics.Raycast(transform.position, Vector3.forward, out hit, Mathf.Infinity, ~ignoreLayers))
		{
			if (wasFloating)
			{
				mover.acceleration = startingStats.acceleration;
				mover.handling = startingStats.handling;
				if (mover.body != null)
				{
					mover.body.drag = startingStats.bodyDrag;
				}
				mover.bodylessDampening = startingStats.bodylessDampening;
				partnerLink.leftTrail.time = partnerLink.rightTrail.time = startingStats.sideTrailTime;
				partnerLink.midTrail.time = partnerLink.rightTrail.time = startingStats.sideTrailTime;
				partnerLink.absorbStrength = startingStats.absorbStrength;
				fluffStick.maxPullForce = startingStats.maxAbsorbReact;
				wasFloating = false;
			}
		}
		else if (!wasFloating)
		{
			wasFloating = true;
			ApplyFloatStats();

			// Ensure that players have fluffs while floating.
			if (fluffSpawn.naturalFluffCount <= 0)
			{
				fluffSpawn.naturalFluffCount = 1;
			}
		}
	}

	private void ApplyFloatStats()
	{
		if (wasFloating)
		{
			mover.acceleration = loneFloatStats.acceleration;
			mover.handling = loneFloatStats.handling;
			if (mover.body != null)
			{
				mover.body.drag = loneFloatStats.bodyDrag;
			}
			mover.bodylessDampening = loneFloatStats.bodylessDampening;
			partnerLink.leftTrail.time = partnerLink.rightTrail.time = loneFloatStats.sideTrailTime;
			partnerLink.midTrail.time = partnerLink.rightTrail.time = loneFloatStats.midTrailTime;
			partnerLink.absorbStrength = loneFloatStats.absorbStrength;
			fluffStick.maxPullForce = loneFloatStats.maxAbsorbReact;

			int connectionBonusCount = Mathf.Min(partnerLink.connectionAttachable.bonds.Count, maxBondBonuses);
			if (connectionBonusCount > 0)
			{
				mover.acceleration += perBondFloatBonus.acceleration * connectionBonusCount;
				mover.handling += perBondFloatBonus.handling * connectionBonusCount;
				if (mover.body != null)
				{
					mover.body.drag += perBondFloatBonus.bodyDrag * connectionBonusCount;
				}
				mover.bodylessDampening += perBondFloatBonus.bodylessDampening * connectionBonusCount;
				partnerLink.leftTrail.time = partnerLink.rightTrail.time += perBondFloatBonus.sideTrailTime * connectionBonusCount;
				partnerLink.midTrail.time = partnerLink.rightTrail.time += perBondFloatBonus.midTrailTime * connectionBonusCount;
				partnerLink.absorbStrength = perBondFloatBonus.absorbStrength * connectionBonusCount;
				fluffStick.maxPullForce = perBondFloatBonus.maxAbsorbReact * connectionBonusCount;
			}
		}
	}

	private void BondMade(BondAttachable connectionPartner)
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
	public float absorbStrength;
	public float maxAbsorbReact;
}
