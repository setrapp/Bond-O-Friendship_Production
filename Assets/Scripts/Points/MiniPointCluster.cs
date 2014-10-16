using UnityEngine;
using System.Collections;

public class MiniPointCluster : MonoBehaviour {

	public AudioClip Gong;
	public float rotSpeed;
	private Vector3 rotDir;
	public float timeConst = 50;

	public GameObject point1;
	public GameObject point2;
	public GameObject point3;
	public GameObject point4;

	// Use this for initialization
	void Start () {
		
		rotSpeed = 6.0f;
		rotDir = new Vector3(0,0,-1);
		
	}
	
	// Update is called once per frame
	void Update () {
		
		transform.Rotate(rotDir * rotSpeed * Time.deltaTime);

		if(point1.GetComponent<MiniPoint>().isHit && point2.GetComponent<MiniPoint>().isHit && point3.GetComponent<MiniPoint>().isHit && point4.GetComponent<MiniPoint>().isHit)
		{
			audio.PlayOneShot(Gong);
			rotSpeed = 200.0f;
			if(rotSpeed > 50.0f)
			{
				rotSpeed -= Time.deltaTime * timeConst;
			}
			//rotDir.y = 1;
			//BroadcastMessage("IsHitOff");
		}
		
	}
}
