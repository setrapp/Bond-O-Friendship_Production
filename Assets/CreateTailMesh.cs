using UnityEngine;
using System.Collections;

public class CreateTailMesh : MonoBehaviour {

    //assuming 60 fps, 3 seconds
    public int maxSize = 180;

    public Vector3[] vertices;
    public int[] indices;
    public Vector3[] normals;
    public Vector2[] uvs;


	// Use this for initialization
	void Start () {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        meshFilter.mesh = mesh;

        //Vertices
        vertices = new Vector3[maxSize*2];

        //Triangles
        indices = new int[(maxSize-1)*2];

        //Normals
        normals = new Vector3[(maxSize-1)*2];

        //UVs

        //UpdateMesh();

	}
	
	// Update is called once per frame
	void FixedUpdate () {



	}
}
