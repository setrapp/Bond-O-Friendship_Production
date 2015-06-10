using UnityEngine;
using System.Collections;

public class AllowPlayerBond : MonoBehaviour {
	public WaitPad triggerPad;
	public ClusterNodePuzzleGroup triggerNodeGroup;
	public MembraneWall triggerMembraneWall;
	public float startingBondLength = 35;
	public bool makeBond = false;
	public bool autoCheck = true;
	public bool destroyOnUse = true;
	public bool editorDisablable = false;

	void Update()
	{
		if (autoCheck)
		{
			if ((triggerPad == null && triggerNodeGroup == null && triggerMembraneWall == null) || ((triggerPad != null && triggerPad.activated) || (triggerNodeGroup != null && triggerNodeGroup.solved)))
			{
				AllowBond();
			}
		}
	}

	private void MembraneBroken(MembraneWall brokenMembrane)
	{
		if (brokenMembrane == triggerMembraneWall)
		{
			AllowBond();
		}
	}

	public void AllowBond()
	{
		// If disabled in editor, do not change player bond stats.
		if (!(editorDisablable && Globals.Instance.earlyBondInEditor && Application.isEditor))
		{
			Globals.Instance.player1.character.bondAttachable.bondOverrideStats.stats.maxDistance = startingBondLength;
			Globals.Instance.player2.character.bondAttachable.bondOverrideStats.stats.maxDistance = startingBondLength;
			
			if (makeBond)
			{
				Globals.Instance.player1.character.bondAttachable.AttemptBond(Globals.Instance.player2.character.bondAttachable, Globals.Instance.player1.transform.position, true);
			}
		}

		if (destroyOnUse)
		{
			Destroy(this);
		}
	}
}
