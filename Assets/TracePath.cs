using UnityEngine;
using System.Collections;

public class TracePath : MonoBehaviour {

    public int maxLength = 180;
    public float startWidth = 1.0f;
    public float endWidth = 1.0f;
    
    private int currentIndex = -1;

    public Vector3[] recentPath;

    //Test via line renderer
    LineRenderer lineRenderer;


	// Use this for initialization
	void Start () {

        recentPath = new Vector3[maxLength];
        recentPath[0] = transform.position;
        currentIndex++;
        lineRenderer = GetComponent<LineRenderer>();

	}
	
	// Update is called once per frame
	void Update () {

        //Remove last vertex from the tail every update
        if (currentIndex > 0) 
            RemovePointFromEnd(ref recentPath, ref currentIndex);

        //If the object has moved, add the new point to the array
        if (transform.position != recentPath[0] && currentIndex >= 0)
        {
            MoveOneIndexForward(ref recentPath, ref currentIndex);
            AddPointAtBeginning(ref recentPath, transform.position);
        }

        lineRenderer.SetVertexCount(currentIndex);
        for (int i = 0; i < currentIndex; i++)
            lineRenderer.SetPosition(i, recentPath[i]);

	}

    void AddPointAtBeginning(ref Vector3[] vectorArray, Vector3 point)
    {
        recentPath[0] = transform.position;
    }

    void RemovePointFromEnd(ref Vector3[] vectorArray, ref int indicesFilled)
    {
        //destroy previously existing vector
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
