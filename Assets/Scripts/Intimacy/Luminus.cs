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
	private bool playerInRange;
	private float intense;
	private float range;
	public float timeMult;

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

		myLight.range = range;
		myLight.intensity = intense;

		player1Dist = Vector3.Distance (transform.position, player1.transform.position);
		player2Dist = Vector3.Distance (transform.position, player2.transform.position);

		if ((player1Dist <= maxDist) && (player2Dist <= maxDist)) {
			playerInRange = true;
			//print ("in range");
		} else if ((player1Dist > maxDist) && (player2Dist > maxDist)) {
			playerInRange = false;
			//print ("out of range");
		}

		if (playerInRange == false) {
			isOn = false;
		}
	

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
				print ("isOn");
				Helper.FirePulse(transform.position, Globals.Instance.defaultPulseStats);

			}
		}
	}
}
