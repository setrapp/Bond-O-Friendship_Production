using UnityEngine;
using System.Collections;

public class SmartAudioSource : MonoBehaviour {
	public GameObject worldSource;
	public bool addAudioTriggers = true;
	public LayerMask triggerLayers;
	public Vector3 placementOffset;
	public bool moveToCollsion = false;
	public bool playMultiple = false;

	void Awake()
	{
		if (worldSource == null)
		{
			worldSource = transform.parent.gameObject;
		}

		if (addAudioTriggers && worldSource != null)
		{
			AudioSource[] audioSources = GetComponents<AudioSource>();
			for (int i = 0; i < audioSources.Length; i++)
			{
				AudioTrigger audioTrigger = worldSource.AddComponent<AudioTrigger>();
				audioTrigger.audioToPlay = audioSources[i];
				audioTrigger.triggerLayers = triggerLayers;
				audioTrigger.audioMover = this;
				audioTrigger.moveToCollsion = moveToCollsion;
				audioTrigger.playMultiple = playMultiple;
			}
		}
	}

	void Update()
	{
		CameraSplitter cameraController = CameraSplitter.Instance;
		if (worldSource != null && cameraController != null)
		{
			// Calculate the vector from the nearest player to the audio source in the world.

			Vector3 fromListener = worldSource.transform.position - cameraController.player1.transform.position;
			if ((worldSource.transform.position - cameraController.player2.transform.position).sqrMagnitude < fromListener.sqrMagnitude)
			{
				fromListener = worldSource.transform.position - cameraController.player2.transform.position;
			}

			// Move to simulate the vector from the player against the actual audio listener.
			transform.position = cameraController.audioListener.transform.position + fromListener + placementOffset;
		}
	}
}
