using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ComputeTailMeshData : ComputeCustomMeshFromLocus {

    public int currentSize;

    public Vector3[] vertices;
    public int[] triangles;
    public Vector3[] normals;
    public Vector2[] uvs;

    public List<Vector3> receivedArray;

    public StoreTailVertices tailScript;

	// Use this for initialization
	void Start () {

        tailScript = GetComponent<StoreTailVertices>();
        receivedArray = tailScript.tailVertices;
        currentSize = receivedArray.Count;

	}
	
	// Update is called once per frame
	void Update () {

        receivedArray = tailScript.tailVertices;
        currentSize = receivedArray.Count;
	
	}
}
