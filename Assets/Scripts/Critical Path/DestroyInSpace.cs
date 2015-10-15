using UnityEngine;
using System.Collections;

public class DestroyInSpace : MonoBehaviour {

	public GameObject destroyee;
	public LayerMask ignoreLayers;
	private bool wasFloating = false;
	private bool falling;
	public SpawnBud spawner;
	public Renderer spawnRenderer;
	public Rigidbody body;
	public Collider blossomCollider;
	public bool disableAllCollidersOnFall = true;
	public float checkRadius = 1;

	// Use this for initialization
	void Start () {
		if (destroyee == null)
		{
			destroyee = gameObject;
		}
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		if (!Physics.Raycast (transform.position, Vector3.forward, out hit, Mathf.Infinity, ~ignoreLayers)) {
			if (!wasFloating && !Physics.CheckSphere (transform.position, checkRadius, ~ignoreLayers)) {
				wasFloating = true;
				if (spawner != null) {
					if (spawnRenderer != null) {
						spawnRenderer.material.color = spawner.parentColor;
					}
					spawner.spawned = false;
				}
				falling = true;
				if (body != null && !body.isKinematic) {
					body.velocity = Vector3.zero;
				}
				if (blossomCollider != null) {
					blossomCollider.enabled = false;
				}
				if (disableAllCollidersOnFall) {
					Collider[] colliders = GetComponentsInChildren<Collider> ();
					for (int i = 0; i < colliders.Length; i++) {
						colliders [i].enabled = false;
					}
				}
			}
		}
		if(falling)
		{
			transform.localScale -= new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime);
			if(transform.localScale.x <= 0)
				Destroy(destroyee);
		}
	}
}
