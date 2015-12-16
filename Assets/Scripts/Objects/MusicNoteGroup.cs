using UnityEngine;
using System.Collections;

public class MusicNoteGroup : MonoBehaviour {

	public MusicNote[] notes;
	public AudioClip[] audioClips;
	public int nextNote = 0;

	public void Start()
	{
		notes = GetComponentsInChildren<MusicNote>();

		if (notes.Length > audioClips.Length)
		{
			Debug.LogError(gameObject.name + " does not contain enough audio clips to play all notes.");
		}

		for (int i = 0; i < notes.Length; i++)
		{
			notes[i].group = this;
		}
	}
}
