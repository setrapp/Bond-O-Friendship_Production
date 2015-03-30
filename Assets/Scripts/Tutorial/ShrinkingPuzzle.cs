using UnityEngine;
using System.Collections;

public class ShrinkingPuzzle : MonoBehaviour {

	private GameObject player1;
	private GameObject player2;
	private Color orange;
	private Color blue;
	private Color startingOrange;
	private Color startingBlue;
	public GameObject particlePrefab;
	private GameObject plantParticle;

	// Use this for initialization
	void Start () {
		player1 = Globals.Instance.player1.gameObject;
		player2 = Globals.Instance.player2.gameObject;
		startingOrange = new Color(0.65f, 0.2f, 0);
		startingBlue = new Color(0.05f, 0.28f, 0.52f);
		orange = new Color(1.0f, 0.61f, 0);
		blue = new Color(0.2f, 0.6f, 1.0f);
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		if(Physics.Raycast(player2.transform.position, -player2.transform.up, out hit) == true)
		{
			if(hit.transform.name == "OrangeMovingPuzzle")
			{
				hit.transform.gameObject.GetComponent<Renderer>().material.color = Color.Lerp(hit.transform.gameObject.GetComponent<Renderer>().material.color, orange, Time.deltaTime);
				hit.transform.localScale -= new Vector3(0.02f, 0.02f, 0);
				if(hit.transform.localScale.x <= 0.5f)
					Destroy(hit.transform.gameObject);
			}
//			else
//				hit.transform.gameObject.GetComponent<Renderer>().material.color = Color.Lerp(hit.transform.gameObject.GetComponent<Renderer>().material.color, startingOrange, Time.deltaTime);
		}
		if(Physics.Raycast(player1.transform.position, -player1.transform.up, out hit) == true)
		{
			if(hit.transform.name == "BlueMovingPuzzle")
			{
				hit.transform.gameObject.GetComponent<Renderer>().material.color = Color.Lerp(hit.transform.gameObject.GetComponent<Renderer>().material.color, blue, Time.deltaTime);
				hit.transform.localScale -= new Vector3(0.02f, 0.02f, 0);
				if(hit.transform.localScale.x <= 0.5f)
					Destroy(hit.transform.gameObject);
			}
//			else
//				hit.transform.gameObject.GetComponent<Renderer>().material.color = Color.Lerp(hit.transform.gameObject.GetComponent<Renderer>().material.color, startingBlue, Time.deltaTime);
		}
		if(transform.childCount == 0 && plantParticle == null)
		{
			plantParticle = (GameObject)Instantiate(particlePrefab);
			plantParticle.transform.position = transform.position;
		}

	}

	void OnTriggerEnter (Collider col) {

	}
}
