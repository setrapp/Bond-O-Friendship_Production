using UnityEngine;
using System.Collections;

public class DeepDetail : MonoBehaviour {

public Material myMaterial;
	
public bool isHit = false;
public bool allDone = false;
private bool isHitOnce = false;
private bool waiting = false;

private float myAlpha;
private float fadeConst = 0.2f;
public bool fading = false;
public bool bright = false;

public GameObject creator;
	
// Use this for initialization
void Start () {

		myAlpha = 1;
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

	if(allDone == true)
		{
			renderer.material.color = Color.blue;
		}

	}
	
	void OnTriggerEnter (Collider collide)
	{
		if (collide.gameObject.tag == "Converser" && collide.gameObject != creator)
		{
			PartnerLink creatorLink = creator.GetComponent<PartnerLink>();
			PartnerLink colliderLink = collide.gameObject.GetComponent<PartnerLink>();
			if (creatorLink != null && colliderLink != null && creatorLink.Partner == colliderLink)
			{
			if(isHitOnce == false && isHit == false)
			{
				setHitOnce();
				waiting = true;
				Invoke("setWaiting", 1.0f);
			}

			if(isHitOnce == true && waiting == false)
				setHitOn();

			if(isHit == true)
				setHitOn();
			if(allDone == false)
			audio.Play();
		}
		
	}
	}
	
	
	void setHitOnce ()
	{
		isHitOnce = true;
		renderer.material.color = Color.red; 
		Invoke("setHitOnceOff",12.0f);
	}
	
	void setHitOnceOff ()
	{
		isHitOnce = false;
		renderer.material = myMaterial;  
	}
	
	void setHitOn ()
	{
		isHit = true;
		renderer.material.color = Color.blue;
		Invoke("setHitOff",12.0f);
	}
	
	void setHitOff ()
	{
		isHit = false;
		setHitOnce();
		renderer.material.color = Color.red;
	}

	void setWaiting ()
	{
		waiting = false;
	}

	void IsHitOff ()
	{
		isHit = false;
		allDone = true;
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