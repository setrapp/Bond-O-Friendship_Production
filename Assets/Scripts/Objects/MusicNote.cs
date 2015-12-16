using UnityEngine;
using System.Collections;

public class MusicNote : MonoBehaviour {

	public MusicNoteGroup group;
	public AudioSource audio;
	private bool played = false;

	public void PlayNote()
	{
		if (group == null || audio == null)
		{
			return;
		}

		if (group.nextNote < group.audioClips.Length && group.audioClips[group.nextNote] != null)
		{
			if (!played)
			{
				audio.clip = group.audioClips[group.nextNote++];
				if (audio.clip != null)
				{
					audio.Play();
				}
				played = true;
			}

		}
		else
		{
			Debug.LogError(gameObject.name + " is attempting to play the clip from " + group.gameObject.name + " at index " + group.nextNote + ", which does not exist"); 
		}
	}

	public void UnplayNote()
	{
		if (played && group != null)
		{
			group.nextNote = Mathf.Max(0, group.nextNote - 1);
			played = false;
		}
	}
}
