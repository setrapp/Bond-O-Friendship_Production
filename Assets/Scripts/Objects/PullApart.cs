using UnityEngine;
using System.Collections;

public class PullApart : MonoBehaviour {

	public GameObject otherSphere;
	public GameObject connector;
	private float xScale;
	public float currentDistance;
	public float breakDistance = 5.0f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		connector.transform.position = (transform.position + otherSphere.transform.position)/2;
		xScale = Vector3.Distance(transform.position, otherSphere.transform.position);
		connector.transform.localScale = new Vector3(xScale, 2, 1);
		connector.transform.right = transform.position - otherSphere.transform.position;

		currentDistance = Vector3.Distance(transform.position, otherSphere.transform.position);

		Vector3 toOther = transform.position - otherSphere.transform.position;
		toOther.z = 0;
		transform.right = -toOther;
		otherSphere.transform.right = toOther;

		if(currentDistance > breakDistance)
		{
			Destroy(otherSphere);
			Destroy(connector);
			Destroy(gameObject);
		}
	}
}
