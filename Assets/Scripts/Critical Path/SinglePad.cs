using UnityEngine;
using System.Collections;

public class SinglePad : MonoBehaviour {

	public Color activatedColor;
	public float gateSpeed;
	public GameObject gate;

	private static int onPadCount;
	private Color startingcolor;
	private float gateLength;
	private float startY;

	// Use this for initialization
	void Start () {
		gateLength = gate.transform.localScale.y;
		startingcolor = GetComponent<Renderer>().material.color;
		startY = gate.transform.localPosition.y;
	}
	
	// Update is called once per frame
	void Update () {

		if(onPadCount > 0 && gate != null)
		{
			GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, activatedColor, Time.deltaTime);
			gate.transform.localScale = new Vector3(gate.transform.localScale.x, gate.transform.localScale.y - Time.deltaTime*gateSpeed, gate.transform.localScale.z);
			gate.transform.Translate(new Vector3(0, -0.006f, 0));
			if(gate.transform.localScale.y <= 0)
				Destroy(gate);
		}
		if(onPadCount == 0 && gate != null)
		{
			GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, startingcolor, Time.deltaTime);
			if(gate.transform.localScale.y < gateLength)
			{
				gate.transform.localScale = new Vector3(gate.transform.localScale.x, gate.transform.localScale.y + Time.deltaTime*gateSpeed*2, gate.transform.localScale.z);
				gate.transform.Translate(new Vector3(0, 0.012f, 0));
			}
		}
	}

	void OnTriggerEnter (Collider col) {
		if(col.name == "Player 1" || col.name == "Player 2")
			onPadCount++;
	}

	void OnTriggerExit (Collider col) {
		if(col.name == "Player 1" || col.name == "Player 2")
			onPadCount--;
	}
}
