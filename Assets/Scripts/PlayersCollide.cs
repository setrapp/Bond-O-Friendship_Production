using UnityEngine;
using System.Collections;

public class PlayersCollide : MonoBehaviour {

	public ParticleSystem collisionParticlePrefab;
	private ParticleSystem collisionParticle;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter (Collision col) {

		if(col.transform.name == "Player 1")
		{
			collisionParticle = (ParticleSystem)Instantiate(collisionParticlePrefab);
			collisionParticle.transform.position = (col.transform.position + transform.position)/2;
			Destroy(collisionParticle.gameObject, 1.0f);
		}
		else{
			collisionParticle = (ParticleSystem)Instantiate(collisionParticlePrefab);
			collisionParticle.transform.position =  col.contacts[0].point;
			Destroy(collisionParticle.gameObject, 1.0f);
		}
	}
}
