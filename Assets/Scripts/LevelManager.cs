using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {
	public bool debugEvent = false;
	private bool evented = false;
	public PartnerLink player;
	public TriggerLooping boundary;
	public float worldWidth;
	public float worldHeight;
	public Vector2 worldOffset;
	[SerializeField]
	public List<GameObject> enableObjects;
	[SerializeField]
	public List<GameObject> disableObjects;

	void Update()
	{
		if (debugEvent)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				LevelEvent();
			}
		}
	}

	public void LevelEvent()
	{
		if (evented)
		{
			return;
		}

		evented = true;

		// If debugging unlink player partner.
		if (/*debugEvent && player != null && */player.Partner != null)
		{
			WaypointSeek partnerSeek = player.Partner.GetComponent<WaypointSeek>();
			if (partnerSeek != null)
			{
				partnerSeek.moveWithoutPartner = true;
			}
			player.Partner.seekingPartner = false;
			//player.Partner.fading = true;
			ConversationManager.Instance.EndConversation(player, player.Partner);
		}

		// Update the looping boundary.
		boundary.ChangeWorldSize(worldWidth, worldHeight);
		boundary.transform.Translate(worldOffset);

		// Enable specified objects for looping.
		for (int i = 0; i < enableObjects.Count; i++)
		{
			LoopTag loopTag = enableObjects[i].GetComponent<LoopTag>();
			if (loopTag)
			{
				loopTag.stayOutsideBounds = false;
				loopTag.passThrough = false;
				loopTag.trackObject = true;
			}
			WaypointSeek waypointSeek = enableObjects[i].GetComponent<WaypointSeek>();
			if (waypointSeek)
			{
				waypointSeek.moveWithoutPartner = true;
			}
		}

		// Disable specified object for looping.
		for (int i = 0; i < disableObjects.Count; i++)
		{
			LoopTag loopTag = disableObjects[i].GetComponent<LoopTag>();
			if (loopTag)
			{
				loopTag.stayOutsideBounds = true;
				loopTag.passThrough = true;
				loopTag.trackObject = false;
			}
		}
	}
}
