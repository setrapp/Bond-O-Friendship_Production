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
		float startZoom = CameraSplitter.Instance.startPos.z;

		if(Globals.Instance != null && targetZoom != null && targetZoom.updateZoom && startZoom != targetZoom.zoomTarget.transform.position.z)
		{
			float progress = (targetZoom.currentZoom - startZoom) / (targetZoom.zoomTarget.transform.position.z - startZoom);

			Globals.Instance.playerLuminIntensity = ((1 - progress) * Globals.Instance.defaultPlayerLuminIntensity) + (progress * maxLuminIntensity);
		}
	}
}
