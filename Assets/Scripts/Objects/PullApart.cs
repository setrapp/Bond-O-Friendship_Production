using UnityEngine;
using System.Collections;

public class PullApart : MonoBehaviour {

//	private GameObject player1;
//	private GameObject player2;
	public GameObject otherSphere;
	public GameObject connector;
	private float xScale;
	public float currentDistance;
	public float breakDistance = 5.0f;

//	private float p1SelfDis;
//	private float p2SelfDis;
//	private float p1OtherDis;
//	private float p2OtherDis;
//	private float pullRange = 8.0f;

	// Use this for initialization
	void Start () {
//		player1 = GameObject.Find("Player 1");
//		player2 = GameObject.Find("Player 2");
	}
	
	// Update is called once per frame
	void Update () {
//		p1SelfDis = Vector3.Distance(transform.position, player1.transform.position);
//		p2SelfDis = Vector3.Distance(transform.position, player2.transform.position);
//		p1OtherDis = Vector3.Distance(otherSphere.transform.position, player1.transform.position);
//		p2OtherDis = Vector3.Distance(otherSphere.transform.position, player2.transform.position);
//
//		if(player1.GetComponent<PartnerLink>().absorbing == true && player2.GetComponent<PartnerLink>().absorbing == true)
//		{
//			if(p1SelfDis < pullRange || p2SelfDis < pullRange || p1OtherDis < pullRange || p2OtherDis < pullRange)
//			{
//				GetComponent<SpringJoint>().spring = 2;
//			}
//		}
//		else
//			GetComponent<SpringJoint>().spring = 300;

		connector.transform.position = (transform.position + otherSphere.transform.position)/2;
		xScale = Vector3.Distance(transform.position, otherSphere.transform.position);
		connector.transform.localScale = new Vector3(xScale, 2, 1);
		connector.transform.right = transform.position - otherSphere.transform.position;

		currentDistance = Vector3.Distance(transform.position, otherSphere.transform.position);

		if(currentDistance > breakDistance)
		{
			Destroy(otherSphere);
			Destroy(connector);
			Destroy(gameObject);
		}
	}
}
