using UnityEngine;
using System.Collections;

public class StreamReaction : MonoBehaviour {

	public float reactionProgress = 0;
	public float reactionRate = 1;

	public virtual void React(float actionRate)
	{
		reactionProgress = Mathf.Clamp01(reactionProgress + actionRate * reactionRate);
	}
}
