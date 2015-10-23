using UnityEngine;
using System.Collections;

public class AllowPlayerBond : MonoBehaviour {
	public WaitPad triggerPad;
	public ClusterNodePuzzleGroup triggerNodeGroup;
	public MembraneWall triggerMembraneWall;
	public bool allowBond = true;
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

	private void MembraneWallBroken(MembraneWall brokenMembrane)
	{
		if (brokenMembrane == triggerMembraneWall)
		{
			AllowBond();
		}
	}

	public void AllowBond()
	{
		// If disabled in editor, do not change player bond stats.
		if (!(editorDisablable && Globals.Instance.earlyBondInEditor && Application.isEditor) && !Globals.Instance.bondAllowed)
		{
			Globals.Instance.Player1.character.bondAttachable.enabled = allowBond;
			Globals.Instance.Player2.character.bondAttachable.enabled = allowBond;
			
			if (makeBond)
			{
				Globals.Instance.Player1.character.bondAttachable.AttemptBond(Globals.Instance.Player2.character.bondAttachable, Globals.Instance.Player1.transform.position, true);

			}

			if (allowBond)
			{
				Globals.Instance.bondAllowed = true;
			}
		}

		if (destroyOnUse)
		{
			Destroy(this);
		}
	}
}
