using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MembraneShellFill : MonoBehaviour {

	public MembraneShell membraneShell;
	public MeshRenderer meshRenderer;
	public MeshFilter meshFilter;
	public CrumpleMesh crumpleMesh;
	private Mesh mesh;
	public Vector3[] vertices;
	public Vector2[] uvs;
	public int[] triangles;
	private List<Vector3> vertexList = new List<Vector3>();
	private List<Vector2> uvList = new List<Vector2>();
	private List<int> triangleList = new List<int>();
	public float maxBurstRadius;
	public bool atMaxBurst = false;
	public float meshBurstRate = 1.02f;
	public float vertexBurstRate = 0.5f;
	public bool recalculateNormals = false;

	void Start()
	{
		if (meshRenderer == null)
		{
			meshRenderer = GetComponent<MeshRenderer>();
		}
		if (crumpleMesh == null)
		{
			crumpleMesh = GetComponent<CrumpleMesh>();
		}
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

		if (membraneShell != null && !membraneShell.breaking) {
			vertexList = new List<Vector3> ();
			uvList = new List<Vector2>();
			triangleList = new List<int> ();

			vertexList.Add (Vector3.zero);
			uvList.Add(new Vector2(0.5f, 0.5f));
			for (int i = 0; i < membraneShell.createdWalls.Count; i++) {
				Membrane membrane = (Membrane)membraneShell.createdWalls [i].membraneCreator.createdBond;
				for (int j = 0; j < membrane.links.Count; j++) {
					if (membrane.links [j].linkPrevious != null || vertexList.Count < 2) {
						Vector3 linkPos = membrane.links [j].transform.position;
						linkPos.z = transform.position.z;
						vertexList.Add(transform.InverseTransformPoint(linkPos));
					}
				}
			}

			float highRadius = FindLargestRadius();
			if (highRadius > 0)
			{
				for (int i = 1; i < vertexList.Count; i++)
				{
					Vector3 uvPos = vertexList[i] / (highRadius * 2);
					uvList.Add(new Vector2(uvPos.x + 0.5f, uvPos.y + 0.5f));
				}
			}

			/*TODO figure out how to prevent creasing caused by overlap.*/
			for (int i = 2; i < vertexList.Count; i++) {
				triangleList.Add (0);
				triangleList.Add (i - 1);
				triangleList.Add (i);
			}
			triangleList.Add (0);
			triangleList.Add (vertexList.Count - 1);
			triangleList.Add (1);

			ApplyChanges ();
		} else {
			if (vertexList.Count > 0)
			{
				vertexList.Clear();
				triangleList.Clear();
				if (crumpleMesh != null)
				{
					crumpleMesh.enabled = true;
				}
			}

			if (!atMaxBurst)
			{
				transform.localScale *= meshBurstRate;
				float highSqrRadius = FindLargestSquareRadius();
				for (int i = 0; i < vertices.Length; i++)
				{
					float sqrRadius = (vertices[i] - vertices[0]).sqrMagnitude;
					if (sqrRadius > 0)
					{
						float toHighFactor = highSqrRadius / sqrRadius;
						float growFactor = ((toHighFactor - 1) * vertexBurstRate) + 1;
						vertices[i] = vertices[0] + ((vertices[i] - vertices[0]) * growFactor);
					}
				}

				ApplyChanges(false);

				if (transform.localScale.x >= maxBurstRadius)
				{
					atMaxBurst = true;
				}
			}
		}
	}

	private float FindLargestSquareRadius()
	{
		float highSqrRadius = 0;
		for (int i = 1; i < vertices.Length; i++)
		{
			float toVertSqrDist = (vertices[i] - vertices[0]).sqrMagnitude;
			if (toVertSqrDist > highSqrRadius)
			{
				highSqrRadius = toVertSqrDist;
			}
		}
		return highSqrRadius;
	}

	private float FindLargestRadius()
	{
		return Mathf.Sqrt(FindLargestSquareRadius());
	}

	private void ApplyChanges(bool updateBuffers = true)
	{
		if (updateBuffers) {
			vertices = new Vector3[vertexList.Count];
			for (int i = 0; i < vertexList.Count; i++) {
				vertices [i] = vertexList [i];
			}

			uvs = new Vector2[uvList.Count];
			for (int i = 0; i < uvList.Count; i++)
			{
				uvs[i] = uvList[i];
			}

			triangles = new int[triangleList.Count];
			for (int i = 0; i < triangleList.Count; i++) {
				triangles [i] = triangleList [i];
			}
		}


		if (vertices.Length == uvs.Length)
		{
			mesh.Clear();
			mesh.vertices = vertices;
			mesh.uv = uvs;
			mesh.triangles = triangles;
		}
		

		if (recalculateNormals)
		{
			mesh.RecalculateNormals();
		}
		mesh.RecalculateBounds();
	}
}
