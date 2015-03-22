using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StreamChannelSeries : MonoBehaviour {
	[SerializeField]
	public List<StreamChannel> channels;
	[SerializeField]
	public List<StreamChannelChange> streamChanges;
	
}

[System.Serializable]
public class StreamChannelChange
{
	public StreamChannel preChangeChannel;
	public StreamChannelSeries nextSeries;
}
