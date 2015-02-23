using UnityEngine;
using System.Collections;

public class MiniNode : MonoBehaviour {

	public bool activated;
	public bool triggerRipple;
	private Color endColor;
	public Color myColor;
	
	private float r;
	private float g;
	private float b;
	private float a;
	
	public GameObject smallripplePrefab;
	private GameObject smallrippleObj;
	
	public float ScaleRate;
	public float LifeTime;
	
	private float scalerate;
	private float lifetime;
	
	private float alphafade;
	private float alpha;
	
	public float activationTimer;
	public float activeTime;

	public bool turnoff;
	
	// Use this for initialization
	void Start () {
		
		activeTime = 6.0f;
		activationTimer = activeTime;
		triggerRipple = true;
		activated = false;
		r = 0.25f;
		g = 0.0f;
		b = 0.75f;
		a = 1.0f;

		scalerate = 10.0f;
		lifetime = 3.0f;
		alpha = 1.0f;
		alphafade = 0.5f;
		
		myColor = new Color((191.0f/255.0f),(105.0f/255.0f),(255.0f/255.0f),a);

		turnoff = false;
		GetComponent<Renderer>().material.color = myColor;
	
	}
	
	// Update is called once per frame
	void Update () {
		endColor = new Color (r,g,b,a);
		
		if(turnoff == false)
		{
			
			if (activated == true)
			{
				activationTimer -= Time.deltaTime;
				//print ("activationTimer");
				GetComponent<Renderer>().material.color = endColor;
				gameObject.GetComponent<Collider>().enabled = false;
				a -= Time.deltaTime*2.0f;
			}
			
			if(activationTimer <= 0)
			{
				//print("deactivate");
				activated = false;
				activationTimer = activeTime;
				gameObject.GetComponent<Collider>().enabled = true;
				GetComponent<Renderer>().material.color = myColor;
				if(a<1.0f)
				{
					a += Time.deltaTime;
				}
			}
		}
		
		else if(turnoff == true)
		{
			GetComponent<Renderer>().material.color = endColor;
			gameObject.GetComponent<Collider>().enabled = false;
			a -= Time.deltaTime*2.0f;
		}
		
	}
	
	
	void OnTriggerEnter(Collider collide)
	{
		if(collide.gameObject.name == "Player 2")
		{
			MiniFire();
			activated = true;
		}
		if(collide.gameObject.name == "Player 1")
		{
			MiniFire();
			activated = true;
		}
		if(LayerMask.LayerToName(collide.gameObject.layer) == "Bond" )
		{
			activated = true;
		}
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