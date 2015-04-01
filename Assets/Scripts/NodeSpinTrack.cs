using UnityEngine;
using System.Collections;

public class NodeSpinTrack : MonoBehaviour {
	public float startRotation;
	public float endRotation;
	public ClusterNodePuzzleGroup targetNodeGroup;

	void Update()
	{
		float portionComplete = (float)targetNodeGroup.donePuzzleCount / targetNodeGroup.puzzleCount;
		Vector3 localRot = transform.localRotation.eulerAngles;
		localRot.z = (startRotation * (1 - portionComplete)) + (endRotation * portionComplete);
		transform.localRotation = Quaternion.Euler(localRot);
	}
}
