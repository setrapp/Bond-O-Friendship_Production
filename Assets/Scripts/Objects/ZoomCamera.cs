using UnityEngine;
using System.Collections;

public class ZoomCamera : MonoBehaviour {

	public WaitPad triggerPad;
	private Camera mainCamera;
	private Camera splitCamera;
	public float startZoom;
	public float zoomTarget;
	public float startZoomPortion = 0;
	public float endZoomPortion = 1;
	public float oldPortionComplete;
	[Header("Resetting Controls")]
	public bool resetOnComplete = true;
	public float resetDelay = 0.5f;
	public float resetAcceleration = 1;
	public float resetMaxSpeed = 5;
	public float resetSpeed = 0f;
	private bool resetting = false;
	[Header("Starting Zoom Only")]
	//public bool zoomToStartingSize = false;
	private bool foundStartingZoom = false;

	void Start()
	{
		if (CameraSplitter.Instance != null)
		{
			mainCamera = CameraSplitter.Instance.splitCamera1;
			splitCamera = CameraSplitter.Instance.splitCamera2;
			startZoom = mainCamera.fieldOfView;
			/*if (zoomToStartingSize)
			{
				zoomTarget = Globals.Instance.startingOrthographicSize;
			}
			else
			{*/
				foundStartingZoom = true;
			//}
		}
	}

	void Update()
	{
		// Wait to find starting zoom for special starting zoomer.
		/*if (zoomToStartingSize)
		{
			if (!CameraSplitter.Instance.startZoomComplete)
			{
				return;
			}
			else if (!foundStartingZoom)
			{
				startZoom = mainCamera.orthographicSize;
				foundStartingZoom = true;
			}
		}*/

		if (endZoomPortion <= 0)
		{
			return;
		}

		if (resetOnComplete && triggerPad.activated && !resetting && oldPortionComplete > 0)
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
			mainCamera.fieldOfView = zoom;
			splitCamera.fieldOfView = zoom;
			Globals.Instance.perspectiveFOV = zoom;
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
