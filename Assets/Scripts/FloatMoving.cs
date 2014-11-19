using UnityEngine;
using System.Collections;

public class FloatMoving : MonoBehaviour {
	public PartnerLink partnerLink;
	public SimpleMover mover;
	private MovementStats startingStats;
	public MovementStats loneFloatStats;
	public MovementStats perConnectionFloatBonus;
	public int maxConnectionBonuses;
	public LayerMask ignoreLayers;
	private bool wasFloating = false;
	public bool Floating
	{
		get { return wasFloating; }
	}

	void Start()
	{
		if (mover == null)
		{
			mover = GetComponent<SimpleMover>();
		}
		if (partnerLink == null)
		{
			partnerLink = GetComponent<PartnerLink>();
		}

		startingStats = new MovementStats();
		startingStats.acceleration = mover.acceleration;
		startingStats.handling = mover.handling;
		startingStats.dampening = mover.dampening;
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
				mover.dampening = startingStats.dampening;
				wasFloating = false;
			}
		}
		else if (!wasFloating)
		{
			wasFloating = true;
			ApplyFloatStats();
		}
	}

	private void ApplyFloatStats()
	{
		if (wasFloating)
		{
			mover.acceleration = loneFloatStats.acceleration;
			mover.handling = loneFloatStats.handling;
			mover.dampening = loneFloatStats.dampening;

			int connectionBonusCount = Mathf.Min(partnerLink.connections.Count, maxConnectionBonuses);
			if (connectionBonusCount > 0)
			{
				mover.acceleration += perConnectionFloatBonus.acceleration * connectionBonusCount;
				mover.handling += perConnectionFloatBonus.handling * connectionBonusCount;
				mover.dampening += perConnectionFloatBonus.dampening * connectionBonusCount;
			}
		}
	}

	private void ConnectionMade(PartnerLink connectedPartner)
	{
		ApplyFloatStats();
	}

	private void ConnectionBroken(PartnerLink disConnectedPartner)
	{
		ApplyFloatStats();
	}
}

[System.Serializable]
public class MovementStats
{
	public float acceleration = 0;
	public float handling = 0;
	public float dampening = 1;
}
