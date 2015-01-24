using UnityEngine;
using System.Collections;

public class SeasonsFloorPuzzle : MonoBehaviour {
	
	public GameObject outerRing;
	public GameObject gate;
	public GameObject emergencyEscapeGate;
	
	public float timer;
	public float maxTime;
	public int totalTargets;
	public int groupNumber;
	public int groupTotal;

	private bool colored = false;
	private bool puzzleComplete;
	private Vector3 originalSize;
	private Color originalColor;
	private int[] groups;

	// Use this for initialization
	void Start () {
		outerRing.GetComponent<Renderer>().material.color = new Color(0, 0, 0, 0);
		originalSize = transform.localScale;
		originalColor = GetComponent<Renderer>().material.color;
		groups = new int[groupTotal];
	}
	
	// Update is called once per frame
	void Update () {
		if(colored == true && groups[groupNumber] != totalTargets)
		{
			timer -= Time.deltaTime;
			transform.localScale = new Vector3((timer/maxTime)*originalSize.x, (timer/maxTime)*originalSize.y, originalSize.z);
			if(timer <= 0)
			{
				timer = maxTime;
				transform.localScale = originalSize;
				colored = false;
				GetComponent<Renderer>().material.color = originalColor;
				groups[groupNumber]--;
			}
		}

		if(groups[groupNumber] == totalTargets && puzzleComplete == false)
		{
			outerRing.GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color;
			if(gate != null)
			{
				Destroy(gate);
			}
			if(emergencyEscapeGate != null)
			{
				Destroy(emergencyEscapeGate);
			}
			transform.localScale = originalSize;
			puzzleComplete = true;
		}
	}

	void OnCollisionEnter (Collision col) {
		Fluff fluff = col.gameObject.GetComponent<Fluff>();
		if(col.transform.tag == "Fluff" && fluff != null && fluff.creator != null)
		{
			GetComponent<Renderer>().material.color = fluff.creator.attachmentColor;
			if(groups[groupNumber] < totalTargets  && colored == false)
				groups[groupNumber]++;
			if(puzzleComplete == true)
				outerRing.GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color;
			colored = true;
		}
	}
}
