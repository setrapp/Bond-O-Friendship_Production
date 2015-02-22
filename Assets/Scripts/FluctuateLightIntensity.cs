using UnityEngine;
using System.Collections;

public class FluctuateLightIntensity : MonoBehaviour {

	public float duration = 1.0f;
	public  float amplitude = 4.0f;
	public float deviation = 2.0f;
	Light spotlight;
	float phi;


	// Use this for initialization
	void Start () {
		spotlight=gameObject.GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
		phi = Time.time / duration * 2 * Mathf.PI;
		spotlight.intensity = Mathf.Cos (phi) * deviation + amplitude;

	}
}
