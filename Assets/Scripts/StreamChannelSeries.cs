using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StreamChannelSeries : MonoBehaviour {
	public bool renderBeds = true;
	//public bool renderBanks = true;
	public bool maskBeds = true;
	public float bedRenderWidth = 0.5f;
	public Material overlayMaterial;
	public Material maskMaterial;
	public int maskRenderOffset = -1;
	[SerializeField]
	public List<StreamChannel> channels;
	[SerializeField]
	public List<StreamChannelChange> streamChanges;

	void Start()
	{
		for(int i = 0; i < channels.Count; i++)
		{
			if (channels[i] != null)
			{
				if (channels[i].bed != null)
				{
					Renderer bedRenderer = channels[i].bed.GetComponent<Renderer>();
					if (bedRenderer != null)
					{
						bedRenderer.enabled = renderBeds;
					}

					if (maskBeds)
					{
						GameObject bedOverlay = (GameObject)Instantiate(channels[i].bed, channels[i].bed.transform.position, channels[i].bed.transform.rotation);
						bedOverlay.transform.position += new Vector3(0, 0, -0.3f) - new Vector3(0, 0, bedOverlay.transform.position.z);
						Renderer bedOverlayRenderer = bedOverlay.GetComponent<Renderer>();
						if (bedOverlayRenderer != null)
						{
							bedOverlayRenderer.enabled = true;
							bedOverlayRenderer.material = overlayMaterial;
						}

						GameObject bedMask = (GameObject)Instantiate(channels[i].bed, channels[i].bed.transform.position, channels[i].bed.transform.rotation);
						bedMask.transform.position += new Vector3(0, 0, 0.4f) - new Vector3(0, 0, bedMask.transform.position.z);
						Renderer bedMaskRenderer = bedMask.GetComponent<Renderer>();
						if (bedMaskRenderer != null)
						{
							bedMaskRenderer.enabled = true;
							bedMaskRenderer.material = maskMaterial;
							RenderQueue maskQueue = bedMask.AddComponent<RenderQueue>();
							maskQueue.targetRenderer = bedMaskRenderer;
							maskQueue.renderBase = RenderQueue.RenderBase.TRANSPARENT;
							maskQueue.renderOffset = maskRenderOffset;
						}

						bedOverlay.transform.parent = channels[i].bed.transform;
						bedMask.transform.parent = channels[i].bed.transform;
						bedOverlayRenderer.transform.localScale = new Vector3(bedRenderWidth, 1, 1);
						bedMaskRenderer.transform.localScale = new Vector3(bedRenderWidth, 1, 1);
					}
				}
				/*if (channels[i].bank1 != null)
				{
					Renderer bank1Renderer = channels[i].bank1.GetComponent<Renderer>();
					if (bank1Renderer != null)
					{
						bank1Renderer.enabled = renderBanks;
					}
				}
				if (channels[i].bank2 != null)
				{
					Renderer bank2Renderer = channels[i].bank2.GetComponent<Renderer>();
					if (bank2Renderer != null)
					{
						bank2Renderer.enabled = renderBanks;
					}
				}*/
			}
		}
	}

	public StreamChannel[] GetNextChannels(StreamChannel currentChannel)
	{
		List<StreamChannel> nextChannelList = new List<StreamChannel>();

		if (currentChannel == null)
		{
			return null;
		}

		for (int i = 0; i < streamChanges.Count; i++)
		{
			if (streamChanges[i].preChangeChannel == currentChannel && streamChanges[i].nextSeries != null && streamChanges[i].nextSeries.channels.Count > 0)
			{
				if (streamChanges[i].postChangeChannel == null || streamChanges[i].postChangeChannel.parentSeries != streamChanges[i].nextSeries)
				{
					nextChannelList.Add(streamChanges[i].nextSeries.channels[0]);
				}
				else
				{
					nextChannelList.Add(streamChanges[i].postChangeChannel);
				}
			}
		}

		int currentIndex = channels.IndexOf(currentChannel);
		if (currentIndex >= 0 && currentIndex < channels.Count - 1 && channels[currentIndex + 1] != null)
		{
			nextChannelList.Add(channels[currentIndex + 1]);
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
	public StreamChannel postChangeChannel;
}
