using UnityEngine;
using System.Collections;

public class ZoomCamera : MonoBehaviour {

	public WaitPad triggerPad;
	private Camera mainCamera;
	private Camera splitCamera;
	//public float startZoom;
	public GameObject zoomTarget;
	public bool centerOnTarget = true;
	public float startZoomPortion = 0;
	public float endZoomPortion = 1;
	public float oldPortionComplete;
	public float currentZoom = 0;
	[HideInInspector]
	public bool updateZoom = false;
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
			//startZoom = CameraSplitter.Instance.transform.position.z;//mainCamera.fieldOfView;
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

		if (Globals.Instance.gameState != Globals.GameState.Unpaused)
		{
			return;
		}

		if (endZoomPortion <= 0)
		{
			return;
		}

		if (resetOnComplete && triggerPad.activated && !resetting && oldPortionComplete > 0)
		{
			StartCoroutine(ResetToStart());
		}

		updateZoom = resetting;

		/*if (triggerPad.portionComplete > oldPortionComplete)
		{
			CameraSplitter.Instance.followPlayers = false;
		}
		else if (triggerPad.portionComplete < oldPortionComplete)
		{
			CameraSplitter.Instance.followPlayers = true;
		}*/

		if (!triggerPad.activated && oldPortionComplete != triggerPad.portionComplete)
		{
			oldPortionComplete = triggerPad.portionComplete;
			updateZoom = true;
		}

		float alterPortionComplete = oldPortionComplete;
		if (updateZoom)
		{
			float startZoom = CameraSplitter.Instance.startPos.z;
			float targetZoomZ = zoomTarget.transform.position.z;

			// Determine how much progress has been made based on start and end parameters.
			alterPortionComplete = Mathf.Clamp01((oldPortionComplete - startZoomPortion) / (endZoomPortion - startZoomPortion));
			currentZoom = (startZoom * (1 - alterPortionComplete)) + (targetZoomZ * alterPortionComplete);

			// Change the camera system's depth to zoom the view.
			Vector3 zoomedPos = CameraSplitter.Instance.transform.position;
			zoomedPos.z = currentZoom;
			CameraSplitter.Instance.transform.position = zoomedPos;
		}

		// Interpolate xy positions of cameras between player center and target focal point.
		if (alterPortionComplete > 0 && centerOnTarget)
		{
			Vector3 focusOffset = zoomTarget.transform.position - ((Globals.Instance.Player1.transform.position + Globals.Instance.Player2.transform.position) / 2);
			CameraSplitter.Instance.mainCameraFollow.centerOffset = ((focusOffset * alterPortionComplete) + CameraSplitter.Instance.mainCameraFollow.centerOffset) / 2;
			CameraSplitter.Instance.splitCameraFollow.centerOffset = ((focusOffset * alterPortionComplete) + CameraSplitter.Instance.splitCameraFollow.centerOffset) / 2;
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
