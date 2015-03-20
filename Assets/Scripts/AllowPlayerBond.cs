using UnityEngine;
using System.Collections;

public class AllowPlayerBond : MonoBehaviour {
	public WaitPad triggerPad;
	public float startingBondLength = 35;
	public bool makeBond = false;

	void Update()
	{
		if (triggerPad == null || triggerPad.activated)
		{
			Globals.Instance.player1.character.bondAttachable.bondOverrideStats.stats.maxDistance = startingBondLength;
			Globals.Instance.player2.character.bondAttachable.bondOverrideStats.stats.maxDistance = startingBondLength;

			if (makeBond)
			{
				Globals.Instance.player1.character.bondAttachable.AttemptBond(Globals.Instance.player2.character.bondAttachable, Globals.Instance.player1.transform.position, true);
			}

			Destroy(this);
		}
	}
}
