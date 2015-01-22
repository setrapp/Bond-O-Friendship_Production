using UnityEngine;
using System.Collections;

public class SeasonsFloorPuzzle : MonoBehaviour {
	
	public GameObject outerRing;
	public GameObject gate;
	public GameObject emergencyEscapeGate;

	private bool colored = false;
	public float timer = 10.0f;
	public float maxTime = 10.0f;
	private Vector3 originalSize;
	private Color originalColor;
	public static int leftColorCount = 0;
	public static int rightColorCount = 0;
	private bool puzzleComplete;

	// Use this for initialization
	void Start () {
		outerRing.GetComponent<Renderer>().material.color = new Color(0, 0, 0, 0);
		originalSize = transform.localScale;
		originalColor = GetComponent<Renderer>().material.color;
	}
	
	// Update is called once per frame
	void Update () {
		if(colored == true && transform.parent.name == "Left Floor Puzzle" && leftColorCount != 4)
		{
			timer -= Time.deltaTime;
			transform.localScale = new Vector3((timer/maxTime)*originalSize.x, (timer/maxTime)*originalSize.y, originalSize.z);
			if(timer <= 0)
			{
				timer = maxTime;
				transform.localScale = originalSize;
				colored = false;
				GetComponent<Renderer>().material.color = originalColor;
				leftColorCount--;
			}
		}
		if(colored == true && transform.parent.name == "Right Floor Puzzle" && rightColorCount != 4)
		{
			timer -= Time.deltaTime;
			transform.localScale = new Vector3((timer/maxTime)*originalSize.x, (timer/maxTime)*originalSize.y, originalSize.z);
			if(timer <= 0)
			{
				timer = maxTime;
				transform.localScale = originalSize;
				colored = false;
				GetComponent<Renderer>().material.color = originalColor;
				rightColorCount--;
			}
		}
		if(leftColorCount == 4 && puzzleComplete == false && transform.parent.name == "Left Floor Puzzle")
		{
			outerRing.GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color;
			if(gate.name == "Forcefield Gate 1")
			{
				Destroy(gate);
				Destroy(emergencyEscapeGate);
			}
			transform.localScale = originalSize;
			puzzleComplete = true;
		}
		if(rightColorCount == 4 && puzzleComplete == false && transform.parent.name == "Right Floor Puzzle")
		{
			outerRing.GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color;
			if(gate.name == "Forcefield Gate 2")
			{
				Destroy(gate);
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
			if(transform.parent.name == "Left Floor Puzzle" && leftColorCount < 4  && colored == false)
				leftColorCount++;
			if(transform.parent.name == "Right Floor Puzzle" && rightColorCount < 4  && colored == false)
				rightColorCount++;
			if(puzzleComplete == true)
				outerRing.GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color;
			colored = true;
		}
	}
}
