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
	
	// Use this for initialization
	void Start () {
		triggerRipple = true;
		activated = false;
		r = 0.0f;
		g = 0.2f;
		b = 0.5f;
		a = 1.0f;

	}
	
	// Update is called once per frame
	void Update () {
		myColor = new Color (r,g,b,a);
		if (activated == true)
		{
			GetComponent<Renderer>().material.color = myColor;
			gameObject.GetComponent<Collider>().enabled = false;
		}

	}
		

	void OnTriggerEnter(Collider collide)
	{
		//print ("General");
		if(collide.gameObject.name == "Player 1")
		{
			FirePulse();
			activated = true;
		}
		if(collide.gameObject.name == "Player 2")
		{
			FirePulse();
			activated = true;
		}
		if(collide.gameObject.tag == "Pulse")
		{
			print ("HIT");
			MiniFire();
			activated = true;
		}



	}

	void FirePulse()
	{
		rippleObj = Instantiate(ripplePrefab,transform.position,Quaternion.Euler(new Vector3(90,0,0))) as GameObject;
		rippleObj.GetComponent<RingPulse>().scaleRate = 10.0f;
		rippleObj.GetComponent<RingPulse>().lifeTime = 3.0f;
		rippleObj.GetComponent<RingPulse>().alpha = 1.0f;
		rippleObj.GetComponent<RingPulse>().alphaFade = 0.2f;

		//triggerRipple = false;
	}

	void MiniFire()
	{
		smallrippleObj = Instantiate(smallripplePrefab,transform.position,Quaternion.Euler(new Vector3(90,0,0))) as GameObject;
		smallrippleObj.GetComponent<RingPulse>().scaleRate = 8.0f;
		smallrippleObj.GetComponent<RingPulse>().lifeTime = 1.5f;
		smallrippleObj.GetComponent<RingPulse>().alpha = 1.0f;
		smallrippleObj.GetComponent<RingPulse>().alphaFade = 0.7f;
		//print ("fire!");
	}
}
