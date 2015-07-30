using UnityEngine;
using System.Collections;

public class Luminus : MonoBehaviour {

	public Light myLight;
	//public GameObject ring;
	private GameObject player1;
	private GameObject player2;
	private float player1Dist;
	private float player2Dist;
	private float maxDist;
	public bool isOn;
	public bool stayOn = false;
	private bool playerInRange;
	private float intense;
	private float range;
	public float timeMult;
	public float intensity = 0;
	public float fadeTime = 1;
	public bool fadingIn = true;

	// Use this for initialization
	void Start () {
		intense = 1.0f;
		range = 1.0f;
		maxDist = 25.0f;
		player1 = GameObject.Find("Player 1");
		player2 = GameObject.Find("Player 2");
		timeMult = 5.0f;
	
	}
	
	// Update is called once per frame
	void Update (){

		//myLight.range = range;
		myLight.intensity = 0;//myLight.intensity = intense;

		player1Dist = Vector3.Distance (transform.position, player1.transform.position);
		player2Dist = Vector3.Distance (transform.position, player2.transform.position);

		if ((player1Dist <= maxDist) && (player2Dist <= maxDist)) {
			playerInRange = true;
			//print ("in range");
		} else if ((player1Dist > maxDist) && (player2Dist > maxDist)) {
			playerInRange = false;
			//print ("out of range");
		}

		if (playerInRange == false && !stayOn) {
			isOn = false;
		}
	

		// Fade intensity.
		if (isOn && fadingIn && intensity < 1)
		{
			if (fadeTime > 0)
			{
				intensity += Time.deltaTime / fadeTime;
			}
			else
			{
				intensity = 1;
			}
		}
		else if (isOn && !fadingIn && intensity > 0)
		{
			if (fadeTime > 0)
			{
				intensity -= Time.deltaTime / fadeTime;
			}
			else
			{
				intensity = 0;
			}
		}
		intensity = Mathf.Clamp01(intensity);

		if (isOn == true) {
			if(intense < 5.0f)
			{
				intense += Time.deltaTime*timeMult;
			}
			if(range < 10.0f)
			{
				range += Time.deltaTime*timeMult;
			}
			//myLight.intensity = 5.0f;
			//myLight.range = 10.0f;

		} else if (isOn == false) {
			if(intense > 1.0f)
			{
				intense -= Time.deltaTime*timeMult;
			}
			if(range > 1.0f)
			{
				range -= Time.deltaTime*timeMult;
			}
			//myLight.range = 1.0f;
			//myLight.intensity = 1.0f;
		}
	}

	void OnTriggerEnter(Collider collide)
	{
		if (collide.gameObject.layer == LayerMask.NameToLayer("Bond")) 
		{
			if(isOn == false)
			{
				isOn = true;
				Helper.FirePulse(transform.position, Globals.Instance.defaultPulseStats);

			}
		}
	}
}
