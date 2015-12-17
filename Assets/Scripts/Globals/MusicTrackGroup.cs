using UnityEngine;
using System.Collections;

public class MusicTrackGroup : MonoBehaviour {

	public MusicTrack[] tracks;

	void Start()
	{
		if (tracks.Length > 0)
		{
			tracks[0].playable = true;
		}
		for (int i = 0; i < tracks.Length; i++)
		{
			tracks[i].playable = false;
		}
	}

	void Update()
	{

	}
}

public class MusicTrack
{
	public AudioSource audio;
	public bool playable = false;
	public float maxVolume;
}