using UnityEngine;
using System.Collections;

public class CanvasProgress : MonoBehaviour {

	public  ClusterNodePuzzle pairedPuzzleScript;
	private Vector3 targetScale;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (pairedPuzzleScript != null && transform.localScale.x < 0.99f) {
			targetScale = new Vector3 (pairedPuzzleScript.progress, pairedPuzzleScript.progress, pairedPuzzleScript.progress);
			transform.localScale = Vector3.Lerp (transform.localScale, targetScale, Time.deltaTime);
		}
	}
}
