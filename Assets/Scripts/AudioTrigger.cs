using UnityEngine;
using System.Collections;

public class AudioTrigger : MonoBehaviour {

	public AudioSource audioToPlay;
	public LayerMask triggerLayers;
	public SmartAudioSource audioMover;
	public bool moveToCollsion = false;
	public bool playMultiple = true;

	void Start()
	{
		if (audioToPlay == null)
		{
			audioToPlay = GetComponent<AudioSource>();
		}
	}

	private void PlayAudio(Vector3 position)
	{
		if (audioToPlay != null && audioToPlay.isActiveAndEnabled && (!audioToPlay.isPlaying || playMultiple))
		{
			if (moveToCollsion && audioMover != null)
			{
				audioMover.placementOffset = position - transform.position;
			}

			audioToPlay.Play();
		}
	}


	void OnCollisionEnter(Collision col)
	{
		int layer = (int)Mathf.Pow(2, col.collider.gameObject.layer);
		if ((layer & triggerLayers) == layer)
		{
			PlayAudio(col.collider.transform.position);
		}
	}

	void OnTriggerEnter(Collider col)
	{
		int layer = (int)Mathf.Pow(2, col.gameObject.layer);
		if ((layer & triggerLayers) == layer)
		{
			PlayAudio(col.GetComponent<Collider>().transform.position);
		}
	}
}
