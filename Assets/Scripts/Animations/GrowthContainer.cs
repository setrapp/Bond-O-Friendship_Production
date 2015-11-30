using UnityEngine;
using System.Collections;

public class GrowthContainer : MonoBehaviour {

    public bool activated;

	// Use this for initialization
	void Start () {
        activated = false;
	}

    // Update is called once per frame
    void Update() {
	
	}

    void Activate()
    {
        activated = true;
        Debug.Log("activated");
    }
}
