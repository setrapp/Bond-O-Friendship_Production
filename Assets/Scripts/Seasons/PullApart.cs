using UnityEngine;
using System.Collections;

public class PullApart : MonoBehaviour {

	public GameObject otherSphere;
	public GameObject connector;
	private float xScale;
	public float currentDistance;
	public float breakDistance = 5.0f;

	public GameObject Symbol1;
	public GameObject Symbol2;
	public GameObject Shadow1;
	public GameObject Shadow2;

	private Color Symbol;
	private Color Shadow;
	private Color ConnectorCol;
	private float alpha;

	public GameObject smallripplePrefab;
	private GameObject smallrippleObj;
	private GameObject smallripple2;

	public bool rippleShot;
	public bool extended;

	// Use this for initialization
	void Start () {
		alpha = 1.0f;
		rippleShot = false;
		extended = false;


	}
	
	// Update is called once per frame
	void Update () {
		Shadow = new Color((161.0f/255.0f),(97.0f/255.0f),(62.0f/255.0f),alpha);
		Symbol = new Color((254.0f/255.0f),(113.0f/255.0f),(54.0f/255.0f),alpha);
		ConnectorCol = new Color((20.0f/255.0f),(164.0f/255.0f),(223.0f/255.0f),alpha); 

		Symbol1.GetComponent<Renderer>().material.color = Symbol;
		Symbol2.GetComponent<Renderer>().material.color = Symbol;
		Shadow1.GetComponent<Renderer>().material.color = Shadow;
		Shadow2.GetComponent<Renderer>().material.color = Shadow;
		connector.GetComponent<Renderer>().material.color = ConnectorCol;

		connector.transform.position = (transform.position + otherSphere.transform.position)/2;
		xScale = Vector3.Distance(transform.position, otherSphere.transform.position);
		connector.transform.localScale = new Vector3(xScale, 2, 1);
		connector.transform.right = transform.position - otherSphere.transform.position;

		currentDistance = Vector3.Distance(transform.position, otherSphere.transform.position);

		Vector3 toOther = transform.position - otherSphere.transform.position;
		toOther.z = 0;
		//transform.forward = otherSphere.transform.forward = Vector3.forward;
		transform.right = -toOther;
		Vector3 rot = transform.rotation.eulerAngles;
		if (rot.y == 180)
		{
			rot.z = rot.y;
			rot.y = 0;
			transform.rotation = Quaternion.Euler(rot);
		}

		otherSphere.transform.right = toOther;
		Vector3 otherRot = otherSphere.transform.rotation.eulerAngles;
		if (otherRot.y == 180)
		{
			otherRot.z = otherRot.y;
			otherRot.y = 0;
			otherSphere.transform.rotation = Quaternion.Euler(otherRot);
		}


		if(currentDistance > breakDistance)
		{
			extended = true;
			MiniFire();
			alpha -= Time.deltaTime*0.7f;
		}

		if(alpha <= 0)
		{
			Destroy(otherSphere);
			Destroy(connector);
			Destroy(gameObject);
		}
	}

	void MiniFire()
	{
		if(rippleShot == false)
		{
			smallrippleObj = Instantiate(smallripplePrefab,Symbol1.transform.position,Quaternion.identity) as GameObject;
			smallrippleObj.GetComponent<RingPulse>().scaleRate = 8.0f;
			smallrippleObj.GetComponent<RingPulse>().lifeTime = 1.5f;
			smallrippleObj.GetComponent<RingPulse>().alpha = 1.0f;
			smallrippleObj.GetComponent<RingPulse>().alphaFade = 0.7f;
			smallrippleObj.GetComponent<RingPulse>().mycolor = Color.white;
			//smallrippleObj.GetComponent<RingPulse>().smallRing = true;

			smallripple2 = Instantiate(smallripplePrefab,Symbol2.transform.position,Quaternion.identity) as GameObject;
			smallripple2.GetComponent<RingPulse>().scaleRate = 8.0f;
			smallripple2.GetComponent<RingPulse>().lifeTime = 1.5f;
			smallripple2.GetComponent<RingPulse>().alpha = 1.0f;
			smallripple2.GetComponent<RingPulse>().alphaFade = 0.7f;
			smallripple2.GetComponent<RingPulse>().mycolor = Color.white;
			//smallrippleObj.GetComponent<RingPulse>().smallRing = true;

			rippleShot = true;
		}
	}
}
