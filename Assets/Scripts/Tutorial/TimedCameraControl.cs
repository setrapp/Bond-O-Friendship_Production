using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimedCameraControl : MonoBehaviour 
{
	[SerializeField]
	public List<TimedCameraTarget> controls;
	public bool readyToControl = true;

	public void InitiateCameraControl()
	{
		if (readyToControl)
		{
			readyToControl = false;
			StartCoroutine(ProcessControl());
		}
	}

	private IEnumerator ProcessControl()
	{
		for (int i = 0; i < controls.Count; i++)
		{

			// If zooming to default, use the starting position of the camera system.
			if (controls[i].zoomToDefault)
			{
				controls[i].targetZoom = CameraSplitter.Instance.startPos.z;
			}

			// Approach zoom target.
			float elapsedTime = 0;
			float zoomRate = controls[i].targetZoom - CameraSplitter.Instance.transform.position.z;
			if (controls[i].changeDuration > 0)
			{
				zoomRate /= controls[i].changeDuration;
			}
			else
			{
				CameraSplitter.Instance.transform.position += new Vector3(0, 0, zoomRate);
			}
			while(elapsedTime < controls[i].changeDuration)
			{
				elapsedTime += Time.deltaTime;
				CameraSplitter.Instance.transform.position += new Vector3(0, 0, zoomRate * Time.deltaTime);
				yield return null;
			}

			// Wait before applying next control.
			elapsedTime = 0;
			while(elapsedTime < controls[i].postChangeWait)
			{
				elapsedTime += Time.deltaTime;
				yield return null;
			}
		}
	}
}

[System.Serializable]
public class TimedCameraTarget
{
	public float targetZoom;
	public float changeDuration;
	public float postChangeWait;
	public bool zoomToDefault = false;
}
