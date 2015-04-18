using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StreamSpawner : MonoBehaviour {

	public Stream streamPrefab;
	public StreamChannelSeries startingChannelSeries;
	public StreamChannel startingChannel;
	public float spawnTime = 5;
	public int streamsPerSpawn = 3;
	public int maxStreams = 100;
	public float seekRandomizeRange = 1;
	public int streamMoveBatch = 3;
	public List<Stream> streams;
	private float lastSpawnTime = -1;

	void Start()
	{
		if (startingChannel == null && startingChannelSeries != null && startingChannelSeries.channels.Count > 0)
		{
			startingChannel = startingChannelSeries.channels[0];
		}
	}

	void Update()
	{
		if (streamPrefab != null)
		{
			if (lastSpawnTime < 0 || (Time.time - lastSpawnTime >= spawnTime))
			{
				lastSpawnTime = Time.time;
				for (int i = 0; i < streamsPerSpawn; i++)
				{
					Stream newStream = ((GameObject)Instantiate(streamPrefab.gameObject, transform.position, transform.rotation)).GetComponent<Stream>();
					newStream.targetChannel = startingChannel;
					newStream.spawner = this;
					newStream.seekOffset = new Vector3(Random.Range(-seekRandomizeRange, seekRandomizeRange), Random.Range(-seekRandomizeRange, seekRandomizeRange), 0);
					newStream.autoMove = false;
					TrackStream(newStream);
				}
			}
		}

		for (int i = 0; i < streams.Count; i++)
		{
			if (streams[i] == null)
			{
				streams.RemoveAt(i);
				i--;
			}
		}

		// Only direct upto a specified number of streams, in order to cut down on processing. Depending on the random generation, a stream may be directed more than once in a frame, but it will only move once.
		for (int i = 0; i < streamMoveBatch; i++)
		{
			Stream moveeStream = streams[Random.Range(0, streams.Count - 1)];
			moveeStream.UpdateMovement();
		}
	}

	public void TrackStream(Stream newStream)
	{
		while (streams.Count + 1 > maxStreams)
		{
			streams.RemoveAt(streams.Count / 2);
		}
		streams.Add(newStream);
		newStream.transform.parent = transform;
	}
}
