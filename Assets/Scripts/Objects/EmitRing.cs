using UnityEngine;
using System.Collections;

public class EmitRing : MonoBehaviour {

	private GameObject ring;
	public GameObject ringPrefab;
	public float scaleRate = 0.2f;
	private float ringTimer;
	public float ringDuration = 3.0f;
	public ParticleSystem ringCollisionParticle;

	// Use this for initialization
	void Start () {
		ringTimer = ringDuration;
	}
	
	// Update is called once per framee
	void Update () {
		
		if(Input.GetKeyDown(KeyCode.E) && ring == null && gameObject.name == "Player 1")
		{
			ring = (GameObject)Instantiate(ringPrefab);
			ring.GetComponent<Collider>().isTrigger = true;
			ring.transform.position = transform.position;
			ring.GetComponent<RingCollision>().creator = gameObject;
			ring.GetComponent<Renderer>().material.color = GetComponent<BondAttachable>().attachmentColor;
		}

		if(Input.GetKeyDown(KeyCode.Keypad1) && ring == null && gameObject.name == "Player 2")
		{
			ring = (GameObject)Instantiate(ringPrefab);
			ring.GetComponent<Collider>().isTrigger = true;
			ring.transform.position = transform.position;
			ring.GetComponent<RingCollision>().creator = gameObject;
			ring.GetComponent<Renderer>().material.color = GetComponent<BondAttachable>().attachmentColor;
		}

		if(ring != null)
		{
			ring.transform.localScale += new Vector3((1 + scaleRate), (1 + scaleRate), 0);
			ringTimer -= Time.deltaTime;
			if(ringTimer <= 0)
			{
				ringTimer = ringDuration;
				Destroy(ring);
			}
		}
	}
}
