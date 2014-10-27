using UnityEngine;
using System.Collections;

public class PulseParticle : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
		void Update () {
			
			Invoke("DestroyPulse", 1.0f);
			
		}
		
		void DestroyPulse()
		{
			Destroy(gameObject);
		}
		
	}
