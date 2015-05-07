using UnityEngine;
using System.Collections;

public class StreamBody : MonoBehaviour {

	public float actionRate = 1;
	public LayerMask ignoreReactionLayers;

	public void ProvokeReaction(StreamReaction reaction)
	{
		if (reaction != null)
		{
			reaction.React(actionRate * Time.deltaTime);
		}
	}
}
