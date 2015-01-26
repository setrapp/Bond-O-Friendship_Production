using UnityEngine;
using System.Collections;

public class CubePuzzle : MonoBehaviour {

	public GameObject destination;
	public GameObject player1;
	public GameObject player2;
	public ParticleSystem callPrefab;
	private ParticleSystem call;
	private float totalDistance;
	private float currentDistance;
	public GameObject startPosition;
	public GameObject wall;
	public ParticleSystem face;
	private ParticleSystem.Particle[] faceParticles;

	// Use this for initialization
	void Start () {
		totalDistance = Vector3.Distance(transform.position, destination.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		currentDistance = 1 - Vector3.Distance(transform.position, destination.transform.position)/totalDistance;
		GetComponent<Renderer>().material.color = new Color((currentDistance*0.2f + 0.1f), (currentDistance*0.9f + 0.1f), (currentDistance*0.2f + 0.1f));

		if((Vector3.Distance(player1.transform.position, transform.position) < 30.0f || Vector3.Distance(player2.transform.position, transform.position) < 30.0f) && call == null)
			call = (ParticleSystem)Instantiate(callPrefab);

		if(call != null)
			call.transform.position = transform.position;

		if(currentDistance > 0.08f && call != null)
			Destroy(call.gameObject);

		if(currentDistance > 0.9f)
		{
			transform.position = destination.transform.position;
			destination.GetComponent<Renderer>().material.color = Color.yellow;
			GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
			Destroy(wall);
		}

		if(transform.localPosition.z > -60.0f)
		{
			transform.position = startPosition.transform.position;
		}

	}

	void LateUpdate () {
		faceParticles = new ParticleSystem.Particle[face.particleCount];
		int count  = face.GetParticles(faceParticles);

		for(int i = 0; i < count; i++)
		{
			float yVel = -currentDistance*2.3f + 1 - ((1-(faceParticles[i].lifetime / faceParticles[i].startLifetime))*2) + ((1-(faceParticles[i].lifetime / faceParticles[i].startLifetime))*2)*currentDistance*2.3f;
			faceParticles[i].velocity = new Vector3(0, yVel, 1.5f);
		}
		
		face.SetParticles(faceParticles, count);
	}
	
	void OnCollisionEnter (Collision col) {

		if(col.transform.tag == "Character")
		{
			GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
		}

		if(col.transform.name == "Bond Collider")
		{
				GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
		}
	}

	void OnTriggerEnter (Collider col) {
		if(col.transform.tag == "Fluff")
		{
			transform.position = startPosition.transform.position;
		}
	}
}
