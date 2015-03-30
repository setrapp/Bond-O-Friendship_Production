using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StreamChannelSeries : MonoBehaviour {
	public bool renderBeds = true;
	public bool renderBanks = true;
	[SerializeField]
	public List<StreamChannel> channels;
	[SerializeField]
	public List<StreamChannelChange> streamChanges;

	void Start()
	{
		for(int i = 0; i < channels.Count; i++)
		{
			Renderer bedRenderer = channels[i].bed.GetComponent<Renderer>();
			if (bedRenderer != null)
			{
				bedRenderer.enabled = renderBeds;
			}
			Renderer bank1Renderer = channels[i].bank1.GetComponent<Renderer>();
			if (bank1Renderer != null)
			{
				bank1Renderer.enabled = renderBanks;
			}
			Renderer bank2Renderer = channels[i].bank2.GetComponent<Renderer>();
			if (bank2Renderer != null)
			{
				bank2Renderer.enabled = renderBanks;
			}
		}
	}

	public StreamChannel[] GetNextChannels(StreamChannel currentChannel)
	{
		List<StreamChannel> nextChannelList = new List<StreamChannel>();

		int currentIndex = channels.IndexOf(currentChannel);
		if (currentIndex >= 0 && currentIndex < channels.Count - 1)
		{
			nextChannelList.Add(channels[currentIndex + 1]);
		}


		for (int i = 0; i < streamChanges.Count; i++)
		{
			if (streamChanges[i].preChangeChannel == currentChannel && streamChanges[i].nextSeries.channels.Count > 0)
			{
				nextChannelList.Add(streamChanges[i].nextSeries.channels[0]);
			}
		}

		if (nextChannelList.Count <= 0)
		{
			return null;
		}

		StreamChannel[] nextChannels = new StreamChannel[nextChannelList.Count];
		for (int i = 0; i < nextChannels.Length; i++)
		{
			nextChannels[i] = nextChannelList[i];
		}

		return nextChannels;
	}
}

[System.Serializable]
public class StreamChannelChange
{
	public StreamChannel preChangeChannel;
	public StreamChannelSeries nextSeries;
}
