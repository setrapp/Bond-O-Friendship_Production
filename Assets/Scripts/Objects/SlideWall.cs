using UnityEngine;
using System.Collections;

public class SlideWall : MonoBehaviour {

	private PullApart pullApart;
	public GameObject puzzleTrigger;
	private float puzzleBreakDistance;
	private float scaling;
	private float originalYScale;
	private float originalYPos;

	
	// Use this for initialization
	void Start () {
		pullApart = puzzleTrigger.GetComponent<PullApart>();
		puzzleBreakDistance = pullApart.breakDistance;
		originalYScale = transform.localScale.y;
		originalYPos = transform.localPosition.y;
	}
	
	// Update is called once per frame
	void Update () {
		if(puzzleTrigger != null)
			scaling = pullApart.currentDistance/puzzleBreakDistance;

		transform.localScale = new Vector3(transform.localScale.x, originalYScale*(1 - scaling), transform.localScale.z);

		if(name == "Stone Gate")
			transform.localPosition = new Vector3(transform.localPosition.x, originalYPos + (originalYScale * 0.5f * scaling), transform.localPosition.z);

		if(name == "Stone Gate 2")
			transform.localPosition = new Vector3(transform.localPosition.x, originalYPos - (originalYScale * 0.5f * scaling), transform.localPosition.z);
	}
}
