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

		if (group.nextNote >= 0 && group.nextNote < group.audioClips.Length && group.audioClips[group.nextNote] != null)
		{
			if (!played)
			{
				audio.clip = group.audioClips[group.nextNote];

				if ((group.nextNote == 0 && group.noteDirection < 0) || (group.nextNote == group.audioClips.Length - 1  && group.noteDirection > 0))
				{
					group.noteDirection *= -1;
				}

				group.nextNote += group.noteDirection;

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
			group.nextNote = Mathf.Max(0, group.nextNote - group.noteDirection);
			played = false;
		}
	}
}
