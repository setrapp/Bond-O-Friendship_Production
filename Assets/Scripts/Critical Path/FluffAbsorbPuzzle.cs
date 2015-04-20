using UnityEngine;
using System.Collections;

public class FluffAbsorbPuzzle : MonoBehaviour {

	public GameObject streamSE;
	public GameObject streamSW;
	public GameObject streamNE;
	public GameObject streamNW;
	public GameObject endSE;
	public GameObject endSW;
	public GameObject endNE;
	public GameObject endNW;

	public Color fillingColor;
	public Color finishedColor;
	public float streamScaleRate;

	private FluffStick fluffStick;
	private Color startColor;
	private bool complete;
	private bool finishedSE;
	private bool finishedSW;
	private bool finishedNE;
	private bool finishedNW;

	// Use this for initialization
	void Start () {
		fluffStick = GetComponent<FluffStick>();
		startColor = GetComponent<Renderer>().material.color;
	}
	
	// Update is called once per frame
	void Update () {
		if(fluffStick.stuckFluff != null && !complete)
		{
			fluffStick.stuckFluff.transform.localScale -= new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime)*0.06f;
			if(fluffStick.stuckFluff.transform.localScale.x <= 0)
				Destroy(fluffStick.stuckFluff.gameObject);

			if(streamSE.transform.localScale.x < 30.0f)
				streamSE.transform.localScale += new Vector3(Time.deltaTime, 0, 0)*streamScaleRate;
			else
				finishedSE = true;
			if(streamSW.transform.localScale.x < 33.5f)
				streamSW.transform.localScale += new Vector3(Time.deltaTime, 0, 0)*streamScaleRate;
			else
				finishedSW = true;
			if(streamNE.transform.localScale.x < 63.5f)
				streamNE.transform.localScale += new Vector3(Time.deltaTime, 0, 0)*streamScaleRate;
			else
				finishedNE = true;
			if(streamNW.transform.localScale.x < 49.5f)
				streamNW.transform.localScale += new Vector3(Time.deltaTime, 0, 0)*streamScaleRate;
			else
				finishedNW = true;

			GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, fillingColor, Time.deltaTime*0.2f);
			if(finishedSE && finishedSW && finishedNE && finishedNW)
				complete = true;

		}

		if(!complete)
		{
			if(fluffStick.stuckFluff == null)
			{
				if(streamSE.transform.localScale.x > 0)
					streamSE.transform.localScale -= new Vector3(Time.deltaTime, 0, 0)*streamScaleRate;
				if(streamSW.transform.localScale.x > 0)
					streamSW.transform.localScale -= new Vector3(Time.deltaTime, 0, 0)*streamScaleRate;
				if(streamNE.transform.localScale.x > 0)
					streamNE.transform.localScale -= new Vector3(Time.deltaTime, 0, 0)*streamScaleRate;
				if(streamNW.transform.localScale.x > 0)
					streamNW.transform.localScale -= new Vector3(Time.deltaTime, 0, 0)*streamScaleRate;

				GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, startColor, Time.deltaTime*0.2f);
			}
			if(finishedSE)
				endSE.GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, fillingColor, Time.deltaTime*0.2f);
			if(finishedSW)
				endSW.GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, fillingColor, Time.deltaTime*0.2f);
			if(finishedNE)
				endNE.GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, fillingColor, Time.deltaTime*0.2f);
			if(finishedNW)
				endNW.GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, fillingColor, Time.deltaTime*0.2f);

			if(!finishedSE)
				endSE.GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, startColor, Time.deltaTime*0.2f);
			if(!finishedSW)
				endSW.GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, startColor, Time.deltaTime*0.2f);
			if(!finishedNE)
				endNE.GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, startColor, Time.deltaTime*0.2f);
			if(!finishedNW)
				endNW.GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, startColor, Time.deltaTime*0.2f);
		}

		if(complete)
		{
			transform.parent.transform.position += new Vector3(0, 0, Time.deltaTime);
			GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, finishedColor, Time.deltaTime*0.2f);
			endSE.GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, finishedColor, Time.deltaTime*0.2f);
			endSW.GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, finishedColor, Time.deltaTime*0.2f);
			endNE.GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, finishedColor, Time.deltaTime*0.2f);
			endNW.GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, finishedColor, Time.deltaTime*0.2f);
		}
	}
}
