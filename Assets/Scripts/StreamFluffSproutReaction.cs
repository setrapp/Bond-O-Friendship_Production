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
				bool spawned = false;
				for (int i = 0; i < fluffPlaceholders.Count; i++)
				{
					if (fluffPlaceholders[i] == null)
					{
						fluffPlaceholders.RemoveAt(i);
						i--;
					}
					else if (spawnAll || !spawned)
					{
						if (fluffPlaceholders[i].createdFluff == null && (fluffPlaceholders[i].attachee == null || fluffPlaceholders[i].attachee.root == null || fluffPlaceholders[i].attachee.root.trackStuckFluffs || fluffPlaceholders[i].attachee.stuckFluff == null))
						{
							fluffPlaceholders[i].SpawnFluff();
							spawned = true;
						}
					}
				}

				if (!spawnAll)
				{
					/*while (!spawned)
					{
						int i = Random.Range(0, fluffPlaceholders.Count);
						if (fluffPlaceholders[i].createdFluff == null && (fluffPlaceholders[i].attachee == null || fluffPlaceholders[i].attachee.root == null || fluffPlaceholders[i].attachee.root.trackStuckFluffs || fluffPlaceholders[i].attachee.stuckFluff == null))
						{
							fluffPlaceholders[i].SpawnFluff();
							spawned = true;
						}
					}*/
				}

				//if (spawned)
				//{
					reactionProgress = 0;
				//}
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
