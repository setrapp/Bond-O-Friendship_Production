using UnityEngine;
using System.Collections;

public class PushPlayers : MonoBehaviour {

	private GameObject player1;
	private GameObject player2;
	private float timer = 0.3f;
	private Vector3 expPos;
	private bool collided;
	private bool timerBool;
	private float p1Vel;
	private float p2Vel;
	public ParticleSystem collisionParticlePrefab;
	private ParticleSystem collisionParticle;

	// Use this for initialization
	void Start () {
		player1 = Globals.Instance.player1.gameObject;
		player2 = Globals.Instance.player2.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if(player1.GetComponent<FloatMoving>().collided == true)
		{
			expPos = (player1.transform.position + player2.transform.position)/2;
			collisionParticle = (ParticleSystem)Instantiate(collisionParticlePrefab);
			collisionParticle.transform.position = expPos;
			p1Vel = player1.GetComponent<CharacterComponents>().mover.velocity.sqrMagnitude;
			p2Vel = player2.GetComponent<CharacterComponents>().mover.velocity.sqrMagnitude;
			player1.GetComponent<Rigidbody>().AddExplosionForce(3000.0f/(p1Vel/200 + 1), expPos, 1000.0f);
			player2.GetComponent<Rigidbody>().AddExplosionForce(3000.0f/(p2Vel/200 + 1), expPos, 1000.0f);
			player1.GetComponent<FloatMoving>().collided = false;
			timerBool = true;
			player1.GetComponent<SimpleMover>().enabled = false;
			player2.GetComponent<SimpleMover>().enabled = false;
		}
		if(timerBool == true)
		{
			timer -= Time.deltaTime;
			if(timer <= 0)
			{
				player1.GetComponent<SimpleMover>().enabled = true;
				player2.GetComponent<SimpleMover>().enabled = true;
				timer = 0.3f;
				timerBool = false;
				Destroy(collisionParticle.gameObject, 1.0f);
			}
		}
	}
}
