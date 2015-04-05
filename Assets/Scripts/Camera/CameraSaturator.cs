using UnityEngine;
using System.Collections;

public class CameraSaturator : MonoBehaviour {
	public TargetCamera targetCameraType;
	public WaitPad trigger;
	private bool used = false;
	public bool enableSaturationAlter = true;

	public void Update()
	{
		if (CameraSplitter.Instance == null || used)
		{
			return;
		}

		Camera targetCamera = CameraSplitter.Instance.splitCamera1;
		if (targetCameraType == TargetCamera.PLAYER_1)
		{
			targetCamera = CameraSplitter.Instance.splitCamera1;
		}
		else if (targetCameraType == TargetCamera.PLAYER_2)
		{
			targetCamera = CameraSplitter.Instance.splitCamera2;
		}

		if (targetCamera == null || trigger == null || !trigger.activated || used)
		{
			return;
		}

		ColorCorrectionCurves colorCorrection = targetCamera.GetComponent<ColorCorrectionCurves>();
		if (colorCorrection != null)
		{
			colorCorrection.enabled = enableSaturationAlter;
		}

		used = true;
	}

	public enum TargetCamera
	{
		COMBINED = 0,
		PLAYER_1,
		PLAYER_2
	}
}
