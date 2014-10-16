using UnityEngine;
using System.Collections;

public class LongDetail : MonoBehaviour {

	public Material myMaterial;
	
	public bool isHit = false;

	public bool allDone = false;

	private float myAlpha;
	private float fadeConst = 0.2f;
	public bool fading = false;
	public bool bright = false;

	public GameObject creator;
	
	// Use this for initialization
	void Start () {
	
		myAlpha = 0;
		bright = true;
		renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, myAlpha);
	}
	
	// Update is called once per frame
	void Update () {

		renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, myAlpha);

		if(fading == true)
		{
			if(myAlpha > 0)
				myAlpha -= Time.deltaTime * fadeConst;
			//print (myAlpha);
			//print (renderer.material.color);
			//print (gameObject);
		}
		
		if(bright == true)
		{
			if(myAlpha < 1)
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
			setHitOn();

			if (allDone == false)
			audio.Play();

			Invoke("setHitOff",10.0f);
		}
		
	}
	}
	
	void setHitOn ()
	{
		isHit = true;
		renderer.material.color = Color.blue;
	}
	
	void setHitOff ()
	{
		isHit = false;
		renderer.material = myMaterial;
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

	public void TurnOff()
	{
		renderer.enabled = false;
	}

}