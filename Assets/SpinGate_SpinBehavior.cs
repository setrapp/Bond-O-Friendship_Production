using UnityEngine;
using System.Collections;

public class SpinGate_SpinBehavior : MonoBehaviour {

    public bool activated = false;
    public float rotationSpeed = -5.0f; //5 degrees/frame counter-clockwise

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (activated)
            transform.RotateAroundLocal(Vector3.forward, Time.deltaTime * rotationSpeed);
	}
}
