using UnityEngine;
using System.Collections;

public class RevealNode : MonoBehaviour {

    public MirroringClusterNode parentNode;

	// Use this for initialization
	void Start () {
        parentNode = transform.parent.GetComponent<MirroringClusterNode>();
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<Renderer>().enabled = parentNode.revealed;    
	}
}
