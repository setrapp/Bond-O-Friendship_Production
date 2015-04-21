using UnityEngine;
using System.Collections;

public class MovePaired : MonoBehaviour {

	public LayerMask ignoreLayers;
	public GameObject pairedObject;

	private bool wasFloating = false;
	private bool falling;
	private Vector3 pairStartPos;
	private Vector3 selfStartPos;

	// Use this for initialization
	void Start () {
		pairStartPos = pairedObject.transform.position;
		selfStartPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		pairedObject.transform.position = pairStartPos + (transform.position - selfStartPos);

		RaycastHit hit;
		if(!Physics.Raycast(pairedObject.transform.position, Vector3.forward, out hit, Mathf.Infinity, ~ignoreLayers))
		{
			if (!wasFloating)
			{
				wasFloating = true;
				falling = true;
			}
		}
		if(falling)
		{
			transform.localScale -= new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime);
			pairedObject.transform.localScale -= new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime)*6.0f;
			if(transform.localScale.x <= 0)
			{
				Destroy(pairedObject);
				Destroy(gameObject);
			}
		}
	}
}
