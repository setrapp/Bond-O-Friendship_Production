using UnityEngine;
using System.Collections;

public class PushPlayers : MonoBehaviour {

	private GameObject player1;
	private GameObject player2;
	private Vector3 expPos;
	private bool collided;
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
			GameObject s =  GameObject.CreatePrimitive(PrimitiveType.Sphere);
			s.transform.position = new Vector3(expPos.x, expPos.y, expPos.z + Random.Range(2.0f, 4.0f));
			s.GetComponent<Renderer>().material.color = new Color(Random.Range(0, 1.0f), Random.Range(0, 1.0f), Random.Range(0, 1.0f), 0.6f);
			s.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
			s.AddComponent<Light>();
			s.transform.parent = transform;
			s.GetComponent<Collider>().enabled = false;
			s.transform.localScale = new Vector3((p1Vel + p2Vel)/80, (p1Vel + p2Vel)/80, (p1Vel + p2Vel)/80);
			player1.GetComponent<FloatMoving>().collided = false;
			Destroy(s, 15.0f);
		}
		if(collisionParticle != null)
				Destroy(collisionParticle.gameObject, 1.0f);
	}
}
