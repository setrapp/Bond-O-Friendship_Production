using UnityEngine;
using System.Collections;

public class ZoomCamera : MonoBehaviour {

	public WaitPad triggerPad;
	private Camera mainCamera;
	private Camera splitCamera;
	private float startZoom;
	public float zoomTarget;
	public float reachAtPortion = 1;
	private float oldPortionComplete;

	void Start()
	{
		if (CameraSplitter.Instance != null)
		{
			mainCamera = CameraSplitter.Instance.splitCamera1;
			splitCamera = CameraSplitter.Instance.splitCamera2;
			startZoom = mainCamera.orthographicSize;
		}
	}

	void Update()
	{
		if (reachAtPortion <= 0)
		{
			return;
		}

		if (oldPortionComplete != triggerPad.portionComplete)
		{
			float alterPortionComplete = triggerPad.portionComplete / reachAtPortion;
			float zoom = (startZoom * ) + (zoomTarget * triggerPad.portionComplete);
			mainCamera.orthographicSize = zoom;
			splitCamera.orthographicSize = zoom;

			oldPortionComplete = triggerPad.portionComplete;
		}
	}
}
