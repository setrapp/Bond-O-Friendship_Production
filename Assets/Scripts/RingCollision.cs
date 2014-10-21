using UnityEngine;
using System.Collections;

public class RingCollision : MonoBehaviour {

	public ParticleSystem ringCollisionParticle;
	private ParticleSystem collidedParticle;
	private Color p1Color;
	private Color p2Color;
	private float timer = 0.7f;

	// Use this for initialization
	void Start () {
		p1Color = new Color(0.263f, 0.451f, 0.874f);
		p2Color = new Color(0.188f, 0.956f, 0.161f);
	}
	
	// Update is called once per frame
	void Update () {
		if(collidedParticle != null)
		{
			timer -= Time.deltaTime;
			if(timer <= 0)
			{
				Destroy(collidedParticle);
				timer = 0.7f;
			}
		}
	}

	void OnTriggerEnter (Collider col) {
			collidedParticle = (ParticleSystem)Instantiate(ringCollisionParticle);
			collidedParticle.transform.position = col.transform.position;
			if(col.name == "Player 1")
				collidedParticle.startColor = p1Color;
			if(col.name == "Player 2")
				collidedParticle.startColor = p2Color;
	}
}
