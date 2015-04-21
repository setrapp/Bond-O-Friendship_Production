using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StreamFluffSproutReaction : StreamReaction {

	public FluffStickRoot fluffStickRoot;
	public List<FluffPlaceholder> fluffPlaceholders;
	public bool spawnAll = false;
	public bool findPlaceholders = true;
	private float actualActionRate;
	private float actualConsumptionRate;
	private bool usingStream = false;
	

	public void Awake()
	{
		if (fluffStickRoot == null)
		{
			fluffStickRoot = GetComponent<FluffStickRoot>();
		}

		if (findPlaceholders)
		{
			FluffPlaceholder[] foundPlaceholders = GetComponentsInChildren<FluffPlaceholder>();
			for (int i = 0; i < foundPlaceholders.Length; i++)
			{
				if (!fluffPlaceholders.Contains(foundPlaceholders[i]))
				{
					foundPlaceholders[i].autoSpawn = false;
					foundPlaceholders[i].fluffRespawns = -1;
					fluffPlaceholders.Add(foundPlaceholders[i]);
				}
			}
		}
	}

	void Update()
	{
		if (!reacting)
		{
			fluffStickRoot.fluffActionRate = actualActionRate;
			fluffStickRoot.fluffConsumeRate = actualConsumptionRate;
			usingStream = false;
		}
		reacting = false;
	}

	public override bool React(float actionRate)
	{
		bool reacted = base.React(actionRate);
		if (reacted)
		{
			if (reactionProgress >= 1)
			{
				bool spawned = false;
				for (int i = 0; i < fluffPlaceholders.Count && (spawnAll || !spawned); i++)
				{
					if (fluffPlaceholders[i] == null)
					{
						fluffPlaceholders.RemoveAt(i);
						i--;
					}
					else
					{
						if (fluffPlaceholders[i].createdFluff == null && (fluffPlaceholders[i].attachee == null || fluffPlaceholders[i].attachee.root == null || fluffPlaceholders[i].attachee.root.trackStuckFluffs || fluffPlaceholders[i].attachee.stuckFluff == null))
						{
							fluffPlaceholders[i].SpawnFluff();
							spawned = true;
						}
					}
				}

				if (spawned)
				{
					reactionProgress = 0;
				}
			}
		}

		if (!usingStream)
		{
			if (fluffStickRoot != null)
			{
				actualActionRate = fluffStickRoot.fluffActionRate;
				actualConsumptionRate = fluffStickRoot.fluffConsumeRate;

				fluffStickRoot.fluffActionRate = 0;
				fluffStickRoot.fluffConsumeRate = 0;
			}
			
			usingStream = true;
		}
		reacting = true;

		return reacted;
	}

	
}
