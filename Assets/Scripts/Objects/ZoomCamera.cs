using UnityEngine;
using System.Collections;

public class ZoomCamera : MonoBehaviour {

	public WaitPad triggerPad;
	private Camera mainCamera;
	private Camera splitCamera;
	private float startZoom;
	public float zoomTarget;
	public float startZoomPortion = 0;
	public float endZoomPortion = 1;
	private float oldPortionComplete;
	[Header("Resetting Controls")]
	public float resetDelay = 0.5f;
	public float resetAcceleration = 1;
	public float resetMaxSpeed = 5;
	public float resetSpeed = 0f;
	private bool resetting = false;

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
		if (endZoomPortion <= 0)
		{
			return;
		}

		if (triggerPad.activated && !resetting && oldPortionComplete > 0)
		{
			StartCoroutine(ResetToStart());
		}

		bool updateZoom = resetting;


		if (!triggerPad.activated && oldPortionComplete != triggerPad.portionComplete)
		{
			oldPortionComplete = triggerPad.portionComplete;
			updateZoom = true;
		}

		if (updateZoom)
		{
			float alterPortionComplete = Mathf.Clamp01((oldPortionComplete - startZoomPortion) / (endZoomPortion - startZoomPortion));
			float zoom = (startZoom * (1 - alterPortionComplete)) + (zoomTarget * alterPortionComplete);
			mainCamera.orthographicSize = zoom;
			splitCamera.orthographicSize = zoom;
		}
	}

	public IEnumerator ResetToStart()
	{
		resetting = true;
		yield return new WaitForSeconds(resetDelay);
		oldPortionComplete = endZoomPortion;

		while (oldPortionComplete > 0)
		{
			resetSpeed += resetAcceleration * Time.deltaTime;
			if (resetSpeed > resetMaxSpeed)
			{
				resetSpeed = resetMaxSpeed;
			}

			float backZoom = resetSpeed * Time.deltaTime;
			if (oldPortionComplete - backZoom < 0)
			{
				oldPortionComplete = 0;
			}
			else
			{
				oldPortionComplete -= backZoom;
				
			}

			yield return null;
		}
		resetting = false;
	}
}
