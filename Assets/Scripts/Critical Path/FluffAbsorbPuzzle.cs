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

	public float duration = 2;
	private float timeSE = 0;
	private float timeSW = 0;
	private float timeNE = 0;
	private float timeNW = 0;

	private FluffStick fluffStick;
	private Color startColor;
	private bool complete;
	private bool finishedSE;
	private bool finishedSW;
	private bool finishedNE;
	private bool finishedNW;

	public AllowPlayerBond tempAllowBond;

	// Use this for initialization
	void Start () {
		fluffStick = GetComponentInChildren<FluffStick>();
		startColor = GetComponent<Renderer>().material.color;

		if (tempAllowBond != null)
		{
			tempAllowBond.AllowBond();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(fluffStick.stuckFluff != null && !complete)
		{
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
				{
					streamSE.transform.localScale -= new Vector3(Time.deltaTime, 0, 0)*streamScaleRate;
					timeSE = 0;
					finishedSE = false;
				}
				if(streamSW.transform.localScale.x > 0)
				{
					streamSW.transform.localScale -= new Vector3(Time.deltaTime, 0, 0)*streamScaleRate;
					finishedSW = false;
				}
				if(streamNE.transform.localScale.x > 0)
				{
					streamNE.transform.localScale -= new Vector3(Time.deltaTime, 0, 0)*streamScaleRate;
					finishedNE = false;
				}
				if(streamNW.transform.localScale.x > 0)
				{
					streamNW.transform.localScale -= new Vector3(Time.deltaTime, 0, 0)*streamScaleRate;				
					finishedNW = false;
				}

				GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, startColor, Time.deltaTime*0.2f);

				if(!finishedSE)
				{
					timeSE += Time.deltaTime / duration;
					
					timeSE = Mathf.Clamp(timeSE,0f,1f);
					endSE.GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, startColor, timeSE);
				}
				if(!finishedSW)
				{
					timeSW += Time.deltaTime / duration;
					
					timeSW = Mathf.Clamp(timeSW,0f,1f);
					endSW.GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, startColor, timeSW);
				}
				if(!finishedNE)
				{
					timeNE += Time.deltaTime / duration;
					
					timeNE = Mathf.Clamp(timeNE,0f,1f);
					endNE.GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, startColor, timeNE);
				}
				if(!finishedNW)
				{
					timeNW += Time.deltaTime / duration;
					
					timeNW = Mathf.Clamp(timeNW,0f,1f);
					endNW.GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, startColor, timeNW);
				}
			}
			if(finishedSE)
			{
				timeSE += Time.deltaTime / duration;

				timeSE = Mathf.Clamp(timeSE,0f,1f);
				endSE.GetComponent<Renderer>().material.color = Color.Lerp(startColor, fillingColor, timeSE);
			}
			if(finishedSW)
			{
				timeSW += Time.deltaTime / duration;
				
				timeSW = Mathf.Clamp(timeSW,0f,1f);
				endSW.GetComponent<Renderer>().material.color = Color.Lerp(startColor, fillingColor, timeSW);
			}
			if(finishedNE)
			{
				timeNE += Time.deltaTime / duration;
				
				timeNE = Mathf.Clamp(timeNE,0f,1f);
				endNE.GetComponent<Renderer>().material.color = Color.Lerp(startColor, fillingColor, timeNE);
			}
			if(finishedNW)
			{
				timeNW += Time.deltaTime / duration;
				
				timeNW = Mathf.Clamp(timeNW,0f,1f);
				endNW.GetComponent<Renderer>().material.color = Color.Lerp(startColor, fillingColor, timeNW);
			}



		}

		if(complete)
		{
			for(int i = 0; i < transform.parent.childCount; i++)
			{
				if(transform.parent.GetChild(i).name == "Cube")
					Destroy(transform.parent.GetChild(i).gameObject);
			}
			if(transform.parent.transform.localPosition.z < 8.0f)
				transform.parent.transform.localPosition += new Vector3(0, 0, Time.deltaTime);
			GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, finishedColor, Time.deltaTime*0.2f);
			endSE.GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, finishedColor, Time.deltaTime*0.2f);
			endSW.GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, finishedColor, Time.deltaTime*0.2f);
			endNE.GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, finishedColor, Time.deltaTime*0.2f);
			endNW.GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, finishedColor, Time.deltaTime*0.2f);
			if(streamSE.transform.localScale.y > 0)
				streamSE.transform.localScale -= new Vector3(0, Time.deltaTime, 0)*0.2f;
			if(streamSW.transform.localScale.y > 0)
				streamSW.transform.localScale -= new Vector3(0, Time.deltaTime, 0)*0.2f;
			if(streamNE.transform.localScale.y > 0)
				streamNE.transform.localScale -= new Vector3(0, Time.deltaTime, 0)*0.2f;
			if(streamNW.transform.localScale.y > 0)
				streamNW.transform.localScale -= new Vector3(0, Time.deltaTime, 0)*0.2f;
		}
	}
}
