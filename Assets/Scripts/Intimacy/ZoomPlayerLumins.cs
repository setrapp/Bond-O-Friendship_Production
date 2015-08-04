using UnityEngine;
using System.Collections;

public class ZoomPlayerLumins : MonoBehaviour {

	public ZoomCamera targetZoom;
	public float maxLuminIntensity = 1;

	void Awake()
	{
		if (targetZoom == null)
		{
			targetZoom = GetComponent<ZoomCamera>();
		}
	}

	void Update()
	{
		if(Globals.Instance != null && targetZoom != null && targetZoom.updateZoom && targetZoom.startZoom != targetZoom.zoomTarget)
		{
			float progress = (targetZoom.currentZoom - targetZoom.startZoom) / (targetZoom.zoomTarget - targetZoom.startZoom);

			Globals.Instance.playerLuminIntensity = ((1 - progress) * Globals.Instance.defaultPlayerLuminIntensity) + (progress * maxLuminIntensity);
		}
	}
}
