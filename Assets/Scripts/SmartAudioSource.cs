using UnityEngine;
using System.Collections;

public class SmartAudioSource : MonoBehaviour {
	public GameObject worldSource;
	
	void Update()
	{
		if (worldSource != null)
		{
			// Calculate the vector from the nearest player to the audio source in the world.
			CameraSplitter cameraController = CameraSplitter.Instance;
			Vector3 fromListener = worldSource.transform.position - cameraController.player1.transform.position;
			if ((worldSource.transform.position - cameraController.player2.transform.position).sqrMagnitude < fromListener.sqrMagnitude)
			{
				fromListener = worldSource.transform.position - cameraController.player2.transform.position;
			}

			// Move to simulate the vector from the player against the actual audio listener.
			transform.position = cameraController.audioListener.transform.position + fromListener;
		}
	}
}
