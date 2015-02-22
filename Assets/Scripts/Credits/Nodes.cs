using UnityEngine;
using System.Collections;

public class Nodes : MonoBehaviour {
	
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
	private int counter;
	private Color childColor;
	public float colorChangeRate = 0.1f;
	
	// Use this for initialization
	void Start () {
		triggerRipple = true;
		activated = false;
		counter = transform.parent.GetComponent<NodeParent>().childrenCounter;
		if(counter != 0)
			childColor = transform.parent.GetComponent<NodeParent>().childZeroColor;
		if(counter == 0)
		{
			r = Random.Range(0, 1.0f);
			g =Random.Range(0, 1.0f);
			b =Random.Range(0, 1.0f);
			counter++;
			transform.parent.GetComponent<NodeParent>().childZeroColor = new Color(r, g, b, 0);
		}
		else if(childColor.r + childColor.g + childColor.r < 1.5f)
		{
			r = childColor.r + colorChangeRate*counter;
			g = childColor.g + colorChangeRate*counter;
			b = childColor.b + colorChangeRate*counter;
			counter++;
		}
		else
		{
			r = childColor.r - colorChangeRate*counter;
			g = childColor.g - colorChangeRate*counter;
			b = childColor.b - colorChangeRate*counter;
			counter++;
		}
		transform.parent.GetComponent<NodeParent>().childrenCounter = counter;
		a = 0.0f;
		scalerate = 10.0f;
		lifetime = 3.0f;
		alpha = 1.0f;
		alphafade = 0.5f;
		GetComponent<Renderer>().material.color = new Color(r, g, b, a);
		
	}
	
	// Update is called once per frame
	void Update () {

		if (activated == true)
		{
			GetComponent<Renderer>().material.color = myColor;
			gameObject.GetComponent<Collider>().enabled = false;
			a += Time.deltaTime*2.0f;
			myColor = new Color (r,g,b,a);
			if(a >= 1.0f)
			{
				transform.parent.GetComponent<NodeParent>().activeChildren++;
				activated = false;
			}
		}
	}
	
	
	void OnTriggerEnter(Collider collide)
	{
		//print ("General");
		if(collide.gameObject.name == "Player 1")
		{
			FirePulse(scalerate,lifetime,alpha,alphafade,collide.transform.position);
			activated = true;
		}
		if(collide.gameObject.name == "Player 2")
		{
			FirePulse(scalerate,lifetime,alpha,alphafade,collide.transform.position);
			activated = true;
		}
		if(collide.gameObject.tag == "Pulse")
		{
			//print ("HIT");
			MiniFire(collide.transform.position);
			activated = true;
		}
		if(LayerMask.LayerToName(collide.gameObject.layer) == "Bond" )
		{
			FirePulse(11.0f,5.0f,1.0f,0.2f, collide.transform.position);
			activated = true;
		}
		
		
	}
	
	
	
	void FirePulse(float scaling, float b, float c, float d, Vector3 firePos)
	{
		rippleObj = Instantiate(ripplePrefab,firePos,Quaternion.identity) as GameObject;
		rippleObj.GetComponent<RingPulse>().scaleRate = scaling;
		rippleObj.GetComponent<RingPulse>().lifeTime = b;
		rippleObj.GetComponent<RingPulse>().alpha = c;
		rippleObj.GetComponent<RingPulse>().alphaFade = d;
		rippleObj.GetComponent<RingPulse>().smallRing = false;
		//triggerRipple = false;
	}
	
	void MiniFire(Vector3 firePos)
	{
		smallrippleObj = Instantiate(smallripplePrefab,firePos,Quaternion.identity) as GameObject;
		smallrippleObj.GetComponent<RingPulse>().scaleRate = 8.0f;
		smallrippleObj.GetComponent<RingPulse>().lifeTime = 1.5f;
		smallrippleObj.GetComponent<RingPulse>().alpha = 1.0f;
		smallrippleObj.GetComponent<RingPulse>().alphaFade = 0.7f;
		smallrippleObj.GetComponent<RingPulse>().smallRing = true;
		//print ("fire!");
	}
}
