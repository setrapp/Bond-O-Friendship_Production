using UnityEngine;
using System.Collections;

public class SetShaderData_DarkAlphaMasker : MonoBehaviour {

    public GameObject p1, p2, l1, l2;
    public float l1p1_sqMag, l2p2_sqMag;    //shortest distances (useful in shader)
    private Vector4 mul_sameLuminus, mul_diffLuminus;

    //Imaginary height of the light source
    public float height = 1.5f;

	// Use this for initialization
	void Start () {
        p1 = Globals.Instance.player1.gameObject;
        p2 = Globals.Instance.player2.gameObject;
        mul_sameLuminus = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
        mul_diffLuminus = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
	}
	
	// Update is called once per frame
	void Update () {

        //every frame, update players positions on material (for shader)
        Vector3 pos = new Vector3(p1.transform.position.x, p1.transform.position.y, transform.position.z - height);
        GetComponent<Renderer>().material.SetVector("_P1Pos", pos);
        pos = new Vector3(p2.transform.position.x, p2.transform.position.y, transform.position.z - height);
        GetComponent<Renderer>().material.SetVector("_P2Pos", pos);

        //repeat for the two closest lumini
        pos = new Vector3(l1.transform.position.x, l1.transform.position.y, transform.position.z - height);
        GetComponent<Renderer>().material.SetVector("_L1Pos", pos);
        pos = new Vector3(l2.transform.position.x, l2.transform.position.y, transform.position.z - height);
        GetComponent<Renderer>().material.SetVector("_L2Pos", pos);

        //if both players are close to the same luminus, avoid double adding the same value
        if (l1 == l2)
            GetComponent<Renderer>().material.SetVector("_Mul", mul_sameLuminus);
        else
            GetComponent<Renderer>().material.SetVector("_Mul", mul_diffLuminus);

	}
}
