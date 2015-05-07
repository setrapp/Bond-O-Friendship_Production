using UnityEngine;
using System.Collections;

public class SetShaderData_DepthMasker : MonoBehaviour {

    public GameObject p1, p2;

	// Use this for initialization
	void Start () {
        p1 = Globals.Instance.player1.gameObject;
        p2 = Globals.Instance.player2.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	    
        //every frame, update players positions on material (for shader)
        renderer.material.SetVector("P1 Position", p1.transform.position);
        renderer.material.SetVector("P2 Position", p2.transform.position);

	}
}
