using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tracer : MonoBehaviour {
	public LineRenderer lineRenderer = null;
	public int maxVertices = -1;
	private List<Vector3> vertices;
	public GameObject lineMakerPrefab = null;
	//private Vector3 lastVertex = Vector3.zero;
	//private Vector3 lastDirection = Vector3.zero;
	public float trailNearWidth = 1;
	public float trailFarWidth = 1;
	public float zOffset = 10;
	public float minVertexDistance = 1;
	
	void Start() 
	{
		vertices = new List<Vector3>();
	}

	public void StartLine(bool startAtVertex = false, Vector3 startVertex = new Vector3())
	{
		DestroyLine();
		CreateLineMaker(true);
		
		AddVertex(transform.position);
	}

	public void AddVertex(Vector3 position) {
		position.z += zOffset;

		// Only draw a new vertex when far enough away from the most recently added.
		if (vertices.Count > 0 && (position - vertices[vertices.Count - 1]).sqrMagnitude < minVertexDistance * minVertexDistance)
		{
			return;
		}

		/* Line Presevation.
		if (vertices.Count > 1) {
			// Preserve look of the most recent line segement if the new vertex
			// drastically changes the direction of motion. Without this, the line segement
			// gets distorted as only the center point is ever stored.
			if (Vector3.Dot((position - lastVertex).normalized, lastDirection) < 0.6f) {
				Vector3 midPosition = lastVertex + (lastDirection * minDragToDraw / 4);
				vertices.Add(midPosition);
				lineRenderer.SetVertexCount(vertices.Count);
				lineRenderer.SetPosition(vertices.Count - 1, midPosition); 
				lastVertex = midPosition;
			}
		}*/

		if (lineRenderer)
		{
			// Add new vertex.
			vertices.Add(position);
			lineRenderer.SetVertexCount(vertices.Count);
			lineRenderer.SetPosition(vertices.Count - 1, position);
			//lastDirection = (position - lastVertex).normalized;
			//lastVertex = position;

			// Keep vertex count within limits.
			if (maxVertices >= 0 && vertices.Count > maxVertices)
			{
				int replaceOffset = vertices.Count - maxVertices;
				for (int i = 0; i < maxVertices; i++)
				{
					vertices[i] = vertices[i + replaceOffset];
					lineRenderer.SetPosition(i, vertices[i]);
				}
				for (int i = maxVertices - 1; i < vertices.Count; i++)
				{
					vertices.RemoveAt(i);
				}
				lineRenderer.SetVertexCount(maxVertices);
			}
		}
		
	}	
	
	public void CreateLineMaker(bool criticalLine) {		
		GameObject newLineMaker = (GameObject)Instantiate(lineMakerPrefab, Vector3.zero, Quaternion.identity);
		newLineMaker.transform.parent = transform;
		lineRenderer = newLineMaker.GetComponent<LineRenderer>();
		lineRenderer.SetVertexCount(0);
		lineRenderer.SetWidth(trailFarWidth, trailNearWidth);
		vertices = new List<Vector3>();
	}

	public void DestroyLine()
	{
		if (lineRenderer != null)
		{
			vertices.Clear();
			lineRenderer.SetVertexCount(0);
			GameObject.Destroy(lineRenderer.gameObject);
			lineRenderer = null;
		}
	}

	public int FindNearestIndex(Vector3 point, int startIndex = 0)
	{
		int nearestIndex = startIndex;
		float minSqrDist = (vertices[0] - point).sqrMagnitude;
		for (int i = startIndex + 1; i < vertices.Count; i++)
		{
			float sqrDist = (vertices[i] - point).sqrMagnitude;
			if (sqrDist < minSqrDist)
			{
				minSqrDist = sqrDist;
				nearestIndex = i;
			}
		}

		return nearestIndex;
	}

	public void MoveVertices(Vector3 alteration)
	{
		for (int i = 0; i < vertices.Count; i++)
		{
			vertices[i] += alteration;
			lineRenderer.SetPosition(i, vertices[i]);
		}
	}

	public Vector3 GetVertex(int index, bool negateZOffset = true)
	{
		Vector3 vertex = vertices[index];
		if (negateZOffset)
		{
			vertex.z -= zOffset;
		}
		return vertex;
	}

	public int GetVertexCount()
	{
		return vertices.Count;
	}
}
