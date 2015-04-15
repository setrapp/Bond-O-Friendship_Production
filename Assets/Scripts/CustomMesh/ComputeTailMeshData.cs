using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ComputeTailMeshData : ComputeCustomMeshFromLocus {

    public int currentSize;

    public Vector3[] vertices;
    public int[] triangles;
    public Vector3[] normals;
    public Vector2[] uvs;

    public StoreTailVertices tailScript;
    public List<Vector3> receivedArray;
    public List<Vector3> rights;

	// Use this for initialization
	void Start () {

        tailScript = GetComponent<StoreTailVertices>();
        receivedArray = tailScript.tailVertices;
        rights = tailScript.rightVectors;
        currentSize = receivedArray.Count;

        //only if there are more than one triangles
        if (currentSize > 1)
        {
            vertices = new Vector3[currentSize * 2];
            triangles = new int[currentSize * 3];
            normals = new Vector3[currentSize * 2];
            uvs = new Vector2[currentSize * 2];
        }

	}
	
	// Update is called once per frame
	void Update () {

        receivedArray = tailScript.tailVertices;
        currentSize = receivedArray.Count;

        //only if there are more than one triangles
        if (currentSize > 1)
        {
            vertices = new Vector3[currentSize * 2];
            triangles = new int[currentSize * 3];
            normals = new Vector3[currentSize * 2];
            uvs = new Vector2[currentSize * 2];
        }

        else
        {
            vertices = null;
            triangles = null;
            normals = null;
            uvs = null; 
        }


        //Compute vertices every frame. (or move them one index forward)
        //move all vertices by 2 indices
        MoveArrayFromBy<Vector3>(ref vertices, 0, 2);
        vertices[0] = receivedArray[0] - rights[0];
        vertices[1] = receivedArray[0] + rights[0];

        //Compute new triangles
        

        //normals (all -ve z axis?)

        //UVs
	
	}

    void MoveArrayFromBy<T>(ref T[] array, int from, int by)
    {
        int arraySize = array.Length;
        for(int i = arraySize-1; i>=from; i--)
        {
            array[i] = array[i - by];
        }
    }

    void RemoveFromIndex<T>(ref T[] array, int from)
    {
        int arraySize = array.Length;
        for(int i = arraySize-1; i>= from; i--)
        {
            //delete those points
            //???
        }
    }
}
