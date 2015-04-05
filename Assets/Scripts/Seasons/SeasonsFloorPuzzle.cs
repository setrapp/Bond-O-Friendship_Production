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
	public float gateCloseTime;

	private bool colored = false;
	private bool puzzleComplete;
	private Vector3 originalSize;
	private Color originalColor;
	private static int[] groups;
	private bool gateClosing;
	private float gateCloseSpeed;
	private float gateXPos;

	// Use this for initialization
	void Start () {
		outerRing.GetComponent<Renderer>().material.color = new Color(0, 0, 0, 0);
		originalSize = transform.localScale;
		originalColor = GetComponent<Renderer>().material.color;
		groups = new int[groupTotal];
		for(int i = 0; i < groupTotal; i++)
			groups[i] = 0;
		gateCloseSpeed = gate.transform.localScale.x/(gateCloseTime*120);
		gateXPos = gate.transform.position.x - gate.transform.localScale.x/2;
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
				gateClosing = true;
				Destroy(gate, gateCloseTime);
			}
			if(emergencyEscapeGate != null)
			{
				Destroy(emergencyEscapeGate);
			}
			transform.localScale = originalSize;
			puzzleComplete = true;
		}
		if(gateClosing == true && gate !=null)
		{
			gate.transform.localScale = new Vector3(gate.transform.localScale.x - gateCloseSpeed, gate.transform.localScale.y, gate.transform.localScale.z);
			gate.transform.position = new Vector3(gateXPos + gate.transform.localScale.x/2, gate.transform.position.y, gate.transform.position.z);
		}
	}

	void AttachFluff(Fluff fluff)
	{
		if(fluff.transform.tag == "Fluff" && fluff != null)
		{
			if (fluff.creator != null)
			{
				GetComponent<Renderer>().material.color = fluff.creator.attachmentColor;
			}
			if(groups[groupNumber] < totalTargets  && colored == false)
				groups[groupNumber]++;
			if(puzzleComplete == true)
				outerRing.GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color;
			colored = true;
			fluff.PopFluff();
		}
	}
}
