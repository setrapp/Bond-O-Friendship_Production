using UnityEngine;
using System.Collections;

public class CreateTailMesh : MonoBehaviour {

    //assuming 60 fps, 3 seconds
    public int maxLength = 180;


 
    public Vector3[] vertices;
    public int[] indices;
    public Vector3[] normals;
    public Vector2[] uvs;

    public Vector3 right;
    private int currentIndex = -1;


	// Use this for initialization
	void Start () {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        meshFilter.mesh = mesh;
        right = new Vector3(1, 0, 0);

        //Vertices
        vertices = new Vector3[maxLength * 2];

        //Triangles
        indices = new int[(maxLength - 1) * 2];

        //Normals
        normals = new Vector3[(maxLength - 1) * 2];

        //UVs

        //UpdateMesh();

	}
	
	// Update is called once per frame
	void FixedUpdate () {

        currentIndex++;
 



	}
}
