using UnityEngine;
using System.Collections;

public class OrbToStream : MonoBehaviour {

	public OrbWaitPad triggerPad;
	public StreamSpawner streamSpawner;
	private bool spawnStream = false;

	// Use this for initialization
	void Awake () {
		if (enabled)
		{
			streamSpawner.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(triggerPad.fullyLit && !spawnStream)
		{
			spawnStream = true;
			streamSpawner.enabled = true;
		}
	}
}
