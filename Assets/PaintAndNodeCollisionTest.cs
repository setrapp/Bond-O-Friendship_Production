using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PaintAndNodeCollisionTest : MonoBehaviour {

    public ClusterNodePuzzle mirrorPuzzle;
    [HideInInspector] public List <ClusterNode> nodes;
    public float nodeRadius = 0.6f;
    public float paintDropRadius;

	// Use this for initialization
	void Start () {
        mirrorPuzzle = GameObject.Find("MirroringClusterPuzzle").GetComponent<ClusterNodePuzzle>();
        nodes = mirrorPuzzle.nodes;
        nodeRadius = (gameObject.GetComponent<PaintCircle>().initialRadius) / 2;
	}
	
	// Update is called once per frame
	void Update () {

        int count = nodes.Count;
        float distance;

        for (int i = 0; i < count; i++)
        {
            if (nodes[i] as MirroringClusterNode != null)
            {
                distance = Vector3.Distance(transform.position, nodes[i].transform.position);
                if (distance <= paintDropRadius + nodeRadius)
                    ((MirroringClusterNode)nodes[i]).RevealNode();
            }
        }

	}
}
