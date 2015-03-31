using UnityEngine;
using System.Collections;

public class StreamFlow : MonoBehaviour {

	/*public SpinPad spinPad;
	public InhibitSpinPad inhibitSpinPad;
	private ParticleSystem particle;
	private bool flowing;*/
	public ParticleSystem stream;
	public float addStreamLifeTime = 0;

	public void TriggerEmpty(GameObject emptyTrigger)
	{
		stream.startLifetime += addStreamLifeTime;
	}
}
