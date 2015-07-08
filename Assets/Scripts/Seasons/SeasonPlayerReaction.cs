using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SeasonPlayerReaction : SeasonReaction {

	public CharacterComponents character;
	public float[] seasonLengthFactors = new float[3];

	protected override void Start()
	{
		base.Start();

		if (character == null)
		{
			character = GetComponent<CharacterComponents>();
		}

		ApplySeasonChanges();
	}

	override protected void ApplySeasonChanges()
	{
		base.ApplySeasonChanges();
		
		if (character != null && character.bondAttachable != null && seasonLengthFactors.Length >= 3)
		{
			BondAttachable bondAttachable = character.bondAttachable;
			for (int i = 0; i < bondAttachable.bonds.Count; i++)
			{
				
				BondAttachable bondPartner = bondAttachable.bonds[i].OtherPartner(character.bondAttachable);
				if (bondPartner == Globals.Instance.player1.character.bondAttachable || bondPartner == Globals.Instance.player2.character.bondAttachable)
				{
					Bond playerBond = character.bondAttachable.bonds[i];
					BondStats attachableStats = bondAttachable.bondOverrideStats.stats;

					switch(season)
					{
						case SeasonManager.ActiveSeason.DRY:
							/*TODO Attempt to maintain length by requestion fluffs from players*/
							playerBond.stats.maxDistance = attachableStats.maxDistance;
							break;
						case SeasonManager.ActiveSeason.WET:
							playerBond.stats.maxDistance = (attachableStats.maxDistance + (attachableStats.maxFluffCapacity * attachableStats.extensionPerFluff));
							break;
						case SeasonManager.ActiveSeason.COLD:
							playerBond.stats.maxDistance = attachableStats.maxDistance;
							break;
					}
					playerBond.stats.maxDistance *= seasonLengthFactors[(int)season];
				}
			}
		}
	}
}
