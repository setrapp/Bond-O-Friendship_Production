using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ComputeTrailVertices : MonoBehaviour {

    public int maxLength = 180;
    public float startWidth = 1.0f;
    public float endWidth = 1.0f;
    
    public int currentIndex = -1;

    public Vector3[] recentPath;
    public List<Vector3> tail;

    //Test via line renderer
    LineRenderer lineRenderer;


	// Use this for initialization
	void Start () {

        recentPath = new Vector3[maxLength];
        ////recentPath[0] = transform.position;
        ////AddPointAtBeginning(ref recentPath, transform.position);
        //currentIndex++;
        lineRenderer = GetComponent<LineRenderer>();

	}
	
	// Update is called once per frame
	void Update () {

        //Remove last vertex from the tail every update
        //if ((currentIndex == maxLength-1 || transform.position == recentPath[0]) && currentIndex >= 0)
        //{
            //RemovePointFromEnd(ref recentPath, ref currentIndex);
            //currentIndex--;
        //}

        //If the object has moved, add the new point to the array
        //else if (transform.position != recentPath[0])
        //{
            tail.Add(transform.position);
            //MoveOneIndexForward(ref recentPath, ref currentIndex);
            //AddPointAtBeginning(ref recentPath, transform.position);
        //}

       // recentPath = tail.ToArray();

        lineRenderer.SetVertexCount(tail.Count);

        for (int i = 0; i < atail.Count; i++)
           lineRenderer.SetPosition(i, tail[i]);

	}

    void AddPointAtBeginning(ref Vector3[] vectorArray, Vector3 point)
    {
        recentPath[0] = transform.position;
        //indicesFilled++;  //Why? STUPID STUPID ME!

    }

    void RemovePointFromEnd(ref Vector3[] vectorArray, ref int indicesFilled)
    {
        //destroy previously existing point
        //vectorArray[indicesFilled] = null;
        indicesFilled--;
    }

    void MoveOneIndexForward(ref Vector3[] vectorArray, ref int indicesFilled)
    {
        indicesFilled++;
        for (int i = indicesFilled; i > 0; i--)
        {
            vectorArray[indicesFilled] = vectorArray[indicesFilled - 1];
        }
    }
}
