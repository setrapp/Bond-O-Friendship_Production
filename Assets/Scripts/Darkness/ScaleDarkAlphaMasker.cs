using UnityEngine;
using System.Collections;

public class ScaleDarkAlphaMasker : MonoBehaviour {

    //exposed
    public float extraWidth = 60.0f;

    //private
    Vector3 p1Pos, p2Pos;

	// Use this for initialization
	void Start () {
        p1Pos = Globals.Instance.Player1.transform.position;
        p2Pos = Globals.Instance.Player2.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        
        p1Pos = Globals.Instance.Player1.transform.position;
        p2Pos = Globals.Instance.Player2.transform.position;

        //center of diagonal
        float z = transform.position.z;
        this.transform.position = (p1Pos + p2Pos) / 2;
        transform.position += new Vector3(0, 0, z - transform.position.z);

        //calculate diagonal distance
        Vector3 diagonal = p2Pos - p1Pos;

        //Scaling
        this.transform.localScale = new Vector3(Mathf.Abs(diagonal.x) + extraWidth, Mathf.Abs(diagonal.y) + extraWidth, 1);

	}
}
