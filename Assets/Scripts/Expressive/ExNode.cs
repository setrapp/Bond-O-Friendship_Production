using UnityEngine;
using System.Collections;

public class ExNode : MonoBehaviour {

	public bool activated;
	public bool triggerRipple;
	private Color myColor;
	private float r;
	private float g;
	private float b;
	private float a;
	
	public GameObject ripplePrefab;
	public GameObject smallripplePrefab;
	private GameObject smallrippleObj;
	private GameObject rippleObj;

	public float ScaleRate;
	public float LifeTime;

	private float scalerate;
	private float lifetime;
	private float alphafade;
	private float alpha;

	// Use this for initialization
	void Start () {
		triggerRipple = true;
		activated = false;
		r = 0.25f;
		g = 0.0f;
		b = 0.75f;
		a = 1.0f;
		GetComponent<Renderer>().material.color = Color.white;
		scalerate = 10.0f;
		lifetime = 3.0f;
		alpha = 1.0f;
		alphafade = 0.5f;


	}
	
	// Update is called once per frame
	void Update () {
		myColor = new Color (r,g,b,a);
		if (activated == true)
		{
			GetComponent<Renderer>().material.color = myColor;
			gameObject.GetComponent<Collider>().enabled = false;
			a -= Time.deltaTime*2.0f;
		}

	}
		

	void OnTriggerEnter(Collider collide)
	{
		//print ("General");
		if(collide.gameObject.name == "Player 1")
		{
			FirePulse(scalerate,lifetime,alpha,alphafade);
			activated = true;
		}
		if(collide.gameObject.name == "Player 2")
		{
			FirePulse(scalerate,lifetime,alpha,alphafade);
			activated = true;
		}
		if(collide.gameObject.tag == "Pulse")
		{
			//print ("HIT");
			MiniFire();
			activated = true;
		}
		if(LayerMask.LayerToName(collide.gameObject.layer) == "Bond" )
		{
			FirePulse(11.0f,5.0f,1.0f,0.2f);
			activated = true;
		}
		if(collide.gameObject.tag == "Fluff" && collide.gameObject.GetComponent<Fluff>().moving == true)
		{
			MiniFire();
			activated = true;
		}
		

	}

	void FirePulse(float a, float b, float c, float d)
	{
		rippleObj = Instantiate(ripplePrefab,transform.position,Quaternion.identity) as GameObject;
		rippleObj.GetComponent<RingPulse>().scaleRate = a;
		rippleObj.GetComponent<RingPulse>().lifeTime = b;
		rippleObj.GetComponent<RingPulse>().alpha = c;
		rippleObj.GetComponent<RingPulse>().alphaFade = d;
		rippleObj.GetComponent<RingPulse>().smallRing = false;


		//triggerRipple = false;
	}

	void MiniFire()
	{
		smallrippleObj = Instantiate(smallripplePrefab,transform.position,Quaternion.identity) as GameObject;
		smallrippleObj.GetComponent<RingPulse>().scaleRate = 8.0f;
		smallrippleObj.GetComponent<RingPulse>().lifeTime = 1.5f;
		smallrippleObj.GetComponent<RingPulse>().alpha = 1.0f;
		smallrippleObj.GetComponent<RingPulse>().alphaFade = 0.7f;
		smallrippleObj.GetComponent<RingPulse>().smallRing = true;
		//print ("fire!");
	}
}
