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

		actualActionRate = fluffStickRoot.fluffActionRate;
		actualConsumptionRate = fluffStickRoot.fluffConsumeRate;
	}

	override protected void Update()
	{
		if (streamsTouched <= 0 && (fluffStickRoot.fluffActionRate != actualActionRate || fluffStickRoot.fluffConsumeRate != actualConsumptionRate))
		{
			fluffStickRoot.fluffActionRate = actualActionRate;
			fluffStickRoot.fluffConsumeRate = actualConsumptionRate;
		}

		base.Update();
	}

	public override bool React(float actionRate)
	{
		bool reacted = base.React(actionRate);
		if (reacted)
		{
			
			if (reactionProgress >= 1 && streamsTouched > 0)
			{
				List<FluffPlaceholder> openPlaceholders = new List<FluffPlaceholder>();
				for (int i = 0; i < fluffPlaceholders.Count; i++)
				{
					if (fluffPlaceholders[i] == null)
					{
						fluffPlaceholders.RemoveAt(i);
						i--;
					}
					else
					{
						// Keep a list all fluffs spots that are open for fluff spawns.
						if (fluffPlaceholders[i].createdFluff == null && fluffPlaceholders[i].readyForSpawn && (fluffPlaceholders[i].attachee == null || fluffPlaceholders[i].attachee.root == null || fluffPlaceholders[i].attachee.root.trackStuckFluffs || fluffPlaceholders[i].attachee.stuckFluff == null))
						{
							openPlaceholders.Add(fluffPlaceholders[i]);
						}
					}
				}

				if (spawnAll)
				{
					// Spawn all fluffs in needed.
					for (int i = 0; i < openPlaceholders.Count; i++)
					{
						openPlaceholders[i].SpawnFluff();
					}

				}
				else if (openPlaceholders.Count > 0)
				{
					// If not spawning all fluffs at once, spawn a random fluff.
					int nextFluffSpawn = Random.Range(0, openPlaceholders.Count);
					openPlaceholders[nextFluffSpawn].SpawnFluff();
				}

				// Reset to start of spawn cycle.
				reactionProgress = 0;
			}
		}

		return reacted;
	}

	public override void SetTouchedStreams(int streamsTouched)
	{
		if (this.streamsTouched <= 0)
		{
			if (fluffStickRoot != null)
			{
				actualActionRate = fluffStickRoot.fluffActionRate;
				actualConsumptionRate = fluffStickRoot.fluffConsumeRate;

				fluffStickRoot.fluffActionRate = 0;
				fluffStickRoot.fluffConsumeRate = 0;
			}
		}
		base.SetTouchedStreams(streamsTouched);
	}

	public override void ReactToEmptyFluffStick(FluffStick fluffStick)
	{
		if (reactionProgress >= 1)
		{
			reactionProgress = 0;
		}
	}
}
