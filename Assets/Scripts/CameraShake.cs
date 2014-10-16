using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {
	private bool shaking = false;
	private float shakeAmount = 0;
	public Vector3 stableLocalPosition;

	//For Testing:


	/*public bool shakingMain = false;
	
	// Update is called once per frame
	void Update () {

		ShakeCamera(shakingMain, .7f);
	
	}*/

	void Start()
	{
		stableLocalPosition = transform.localPosition;
	}

	void Update()
	{
		if (shaking)
		{
			camera.transform.localPosition = stableLocalPosition + Random.insideUnitSphere * shakeAmount;
			camera.transform.localPosition = new Vector3(camera.transform.localPosition.x, camera.transform.localPosition.y, stableLocalPosition.z);
		}
	}

	public void ShakeCamera(float shakeAmount)
	{
		shaking = true;
		this.shakeAmount = shakeAmount;
		stableLocalPosition = transform.localPosition;
	}

	public void StopShaking()
	{
		shaking = false;
		shakeAmount = 0;
		transform.localPosition = stableLocalPosition;
	}
	
}
