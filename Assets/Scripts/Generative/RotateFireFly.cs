﻿using UnityEngine;
using System.Collections;

public class RotateFireFly : MonoBehaviour {

    public float x, y, z;
    public float angleStep = 3;
    public float timeStep = 1;
    private Vector3 rotationAxis;


    // Use this for initialization
    void Start()
    {
        rotationAxis = new Vector3(x, y, z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, timeStep*angleStep, 0);
    }
}
