using UnityEngine;
using System.Collections;

public class CameraSwapper : MonoBehaviour {
	public WaitPad trigger;
	public SwapBasis swapBasedOn;
	private bool used = false;

	public void Update()
	{
		if (trigger == null || !trigger.activated || used)
		{
			return;
		}

		if (CameraSplitter.Instance == null || CameraSplitter.Instance.mainCameraFollow == null || CameraSplitter.Instance.splitCameraFollow == null)
		{
			return;
		}

		CameraFollow camera1 = CameraSplitter.Instance.mainCameraFollow;
		CameraFollow camera2 = CameraSplitter.Instance.splitCameraFollow;
		Transform cameraPlayer1 = camera1.player1;
		Transform cameraPlayer2 = camera1.player2;

		bool swap = false;
		switch (swapBasedOn)
		{
			case SwapBasis.PLAYERS_X:
				if (cameraPlayer2.localPosition.x > cameraPlayer1.localPosition.x)
				{
					swap = true;
				}
				break;
			case SwapBasis.PLAYERS_Y:
				if (cameraPlayer2.localPosition.y > cameraPlayer1.localPosition.y)
				{
					swap = true;
				}
				break;
			default:
				if (cameraPlayer1 != Globals.Instance.Player1.transform)
				{
					swap = true;
				}
				break;
		}

		if (swap)
		{
			CameraSplitter.Instance.mainCameraFollow = camera2;
			CameraSplitter.Instance.splitCameraFollow = camera1;
			camera1.player1 = cameraPlayer2;
			camera1.player2 = cameraPlayer1;
			camera2.player1 = cameraPlayer1;
			camera2.player2 = cameraPlayer2;
		}

		used = true;
	}

	public enum SwapBasis
	{
		DEFAULT = 0,
		PLAYERS_X,
		PLAYERS_Y
	}
}
