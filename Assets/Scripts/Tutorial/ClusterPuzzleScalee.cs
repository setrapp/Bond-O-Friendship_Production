using UnityEngine;
using System.Collections;

public class ClusterPuzzleScalee : MonoBehaviour {

    public ClusterNodePuzzle triggerCluster;
    public Vector3 startScale;
    public Vector3 endScale;

    private ShrinkAndMove shrinkAndMove;

    void Start()
    {
        shrinkAndMove = gameObject.GetComponent<ShrinkAndMove>();
    }

    void Update()
    {
        transform.localScale = (startScale * (1 - triggerCluster.progress)) + (endScale * triggerCluster.progress);

        if (triggerCluster.progress >= 1 && !shrinkAndMove.fullSize)
		{
			shrinkAndMove.BecomeFullSize();
		}
    }
}
