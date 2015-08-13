using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PaintAndNodeCollisionTest : MonoBehaviour {

    public ClusterNodePuzzle mirrorPuzzle;
    public List <ClusterNode> nodes;
    public float nodeRadius = 0.6f;
    public float paintDropRadius;

	// Use this for initialization
	void Start () {
        //mirrorPuzzle = GetComponent<CanvasBehavior>().puzzleToReveal;
        nodes = mirrorPuzzle.nodes;    
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void CheckPaintAndNodeCollision(PaintCircle paintCircle)
	{
		if (GetComponent<CanvasBehavior> ().paintCopier != null) {
			int count = nodes.Count;
			float distanceSqr;
			float paintDropRadius = paintCircle.initialRadius / 2;
		
			for (int i = 0; i < count; i++) {
				if (nodes [i] as MirroringClusterNode != null) {
					distanceSqr = Vector3.SqrMagnitude (paintCircle.transform.position - nodes [i].transform.position);
					if (distanceSqr <= (paintDropRadius + nodeRadius) * (paintDropRadius + nodeRadius))
						((MirroringClusterNode)nodes [i]).RevealNode ();
				}
			}
		}
	}
}
