using UnityEngine;
using System.Collections;

public class PaletteNorm : MonoBehaviour {

	
	public GameObject ripplePrefab;
	private GameObject rippleObj;
	public Vector3 collidePos;

	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider collide)
	{
		if(collide.gameObject.name == "Player 1" || collide.gameObject.name == "Player 2")
		{
			collidePos = collide.transform.position;
			Fire();
			collide.gameObject.GetComponent<Paint>().origColor = true;
			
		}
		
	}
	
	void Fire()
	{
		rippleObj = Instantiate(ripplePrefab,collidePos,Quaternion.identity) as GameObject;
		rippleObj.GetComponent<Collider>().enabled = false;
		rippleObj.GetComponent<RingPulse>().smallRing = false;
		rippleObj.GetComponent<RingPulse>().scaleRate = 12.0f;
		rippleObj.GetComponent<RingPulse>().lifeTime = 1.0f;
		rippleObj.GetComponent<RingPulse>().alpha = 1.0f;
		rippleObj.GetComponent<RingPulse>().alphaFade = 2.0f;
	
	}
}