using UnityEngine;
using System.Collections;

public class RingBreak : MonoBehaviour {

	public GameObject player1; 
	public GameObject player2;
	private Vector3 myPos;
	//private Transform p1Trans;
	//private Transform p2Trans;
	private float pOneDist;
	private float pTwoDist;
	
	public GameObject pulsePrefab;
	public GameObject pulse;
	//public GameObject pointer;
	//public Vector3 pointerTip; 
	private float timer;
	private Color mycolor;

	// Use this for initialization
	void Start () {

		player1 = GameObject.Find("Player 1");
		player2 = GameObject.Find("Player 2");
		//p1Trans = player1.transform;
		//p2Trans = player2.transform;
		timer = 2.0f;
		mycolor = GetComponent<Renderer>().material.color;
	
	}
	
	// Update is called once per frame
	void Update () {

		pOneDist = Vector3.Distance(transform.position, player1.transform.position);
		pTwoDist = Vector3.Distance(transform.position, player2.transform.position);

		if(pOneDist < 10.0f)
		{
			if(pOneDist < pTwoDist)
			{
				timer -= Time.deltaTime;
			}
		}
		
		if(pTwoDist < 10.0f)
		{
			if(pTwoDist < pOneDist)
			{
				timer -= Time.deltaTime;
			}
		}
		if(pTwoDist > 10.0f && pOneDist > 10.0f)
		{
			GetComponent<Renderer>().material.color = mycolor;
		}
		if(timer <= 1 && timer > 0)
		{
			GetComponent<Renderer>().material.color = Color.magenta;
		}
		else
			GetComponent<Renderer>().material.color = mycolor;

		if(timer<=0)
		{
			FirePulse();
			timer = 5.0f;
		}
	}
	void FirePulse()
	{
		pulse = Instantiate(pulsePrefab,transform.position,Quaternion.identity) as GameObject;
		pulse.transform.parent = GameObject.Find("Level").transform;
		//RingPulse enemyPulse = pulse.GetComponent<RingPulse>();
		//pulse.GetComponent<RingCollision>().creator = gameObject;
		pulse.GetComponent<Renderer>().material.color = Color.magenta;
		//enemyPulse.target = transform.forward;
		//enemyPulse.creator = gameObject;
	}
	

}
