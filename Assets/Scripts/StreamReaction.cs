using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class StreamReaction : MonoBehaviour {

	public float reactionProgress = 0;
	public float reactionRate = 1;
	public float decayRate = 0;
	public float decayDelay = 0;
	protected float lastReaction = 0;
	public bool reactable = true;
	[SerializeField]
	public List<StreamReaction> superiors;
	public int streamsTouched = 0;
	public bool stopReactionOnComplete = false;

	virtual protected void Start()
	{
		if (superiors.Count > 0)
		{
			reactable = false;
		}
	}

	virtual protected void Update()
	{
		if (decayRate < 0)
		{
			decayRate = reactionRate;
		}

		if (Time.time - lastReaction >= decayDelay)
		{
			if (reactionProgress > 0)
			{
				React(-decayRate * Time.deltaTime);
			}
		}
	}

	public virtual bool React(float actionRate)
	{
		if (superiors.Count > 0)
		{
			reactable = true;
			for (int i = 0; i < superiors.Count; i++)
			{
				if (superiors[i] != null && superiors[i].isActiveAndEnabled && superiors[i].reactionProgress < 1)
				{
					reactable = false;
				}
			}
		}

		if (reactable && enabled && gameObject.activeInHierarchy)
		{
			bool reacted = (actionRate >= 0 && reactionProgress < 1) || (actionRate < 0 && reactionProgress > 0);
			reactionProgress = CalculateReaction(actionRate);

			if (actionRate > 0)
			{
				lastReaction = Time.time;
			}

			if (reactionProgress >= 1 && stopReactionOnComplete)
			{
				Destroy(this);
			}

			return reacted;
		}
		return false;
	}

	public virtual void SetTouchedStreams(int streamsTouched)
	{
		this.streamsTouched = Mathf.Max(streamsTouched, 0);
	}

	public float CalculateReaction(float actionRate)
	{
		return Mathf.Clamp01(reactionProgress + actionRate * reactionRate);
	}

	public virtual void ReactToEmptyFluffStick(FluffStick fluffStick) { }
}
