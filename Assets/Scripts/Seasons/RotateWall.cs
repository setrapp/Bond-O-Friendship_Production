using UnityEngine;
using System.Collections;

public class RotateWall : MonoBehaviour {

	private PullApart pullApart;
	public GameObject puzzleTrigger;
	private float puzzleBreakDistance;
	private float currentZRotation;
	private Quaternion wallRotation;

	// Use this for initialization
	void Start () {
		pullApart = puzzleTrigger.GetComponent<PullApart>();
		puzzleBreakDistance = pullApart.breakDistance;
		wallRotation = new Quaternion(0, 0, 0, 1);
	}
	
	// Update is called once per frame
	void Update () {
		if(puzzleTrigger != null)
			currentZRotation = -45.0f*(pullApart.currentDistance/puzzleBreakDistance);

		wallRotation.eulerAngles = new Vector3(0, 0, currentZRotation);
		transform.rotation = wallRotation;

		if(puzzleTrigger == null || puzzleTrigger.GetComponent<PullApart>().extended == true)
		{
			wallRotation.eulerAngles = new Vector3(0, 0, -45);
			transform.rotation = wallRotation;
		}
	}
}
