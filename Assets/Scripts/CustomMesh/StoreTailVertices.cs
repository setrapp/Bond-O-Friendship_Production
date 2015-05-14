using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StoreTailVertices : StoreCustomMeshLocus {

    //200 points
    public int maxSize = 200;
    //public int currentIndex = -1;
    public bool renderLine = false;
    public List <Vector3> tailVertices;
    public List <Vector3> rightVectors;
    private Vector3 right;
    //public Vector3[] right;

    //Debug stuff
    public LineRenderer lr;

	// Use this for initialization
	void Start () 
    {

        right = new Vector3(1f, 0f, 0f);
        lr = GetComponent<LineRenderer>();
	
	}
	
	// Update is called once per frame
	void Update () 
    {

        right = new Vector3(1, 0, 0);
        int currentSize = tailVertices.Count;

        //if tail not at max length
        if (currentSize < maxSize - 1)
        {
            tailVertices.Insert(0, transform.position);
            //rightVectors.Insert(0, transform.rotation * right); 
        }

        else if (currentSize == maxSize - 1)
        {
            //Remove from the end
            tailVertices.RemoveAt(tailVertices.Count-1);
            rightVectors.RemoveAt(rightVectors.Count - 1);
            //Add at the beginning
            tailVertices.Insert(0, transform.position);
            //rightVectors.Insert(0, transform.rotation * right);
        }

        if (currentSize >= 2)
            right = Quaternion.Euler(0, 0, 90) * (tailVertices[currentSize - 1] - tailVertices[currentSize - 2]);

        rightVectors.Insert(0, transform.rotation * right.normalized); 

        //else //extra points
        //{
        //    while (tailVertices.Count > maxSize)
        //    {
        //        tailVertices.RemoveAt(tailVertices.Count - 1);
        //        rightVectors.RemoveAt(rightVectors.Count - 1);
        //    }
        //}

        if (renderLine)
        {
            lr.enabled = true;
            lr.SetVertexCount(currentSize);
            for (int i = 0; i < currentSize; i++)
            {
                lr.SetPosition(i, tailVertices[i]);
                //lr.SetPosition(i, rightVectors[i]);
            }
        }

        else lr.enabled = false;

	}
}
