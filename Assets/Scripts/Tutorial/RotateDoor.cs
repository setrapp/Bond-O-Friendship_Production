using UnityEngine;
using System.Collections;

public class RotateDoor : MonoBehaviour {

	private NodePuzzle nodePuzzle;
	private bool rotating;

	// Use this for initialization
	void Start () {
		nodePuzzle = transform.parent.GetChild(0).GetComponent<NodePuzzle>();
	}
	
	// Update is called once per frame
	void Update () {
		if(nodePuzzle != null)
		{
			if(nodePuzzle.solved == true)
				rotating = true;
		}

		if(rotating == true)
		{
			if(name == "Gate Hinge" && transform.localRotation.eulerAngles.z < 237.0f)
				transform.Rotate(Vector3.forward * Time.deltaTime * 8.0f);
			if(name == "Gate Hinge 2" && transform.localRotation.eulerAngles.z > 56.0f)
				transform.Rotate(-Vector3.forward * Time.deltaTime * 8.0f);
		}
	}
}
