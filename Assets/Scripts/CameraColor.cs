using UnityEngine;
using System.Collections;

public class CameraColor : MonoBehaviour {

	public GameObject player;

	private Color myColor;
	private float rColor;
	private float gColor;
	private float bColor;

	private float fadeTime = 0.02f;
	private float desatTime = 0.05f;

	// Use this for initialization
	void Start () {

		rColor = 0;
		gColor = 0;
		bColor = 0;
		
	}
	
	// Update is called once per frame
	void Update () {

		myColor = new Color(Mathf.Max(rColor, 0.05f),Mathf.Max(rColor, 0.05f),Mathf.Max(rColor, 0.05f),0);
		camera.backgroundColor = myColor;

		if(player.GetComponent<PartnerLink>().Partner != null)
		{
		if(rColor < 0.2f)
		rColor += Time.deltaTime * fadeTime;
		if(bColor < 0.2f)
		bColor += Time.deltaTime * fadeTime;

		if(player.GetComponent<PartnerLink>().isgaining == true)
		{
			if(rColor < 0.3f)
			rColor += Time.deltaTime * fadeTime;
			if(bColor < 0.7f)
			bColor += Time.deltaTime * fadeTime;
			if(gColor < 0.5f)
			gColor += Time.deltaTime * fadeTime;
		}

		if(player.GetComponent<PartnerLink>().islagging == true)
		{
			if(rColor > 0.2f)
			rColor -= Time.deltaTime * desatTime;
			if(bColor > 0.2f)
			bColor -= Time.deltaTime * desatTime;
			if(gColor > 0.2f)
			gColor -= Time.deltaTime * desatTime;
		}

		
		}

		if(player.GetComponent<PartnerLink>().Partner == null)
		{
			if(rColor > 0)
			rColor -= Time.deltaTime * desatTime;
			if(gColor > 0)
			gColor -= Time.deltaTime * desatTime;
			if(bColor > 0)
			bColor -= Time.deltaTime * desatTime;
		}

		if(player.GetComponent<PartnerLink>().isLeadingnow)
		{
			if(bColor < 0.3f)
			bColor += Time.deltaTime * fadeTime;
			if(gColor < 0.2f)
			gColor += Time.deltaTime * fadeTime;
		}
		else
		{
			if(bColor > 0.2f)
			bColor -= Time.deltaTime * desatTime;
			if(gColor > 0f)
			gColor -= Time.deltaTime * desatTime;
		}

	}
}
