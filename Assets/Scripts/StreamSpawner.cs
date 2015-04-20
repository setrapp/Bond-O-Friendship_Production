using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StreamSpawner : MonoBehaviour {

	public Stream streamPrefab;
	public StreamChannelSeries startingChannelSeries;
	public StreamChannel startingChannel;
	public float spawnTime = 5;
	public float destroyTimeFactor = 2;
	public int streamsPerSpawn = 3;
	public int maxStreams = 100;
	public float seekRandomizeRange = 1;
	public int streamMoveBatch = 3;
	public float streamGroupActionRate = 1;
	public List<Stream> streams;
	private float lastSpawnTime = -1;
	private int moveBatchIndex = 0;

	void Start()
	{
		if (startingChannel == null && startingChannelSeries != null && startingChannelSeries.channels.Count > 0)
		{
			startingChannel = startingChannelSeries.channels[0];
		}
	}

	void Update()
	{
		if (streamPrefab != null && streamsPerSpawn > 0)
		{
			if (lastSpawnTime < 0 || (Time.time - lastSpawnTime >= spawnTime && spawnTime >= 0))
			{
				lastSpawnTime = Time.time;
				for (int i = 0; i < streamsPerSpawn; i++)
				{
					Stream newStream = ((GameObject)Instantiate(streamPrefab.gameObject, transform.position, transform.rotation)).GetComponent<Stream>();
					newStream.targetChannel = startingChannel;
					newStream.spawner = this;
					newStream.seekOffset = new Vector3(Random.Range(-seekRandomizeRange, seekRandomizeRange), Random.Range(-seekRandomizeRange, seekRandomizeRange), 0);
					newStream.actionRate = streamGroupActionRate / streamsPerSpawn;
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
		/*if (streams.Count > 0)
		{
			bool checkedAllStreams = false;
			for (int i = 0; i < streamMoveBatch && !checkedAllStreams; i++)
			{
				int checkIndex = (moveBatchIndex + i) % streams.Count;
				Stream moveeStream = streams[checkIndex];

				// Only attempt to update streams that are actively moving forward.
				if (!moveeStream.ending)
				{
					moveeStream.UpdateMovement();
				}
				else
				{
					i--;
				}

				// Allow the loop to end if all indices have been seen.
				if (checkIndex == moveBatchIndex - 1)
				{
					checkedAllStreams = true;
				}
				
			}
			moveBatchIndex = (moveBatchIndex + streamMoveBatch) % streams.Count;
		}*/
	}

	public void TrackStream(Stream newStream)
	{
		while (streams.Count + 1 > maxStreams)
		{
			int removeIndex = Random.Range(0, streams.Count - 1);
			Stream removeStream = streams[removeIndex];
			streams.RemoveAt(removeIndex);
			Destroy(removeStream.gameObject);
		}
		streams.Add(newStream);
		newStream.transform.parent = transform;
	}

	public void StopTrackingStream(Stream stream)
	{
		streams.Remove(stream);
	}
}
