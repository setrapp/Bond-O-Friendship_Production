using UnityEngine;
using System.Collections;

public class SimplePoint : MonoBehaviour {

	public AudioClip Gong;

	public bool pointMade;

	public GameObject lilPoint1;
	public GameObject lilPoint2;
	public GameObject lilPoint3;
	public GameObject lilPoint4;
	public GameObject lilPoint5;
	public GameObject lilPoint6;
	public GameObject lilPoint7;
	public GameObject lilPoint8;

	public float rotSpeed;
	public float timeConst = 50;
	private Vector3 rotVect;

	private float myAlpha;
	private float fadeConst = 0.2f;
	public bool fading = false;
	public bool bright = false;

	public float informationFactor;
	public GameObject creator;

	
	// Use this for initialization
	void Start () {

		rotSpeed = 4.0f;
		rotVect = new Vector3(0,0,1);	

		myAlpha = 0;
		bright = true;
		renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, myAlpha);
	
	}
	
	// Update is called once per frame
	void Update () {

		renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, myAlpha);

		if(fading == true)
		{
			if(myAlpha >= 0)
				myAlpha -= Time.deltaTime * fadeConst;
		}
		
		if(bright == true)
		{
			if(myAlpha <=1)
				myAlpha += Time.deltaTime * fadeConst;
		}

		transform.Rotate(rotVect * rotSpeed * Time.deltaTime);

		if(lilPoint1.GetComponent<Detail>().isHit && lilPoint2.GetComponent<Detail>().isHit && lilPoint3.GetComponent<Detail>().isHit && lilPoint4.GetComponent<Detail>().isHit &&
		   lilPoint5.GetComponent<Detail>().isHit && lilPoint6.GetComponent<Detail>().isHit && lilPoint7.GetComponent<Detail>().isHit && lilPoint8.GetComponent<Detail>().isHit) 
		{
			renderer.material.color = Color.cyan;
			//print("Good Point");
			pointMade = true;
			audio.PlayOneShot(Gong);
			//rotVect.y = 1;
			rotSpeed = 200.0f;
			if(rotSpeed > 50.0f)
			{
				rotSpeed -= Time.deltaTime * timeConst;
			}
			BroadcastMessage("IsHitOff");
			lilPoint1.GetComponent<Detail>().creator.BroadcastMessage("UnderstandPoint", informationFactor);
		}
	
	}

	public void IsFading()
	{
		fading = true;
		bright = false;
		//print ("Is fading");
	}
	
	public void IsBright()
	{
		fading = false;
		bright = true;
		//print ("Is Bright");
	}

}
