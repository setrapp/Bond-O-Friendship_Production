using UnityEngine;
using System.Collections;

public class WiggleHandler : MonoBehaviour {

    public ClusterNode clusterNode;
    public CrumpleMesh crumpleMesh;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if(clusterNode != null && crumpleMesh !=null)
        {
            crumpleMesh.wilin = clusterNode.lit;
        }
	}
}
