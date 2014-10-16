using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class ControllerFeedback : MonoBehaviour {

	float duration;
	float intensity;
	float startTime;

	bool isVibrating;

	// Use this for initialization
	void Start () {
		isVibrating = true;
		intensity = 0.0f;
		duration = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Time.time - startTime > duration)
		{
			EndVibration();
		}

		if (!isVibrating)
		{
			EndVibration();
		}

		//GamePad.SetVibration(0, intensity, intensity);

	}

	public void HardVibrate (float time) {
		startTime = Time.time;
		intensity = 0.5f;
		duration = 0.5f;
	}

	public void SoftVibrate (float time) {
		startTime = Time.time;
		intensity = 0.3f;
		duration = 0.5f;
	}

	public void SetVibration (float intensity)
	{
		isVibrating = true;
		this.intensity = intensity;
	}

	public void SetVibration (float intensity, float duration)
	{
		isVibrating = true;
		this.intensity = intensity;
		this.duration = duration;
		startTime = Time.time;
	}

	public void EndVibration ()
	{
		isVibrating = false;
		intensity = 0.0f;
	}
}
