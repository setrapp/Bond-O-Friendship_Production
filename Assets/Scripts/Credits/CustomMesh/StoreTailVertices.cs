using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StoreTailVertices : StoreCustomMeshLocus {

    //200 points
    public int maxSize = 200;
    //public int currentIndex = -1;
    public bool renderLine = false;
    public List <Vector3> tailVertices;
    //public Vector3[] right;

    //Debug stuff
    public LineRenderer lr;

	// Use this for initialization
	void Start () {

        lr = GetComponent<LineRenderer>();
	
	}
	
	// Update is called once per frame
	void Update () {

        int currentSize = tailVertices.Count;
        //if no tail
        if (currentSize < maxSize-1)
        {
            tailVertices.Insert(0, transform.position);
        }

        else if (currentSize == maxSize - 1)
        {
            tailVertices.RemoveAt(tailVertices.Count-1);
            tailVertices.Insert(0, transform.position);
        }

        else //extra points
        {
            while (tailVertices.Count > maxSize)
                tailVertices.RemoveAt(tailVertices.Count-1);
        }

        if (renderLine)
        {
            lr.enabled = true;
            lr.SetVertexCount(currentSize);
            for (int i = 0; i < currentSize; i++)
            {
                lr.SetPosition(i, tailVertices[i]);
            }
        }

        else lr.enabled = false;
	}
}
