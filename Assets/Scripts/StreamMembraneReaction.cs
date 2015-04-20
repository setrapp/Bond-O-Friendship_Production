using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StreamMembraneReaction : StreamReaction {

	public MembraneWall membraneWall;
	private Membrane createdMembrane;

	void Awake()
	{
		membraneWall.createOnStart = false;
	}

	public override bool React(float actionRate)
	{
		bool reacted = base.React(actionRate);
		if (reacted)
		{
			base.React(actionRate);

			if (reactionProgress >= 1)
			{
				if (createdMembrane == null)
				{
					createdMembrane = membraneWall.CreateWall();
				}
			}
		}
		return reacted;
	}
}
