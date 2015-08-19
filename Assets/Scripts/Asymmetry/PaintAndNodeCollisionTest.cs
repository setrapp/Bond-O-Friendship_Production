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
            Vector3 paintPos2D, nodePos2D;
		
			for (int i = 0; i < count; i++)
			{
				if (nodes [i] as MirroringClusterNode != null)
				{
                    //check on same plane (z=0)
                    paintPos2D = new Vector3(paintCircle.transform.position.x, paintCircle.transform.position.y, 0);
                    nodePos2D = new Vector3(nodes[i].transform.position.x, nodes[i].transform.position.y, 0);
					distanceSqr = Vector3.SqrMagnitude (paintPos2D - nodePos2D);
					if (distanceSqr <= (paintDropRadius + nodeRadius) * (paintDropRadius + nodeRadius))
					{
						((MirroringClusterNode)nodes[i]).RevealNode();

						// Fire pulses to show players something happened.
						Helper.FirePulse(paintCircle.transform.position - GetComponent<CanvasBehavior>().mirrorDistance, Globals.Instance.defaultPulseStats);
						Helper.FirePulse(paintCircle.transform.position, Globals.Instance.defaultPulseStats);
					}
				}
			}
		}
	}
}
