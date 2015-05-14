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
	public GameObject bedSE;
	public GameObject bedSW;
	public GameObject bedNE;
	public GameObject bedNW;

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
			if(streamSE.transform.localScale.x < bedSE.transform.localScale.x)
			{
				streamSE.transform.localScale += new Vector3(Time.deltaTime, 0, 0)*streamScaleRate;
				streamSE.transform.localPosition += new Vector3(Time.deltaTime, -Time.deltaTime, 0);
			}
			else
				finishedSE = true;
			if(streamSW.transform.localScale.x < bedSW.transform.localScale.x)
			{
				streamSW.transform.localScale += new Vector3(Time.deltaTime, 0, 0)*streamScaleRate;
				streamSW.transform.localPosition += new Vector3(-Time.deltaTime, -Time.deltaTime, 0);
			}
			else
				finishedSW = true;
			if(streamNE.transform.localScale.x < bedNE.transform.localScale.x)
			{
				streamNE.transform.localScale += new Vector3(Time.deltaTime, 0, 0)*streamScaleRate;
				streamNE.transform.localPosition += new Vector3(Time.deltaTime, Time.deltaTime, 0);
			}
			else
				finishedNE = true;
			if(streamNW.transform.localScale.x < bedNW.transform.localScale.x)
			{
				streamNW.transform.localScale += new Vector3(Time.deltaTime, 0, 0)*streamScaleRate;
				streamNW.transform.localPosition += new Vector3(-Time.deltaTime, Time.deltaTime, 0);
			}
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
					streamSE.transform.localPosition -= new Vector3(Time.deltaTime, -Time.deltaTime, 0);
					timeSE = 0;
					finishedSE = false;
				}
				if(streamSW.transform.localScale.x > 0)
				{
					streamSW.transform.localScale -= new Vector3(Time.deltaTime, 0, 0)*streamScaleRate;
					streamSW.transform.localPosition -= new Vector3(-Time.deltaTime, -Time.deltaTime, 0);
					timeSW = 0;
					finishedSW = false;
				}
				if(streamNE.transform.localScale.x > 0)
				{
					streamNE.transform.localScale -= new Vector3(Time.deltaTime, 0, 0)*streamScaleRate;
					streamNE.transform.localPosition -= new Vector3(Time.deltaTime, Time.deltaTime, 0);
					timeNE = 0;
					finishedNE = false;
				}
				if(streamNW.transform.localScale.x > 0)
				{
					streamNW.transform.localScale -= new Vector3(Time.deltaTime, 0, 0)*streamScaleRate;
					streamNW.transform.localPosition -= new Vector3(-Time.deltaTime, Time.deltaTime, 0);
					timeNW = 0;
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
			if(streamSE.transform.localScale.x > 0)
			{
				streamSE.transform.localScale -= new Vector3(Time.deltaTime, 0, 0)*streamScaleRate;
				float dist =  streamSE.transform.localScale.x/streamScaleRate;
				endSE.transform.position = streamSE.transform.position + new Vector3(-dist,-dist, 0);
				streamSE.transform.localPosition -= new Vector3(Time.deltaTime, -Time.deltaTime, 0);
			}
			if(streamSW.transform.localScale.x > 0)
			{
				streamSW.transform.localScale -= new Vector3(Time.deltaTime, 0, 0)*streamScaleRate;
				float dist =  streamSW.transform.localScale.x/streamScaleRate;
				endSW.transform.position = streamSW.transform.position + new Vector3(-dist,dist, 0);
				streamSW.transform.localPosition -= new Vector3(-Time.deltaTime, -Time.deltaTime, 0);
			}
			if(streamNE.transform.localScale.x > 0)
			{
				streamNE.transform.localScale -= new Vector3(Time.deltaTime, 0, 0)*streamScaleRate;
				float dist =  streamNE.transform.localScale.x/streamScaleRate;
				endNE.transform.position = streamNE.transform.position + new Vector3(dist,-dist, 0);
				streamNE.transform.localPosition -= new Vector3(Time.deltaTime, Time.deltaTime, 0);
			}
			if(streamNW.transform.localScale.x > 0)
			{
				streamNW.transform.localScale -= new Vector3(Time.deltaTime, 0, 0)*streamScaleRate;
				float dist =  streamNW.transform.localScale.x/streamScaleRate;
				endNW.transform.position = streamNW.transform.position + new Vector3(dist, dist, 0);
				streamNW.transform.localPosition -= new Vector3(-Time.deltaTime, Time.deltaTime, 0);
			}
			if(bedSE != null)
				Destroy(bedSE);
			if(bedSW != null)
				Destroy(bedSW);
			if(bedNE != null)
				Destroy(bedNE);
			if(bedNW != null)
				Destroy(bedNW);
			GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, finishedColor, Time.deltaTime*0.2f);
			endSE.GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, finishedColor, Time.deltaTime*0.2f);
			endSW.GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, finishedColor, Time.deltaTime*0.2f);
			endNE.GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, finishedColor, Time.deltaTime*0.2f);
			endNW.GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, finishedColor, Time.deltaTime*0.2f);
		}
	}
}
