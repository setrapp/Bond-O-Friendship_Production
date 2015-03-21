using UnityEngine;
using System.Collections;

public class RiverFlow : MonoBehaviour {

	public SpinPad spinPad;
	public InhibitSpinPad inhibitSpinPad;
	private ParticleSystem particle;
	private bool flowing;

	// Use this for initialization
	void Start () {
		particle = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		/*if(inhibitSpinPad.collided == true && spinPad.resetting == true && flowing == false)
		{
			particle.startLifetime += 3;
			flowing = true;
		}*/
	}
}
