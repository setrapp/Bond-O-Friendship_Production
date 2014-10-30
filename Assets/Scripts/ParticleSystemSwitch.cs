using UnityEngine;
using System.Collections;

public class ParticleSystemSwitch : MonoBehaviour {

	public ParticleSystem p2A1;
	public ParticleSystem p2A2;
	public ParticleSystem p2A3;
	public ParticleSystem p2A4;
	public ParticleSystem p2A5;
	public ParticleSystem p2A6;
	private float timerOn;
	public float onTime = 5.0f;
	private float timerOff;
	public float offTime = 1.0f;

	// Use this for initialization
	void Start () {
		timerOn = onTime;
		timerOff = offTime;
	}
	
	// Update is called once per frame
	void Update () {

		timerOn -= Time.deltaTime;
		if(timerOn <= 0)
		{
			p2A1.enableEmission = false;
			p2A2.enableEmission = false;
			p2A3.enableEmission = false;
			p2A4.enableEmission = false;
			p2A5.enableEmission = false;
			p2A6.enableEmission = false;
			timerOff -= Time.deltaTime;
			if(timerOff <= 0)
			{
				p2A1.enableEmission = true;
				p2A2.enableEmission = true;
				p2A3.enableEmission = true;
				p2A4.enableEmission = true;
				p2A5.enableEmission = true;
				p2A6.enableEmission = true;
				timerOn = onTime;
				timerOff = offTime;
			}
		}

	}
	
}
