using UnityEngine;
using System.Collections;

public class StreamConnectTrigger : MonoBehaviour {

	public GameObject connectionPartner;
	public float connectionDistance = 0.2f;
	public StreamChannelChange channelChange;
	public float waitTime = 0;
	private bool connected;

	void Update()
	{
		if (connectionPartner != null && (connectionPartner.transform.position - transform.position).sqrMagnitude <= Mathf.Pow(connectionDistance, 2) && ! connected)
		{
			StartCoroutine(ConnectStreamChannels());
		}
	}

	private IEnumerator ConnectStreamChannels()
	{
		if (channelChange != null && channelChange.preChangeChannel != null && channelChange.nextSeries != null)
		{
			connected = true;
			yield return new WaitForSeconds(waitTime);

			channelChange.preChangeChannel.parentSeries.streamChanges.Add(channelChange);
			
		}
	}
}
