using UnityEngine;
using System.Collections;

public class HandleDripHit : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider collide)
    {
        if (collide.name == "DripPaintCircle(Clone)")
        {
            Debug.Log("Watered");
        }
    }
}
