using UnityEngine;
using System.Collections;

public class ShootBreakShoot : MonoBehaviour {

	public GameObject pulsePrefab;
	private GameObject pulse;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	
	
	}
	void FirePulse()
	{
	pulse = Instantiate(pulsePrefab, transform.position, Quaternion.identity) as GameObject;
	}

}
