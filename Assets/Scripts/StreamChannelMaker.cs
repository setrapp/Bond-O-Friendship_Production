using UnityEngine;
using System.Collections;

public class StreamChannelMaker : MonoBehaviour {

	public StreamChannel streamChannelPrefab;
	public StreamChannelSeries channelSeriesPrefab;
	public Tracer guide;
	public float bedWidth = 1;
	public float bankWidth = 1;
	public float turningAngle = 45;

	private StreamChannel newChannel = null;
	private StreamChannelSeries channelSeries = null;
	private int recentGuideIndex = 0;
	private int nextGuideIndex = 1;
	


	void Start()
	{
		if (guide == null)
		{
			guide = GetComponent<Tracer>();
		}

		guide.CreateLineMaker(true);
		StartChannelSeries();

		MakeChannels();
	}

	void Update()
	{
		if (Input.GetMouseButtonUp(1))
		{
			StartChannelSeries();
		}

		MakeChannels();
	}

	void MakeChannels()
	{
		if (recentGuideIndex < 0 || recentGuideIndex > guide.GetVertexCount() - 3)
		{
			return;
		}

		float turningDot = Mathf.Cos(turningAngle / 2 * Mathf.Deg2Rad);

		if (nextGuideIndex <= recentGuideIndex)
		{
			nextGuideIndex = recentGuideIndex + 1;
		}

		Vector3 recentVertex = guide.GetVertex(recentGuideIndex);

		if (newChannel == null)
		{
			SpawnChannel(recentVertex);
		}

		//while(recentGuideIndex < guide.GetVertexCount() - 1)
		while (nextGuideIndex < guide.GetVertexCount() - 1)
		{
			Vector3 nextVertex = guide.GetVertex(nextGuideIndex);

			Vector3 toNext = nextVertex - recentVertex;
			float toNextDist = toNext.magnitude;
			toNext /= toNextDist;

			Vector3 prospectiveVertex =  guide.GetVertex(nextGuideIndex + 1);
			Vector3 toProspective = (prospectiveVertex - nextVertex).normalized;


			if (toProspective.sqrMagnitude > 0)
			{
				if (!newChannel.gameObject.activeSelf)
				{
					newChannel.gameObject.SetActive(true);
				}

				newChannel.bed.transform.localScale = new Vector3(bedWidth, toNextDist, newChannel.bed.transform.localScale.z);
				newChannel.bank1.transform.localScale = new Vector3(bankWidth, toNextDist, newChannel.bank1.transform.localScale.z);
				newChannel.bank2.transform.localScale = new Vector3(bankWidth, toNextDist, newChannel.bank2.transform.localScale.z);
				
				float bankOffset = (bedWidth / 2) + (bankWidth / 2);
				newChannel.bed.transform.localPosition = new Vector3(0, newChannel.bed.transform.localPosition.y, toNextDist / 2);
				newChannel.bank1.transform.localPosition = new Vector3(bankOffset, newChannel.bank1.transform.localPosition.y, toNextDist / 2);
				newChannel.bank2.transform.localPosition = new Vector3(-bankOffset, newChannel.bank1.transform.localPosition.y, toNextDist / 2);
					
				newChannel.transform.LookAt(nextVertex, Vector3.forward);

				if (Vector3.Dot(toNext, toProspective) < turningDot)
				{
					recentGuideIndex = nextGuideIndex;
					recentVertex = nextVertex;
					
					SpawnChannel(recentVertex);
				}	
			}

			nextGuideIndex++;
		}
	}

	private void SpawnChannel(Vector3 recentVertex)
	{
		newChannel = ((GameObject)Instantiate(streamChannelPrefab.gameObject, recentVertex, Quaternion.identity)).GetComponent<StreamChannel>();
		newChannel.gameObject.SetActive(false);
		if (channelSeries != null)
		{
			newChannel.transform.parent = channelSeries.transform;
			newChannel.parentSeries = channelSeries;
			channelSeries.channels.Add(newChannel);
		}
	}

	private void StartChannelSeries()
	{
		if (channelSeriesPrefab != null)
		{
			StreamChannelSeries newChannelSeries = ((GameObject)Instantiate(channelSeriesPrefab.gameObject, transform.position, Quaternion.identity)).GetComponent<StreamChannelSeries>();

			if (channelSeries != null && channelSeries.channels.Count > 0)
			{
				StreamChannelChange streamChange = new StreamChannelChange();
				streamChange.preChangeChannel = channelSeries.channels[channelSeries.channels.Count - 1];
				streamChange.nextSeries = newChannelSeries;
				channelSeries.streamChanges.Add(streamChange);
			}

			channelSeries = newChannelSeries;
		}
	}
}
