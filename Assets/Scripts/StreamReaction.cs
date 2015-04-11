using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class StreamReaction : MonoBehaviour {

	public float reactionProgress = 0;
	public float reactionRate = 1;
	public bool reactable = true;
	[SerializeField]
	public List<StreamReaction> superiors;

	void Start()
	{
		if (superiors.Count > 0)
		{
			reactable = false;
		}
	}

	public virtual bool React(float actionRate)
	{
		if (superiors.Count > 0)
		{
			reactable = true;
			for (int i = 0; i < superiors.Count; i++)
			{
				if (superiors[i] != null && superiors[i].reactionProgress < 1)
				{
					reactable = false;
				}
			}
		}

		if (reactionProgress < 1 && reactable)
		{
			reactionProgress = CalculateReaction(actionRate);
			return true;
		}
		return false;
	}

	public float CalculateReaction(float actionRate)
	{
		return Mathf.Clamp01(reactionProgress + actionRate * reactionRate);
	}
}
