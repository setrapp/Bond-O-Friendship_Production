using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MembraneShellFill : MonoBehaviour {

	public MembraneShell membraneShell;
	public MeshFilter meshFilter;
	private Mesh mesh;
	public Vector3[] vertices;
	public Vector2[] uvs;
	public int[] triangles;
	private List<Vector3> vertexList = new List<Vector3>();
	private List<int> triangleList = new List<int>();
	public bool recalculateNormals = false;

	void Start()
	{
		if (meshFilter == null)
		{
			meshFilter = GetComponent<MeshFilter>();
		}

		if (meshFilter != null)
		{
			mesh = meshFilter.mesh;
		}

		/*if (mesh != null)
		{
			vertexList = new List<Vector3>();
			for (int i = 0; i < mesh.vertexCount; i++)
			{
				Debug.Log(mesh.vertices[i]);
				vertexList.Add(mesh.vertices[i]);
			}

			triangleList = new List<int>();
			for (int i = 0; i < mesh.triangles.Length; i++)
			{
				triangleList.Add(mesh.triangles[i]);
			}


			// END TEMP



			
			
		}*/
	}

	void Update()
	{
		/*TODO if membranes are disabled, disable the renderer*/

		if (membraneShell != null)
		{
			vertexList = new List<Vector3>();
			triangleList = new List<int>();

			vertexList.Add(Vector3.zero);
			for (int i = 0; i < membraneShell.createdWalls.Count; i++)
			{
				Membrane membrane = (Membrane)membraneShell.createdWalls[i].membraneCreator.createdBond;
				for (int j = 0; j < membrane.links.Count; j++)
				{
					vertexList.Add(transform.InverseTransformPoint(membrane.links[j].transform.position));
					if (vertexList.Count >= 3)
					{
						triangleList.Add(0);
						triangleList.Add(vertexList.Count - 2);
						triangleList.Add(vertexList.Count - 1);
					}
				}
				triangleList.Add(0);
				triangleList.Add(vertexList.Count - 1);
				triangleList.Add(1);
			}

			ApplyChanges();
		}
	}

	private void ApplyChanges()
	{
		vertices = new Vector3[vertexList.Count];
		for (int i = 0; i < vertexList.Count; i++)
		{
			vertices[i] = vertexList[i];
		}

		triangles = new int[triangleList.Count];
		for (int i = 0; i < triangleList.Count; i++)
		{
			triangles[i] = triangleList[i];
		}

		mesh.Clear();
		mesh.vertices = vertices;
		mesh.triangles = triangles;



		if (recalculateNormals)
		{
			mesh.RecalculateNormals();
		}
		mesh.RecalculateBounds();
	}
}
