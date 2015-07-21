using UnityEngine;
using System.Collections;

public class PairedClusterNode : ClusterNode {

    public ClusterNode mirrorNode;
    public bool revealed;

	// Update is called once per frame
    protected override void Update() 
    {
        base.Update();
        MirrorNode();
	}

    void MirrorNode()
    {
        revealed = mirrorNode.GetComponent<ClusterNode>().lit;  
    }
}
