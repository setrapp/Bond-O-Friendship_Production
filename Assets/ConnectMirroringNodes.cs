using UnityEngine;
using System.Collections;

public class ConnectMirroringNodes : MonoBehaviour {

    public ClusterNodePuzzle clusterToMirror;

	// Use this for initialization
	void Awake () {

        float numberOfNodes = this.gameObject.GetComponent<ClusterNodePuzzle>().nodes.Count;
        ClusterNode node, nodeToMirror;

        for (int i = 0; i < numberOfNodes; i++)
        {
            node = this.gameObject.GetComponent<ClusterNodePuzzle>().nodes[i];
            nodeToMirror = clusterToMirror.nodes[i];
            node.gameObject.GetComponent<PairedClusterNode>().mirrorNode = nodeToMirror;
        }
	
	}

    // Update is called once per frame
    void Update()
    {
	}
}
