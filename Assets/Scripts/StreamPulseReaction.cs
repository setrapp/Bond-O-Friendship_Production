using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StreamPulseReaction : StreamReaction {

	public Vector3 pulsePositionOffset = Vector3.zero;
	public RingPulse optionalPulsePrefab;
	public bool useDefaultStats = true;
	public PulseStats optionalPulseStats;
	private RingPulse createdPulse;

	public override bool React(float actionRate)
	{
		bool reacted = base.React(actionRate);
		if (reacted)
		{
			base.React(actionRate);

			if (reactionProgress >= 1 && actionRate > 0 && createdPulse == null)
			{
				PulseStats pulseStats = optionalPulseStats;
				if (useDefaultStats)
				{
					pulseStats = Globals.Instance.defaultPulseStats;
				}
				createdPulse = Helper.FirePulse(transform.TransformPoint(pulsePositionOffset), pulseStats, optionalPulsePrefab);
			}
		}
		return reacted;
	}
}
