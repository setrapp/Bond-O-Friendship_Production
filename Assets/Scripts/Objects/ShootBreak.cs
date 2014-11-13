using UnityEngine;
using System.Collections;

public class ShootBreak : MonoBehaviour {

	public GameObject player1; 
	public GameObject player2;
	private Transform p1Trans;
	private Transform p2Trans;
	private float pOneDist;
	private float pTwoDist;

	public GameObject pulsePrefab;
	public GameObject pulse;
	public GameObject pointer;
	public Vector3 pointerTip; 
	private float timer;

	// Use this for initialization
	void Start () {
		player1 = GameObject.Find("Player 1");
		player2 = GameObject.Find("Player 2");
		p1Trans = player1.transform;
		p2Trans = player2.transform;
		timer = 4.0f;
	}
	
	// Update is called once per frame
	void Update () {
	
		pointerTip = pointer.transform.position;
		pointerTip.z = 0;

		pOneDist = Vector3.Distance(transform.position, player1.transform.position);
		pTwoDist = Vector3.Distance(transform.position, player2.transform.position);

		if(pOneDist < 20.0f)
		{
			if(pOneDist < pTwoDist)
			transform.LookAt(p1Trans);
		}

		if(pTwoDist < 20.0f)
		{
			if(pTwoDist < pOneDist)
			transform.LookAt(p2Trans);
		}

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
		if(timer <= 1 && timer > 0)
		{
			pointer.GetComponent<Renderer>().material.color = Color.magenta;
		}
		else
			pointer.GetComponent<Renderer>().material.color = Color.white;

		if(timer<=0)
		{
			FirePulse();
			timer = 2.0f;
		}
	}

	void FirePulse()
	{
		pulse = Instantiate(pulsePrefab,pointerTip,Quaternion.identity) as GameObject;
		EnemyPulse enemyPulse = pulse.GetComponent<EnemyPulse>();
		enemyPulse.target = transform.forward;
		enemyPulse.creator = gameObject;
	}
	

}
