using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//[RequireComponent(typeof(MeshFilter))]
//[RequireComponent(typeof(MeshRenderer))]
public class ComputeTailMeshData : ComputeCustomMeshFromLocus {

    public int currentSize;

    public Vector3[] vertices;
    public int[] triangles;
    public Vector3[] normals;
    public Vector2[] uvs;

    public StoreTailVertices tailScript;
    public List<Vector3> receivedArray;
    public List<Vector3> rights;

    public MeshFilter meshFilter;
    public Mesh mesh;

	// Use this for initialization
	void Start () {

        meshFilter = GetComponent<MeshFilter>();
        mesh = new Mesh();

        tailScript = GetComponent<StoreTailVertices>();
        receivedArray = tailScript.tailVertices;
        rights = tailScript.rightVectors;
        currentSize = receivedArray.Count;

        //only if there are more than one triangles
        if (currentSize > 1)
        {
            vertices = new Vector3[currentSize * 2];
            triangles = new int[currentSize * 6-6];
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
            triangles = new int[(currentSize-1) * 6];
            normals = new Vector3[currentSize * 2];
            uvs = new Vector2[currentSize * 2];
            mesh = new Mesh();

            //Compute vertices every frame. (or move them one index forward)
            //move all vertices by 2 indices
            //MoveArrayFromBy<Vector3>(ref vertices, 0, 2);
            //vertices[0] = receivedArray[0] + rights[0];
            //vertices[1] = receivedArray[0] - rights[0];
            for (int i = 0; i < currentSize; i++ )
            {
                //vertices
                vertices[i*2] = receivedArray[i] - rights[i];
                vertices[i*2+1] = receivedArray[i] + rights[i];

                //Compute new triangles (different loop outside)
                //MoveTrianglesFromBy(ref triangles, 0, 6, 2);

                //normals (all -ve z axis?)
                normals[i * 2] = new Vector3(0, 0, -1);
                normals[i * 2+1] = new Vector3(0, 0, -1);

                //UVs

            }

            //update triangles
            int index = 0;
            for(int i=0; i<= (currentSize-2)*2;i+=2)
            {

                triangles[index++] = i;
                triangles[index++] = i + 1;
                triangles[index++] = i + 2;

                triangles[index++] = i + 1;
                triangles[index++] = i + 3;
                triangles[index++] = i + 2;
            }

                
        }

        else
        {
            //vertices = null;
            //triangles = null;
            //normals = null;
            //uvs = null; 
        }	

        //Assign stuff to the mesh
        AssignToMesh(mesh, vertices, triangles, normals);
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

    void MoveTrianglesFromBy(ref int[] array, int from, int by, int addValue)
    {
        int arraySize = array.Length;
        for (int i = arraySize - 1; i >= from; i--)
        {
            array[i] = array[i - by];
            array[i] += addValue;
        }
    }

    void AssignToMesh(Mesh mesh, Vector3[] vertices, int [] triangles, Vector3[] normals)
    {
        this.mesh.vertices = vertices;
        this.mesh.triangles = triangles;
        this.mesh.normals = normals;
        this.meshFilter.mesh = this.mesh;
    }
}
