using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SeasonPlayerReaction : SeasonReaction {

	public CharacterComponents character;
	public float[] seasonDragFactors = new float[3];
	private float baseDrag;
	public float[] seasonLengthFactors = new float[3];

	protected override void Start()
	{
		base.Start();

		if (character == null)
		{
			character = GetComponent<CharacterComponents>();
		}

		if (character != null && character.body != null)
		{
			baseDrag = character.body.drag;
		}

		ApplySeasonChanges();
	}

	override protected void ApplySeasonChanges()
	{
		base.ApplySeasonChanges();
		
		/*TOOD change tail length???*/

		if (character != null && character.bondAttachable != null && seasonLengthFactors.Length >= 3)
		{
			for (int i = 0; i < character.bondAttachable.bonds.Count; i++)
			{
				BondAttachable bondPartner = character.bondAttachable.bonds[i].OtherPartner(character.bondAttachable);
				if (bondPartner == Globals.Instance.Player1.character.bondAttachable || bondPartner == Globals.Instance.Player2.character.bondAttachable)
				{
					BondSeasonReact(character.bondAttachable.bonds[i]);
				}
			}
		}

		if (character != null && character.body != null)
		{
			character.body.drag = baseDrag * seasonDragFactors[(int)season];
		}
	}

	public void BondSeasonReact(Bond reactingBond)
	{
		if (manager == null)
		{
			return;
		}

		BondStats attachableStats = character.bondAttachable.bondOverrideStats.stats;

		switch (season)
		{
			case SeasonManager.ActiveSeason.DRY:
				/*TODO Attempt to maintain length by requestion fluffs from players*/
				reactingBond.stats.maxDistance = attachableStats.maxDistance;
				reactingBond.stats.maxFluffCapacity = attachableStats.maxFluffCapacity;
				break;
			case SeasonManager.ActiveSeason.WET:
				reactingBond.stats.maxDistance = (attachableStats.maxDistance + (attachableStats.maxFluffCapacity * attachableStats.extensionPerFluff));
				reactingBond.stats.maxFluffCapacity = 0;
				break;
			case SeasonManager.ActiveSeason.COLD:
				reactingBond.stats.maxDistance = attachableStats.maxDistance;
				reactingBond.stats.maxFluffCapacity = 0;
				break;
		}
		reactingBond.stats.maxDistance *= seasonLengthFactors[(int)season];
	}

	public void ChangeActiveLevel(Island activeIsland)
	{
		FindManager(true);
	}

	public void BondAttached(Bond newBond)
	{
		BondSeasonReact(newBond);
	}
}
