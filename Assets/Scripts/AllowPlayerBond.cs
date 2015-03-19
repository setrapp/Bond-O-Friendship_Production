using UnityEngine;
using System.Collections;

public class AllowPlayerBond : MonoBehaviour {
	public WaitPad triggerPad;
	public float startingBondLength = 35;

	void Update()
	{
		if (triggerPad == null || triggerPad.activated)
		{
			Globals.Instance.player1.character.bondAttachable.bondOverrideStats.stats.maxDistance = startingBondLength;
			Globals.Instance.player2.character.bondAttachable.bondOverrideStats.stats.maxDistance = startingBondLength;
			Destroy(this);
		}
	}
}
