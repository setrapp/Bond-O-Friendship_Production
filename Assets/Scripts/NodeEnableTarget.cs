using UnityEngine;
using System.Collections;

public class NodeEnableTarget : MonoBehaviour {
	public ClusterNodePuzzle triggerPuzzle;
	public GameObject targetEnablee;

	void Update()
	{
		if (triggerPuzzle.solved)
		{
			targetEnablee.SetActive(true);
			Destroy(this);
		}
	}
}
