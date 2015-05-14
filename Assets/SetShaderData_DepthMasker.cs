using UnityEngine;
using System.Collections;

public class SetShaderData_DepthMasker : MonoBehaviour {

    public GameObject p1, p2;
    //light cone
    public float height = 1.5f;
    public float radius = 3.0f;
    [HideInInspector] public float squaredHypotenuse;

	// Use this for initialization
	void Start () {
        p1 = Globals.Instance.player1.gameObject;
        p2 = Globals.Instance.player2.gameObject;
        squaredHypotenuse = height * height + radius * radius;
	}
	
	// Update is called once per frame
	void Update () {

        //every frame, update players positions on material (for shader)

        Vector3 pos = new Vector3(p1.transform.position.x, p1.transform.position.y, transform.position.z - height);
        renderer.material.SetVector("_P1Pos", pos);

        //p2
        pos = new Vector3(p2.transform.position.x, p2.transform.position.y, transform.position.z - height);
        renderer.material.SetVector("_P2Pos", pos);

	}
}
