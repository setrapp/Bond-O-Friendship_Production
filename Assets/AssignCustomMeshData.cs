using UnityEngine;
using System.Collections;

public class AssignCustomMeshData : MonoBehaviour {

    //assuming 60 fps, 3 seconds
    public int maxLength;
    public ComputeTrailVertices trailScript;
    [HideInInspector]
    public Vector3[] receivedData;
 
    private Vector3[] vertices;
    private int[] indices;
    private Vector3[] normals;
    private Vector2[] uvs;

    public Vector3 right;
    private int currentIndex = -1;
    private int currentWidth = 5;

    public Mesh mesh;


	// Use this for initialization
	void Start () {

        trailScript = GetComponent<ComputeTrailVertices>();
        maxLength = trailScript.maxLength;

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        mesh = new Mesh();
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
	void Update () {

        //Update mesh from array
        receivedData = trailScript.recentPath;
        currentIndex = trailScript.currentIndex;
        for (int i=0; i<=currentIndex; i+=2)
        {
            vertices[i] = trailScript.recentPath[i] + right * currentWidth;
            vertices[i+1] = trailScript.recentPath[i] - right * currentWidth;
        }
        mesh.vertices = vertices;

        //for (int i = 0 )
        //for (int i=0, )



	}
}
