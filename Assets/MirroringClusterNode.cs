using UnityEngine;
using System.Collections;

public class MirroringClusterNode : ClusterNode {

    public ClusterNode nodeToMirror;
    public bool revealed;

	// Update is called once per frame
    protected override void Update() 
    {
        base.Update();
        MirrorNode();
	}

    void MirrorNode()
    {
        revealed = nodeToMirror.lit;  
    }
}
