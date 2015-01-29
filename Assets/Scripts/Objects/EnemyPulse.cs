using UnityEngine;
using System.Collections;

public class EnemyPulse : MonoBehaviour {

	public GameObject creator;
	public Vector3 target;
	private float speed = 1.0f;
	private float lifeTime;

	// Use this for initialization
	void Start () {
		lifeTime = 5.0f;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 direction = target;
		direction.z = 0;
		Vector3 moveVector = direction.normalized * Time.deltaTime * speed;
		transform.position += moveVector*5.0f;
		lifeTime -= Time.deltaTime;
		if(lifeTime <= 0)
		{
			Destroy(gameObject);
		}
	}
	void OnTriggerEnter(Collider collide)
	{
		//if(collide.gameObject.name == "Shield")
		//{
		//	Destroy(gameObject);
		//}
		//if(collide.gameObject.tag == "Character")
		//{
		//	Destroy(gameObject);
		//}
		if(collide.gameObject.tag == "Object")
		{
			Destroy(gameObject);
		}
	}
}
