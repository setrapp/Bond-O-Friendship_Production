using UnityEngine;
using System.Collections;

public class EmitRing : MonoBehaviour {

	private GameObject ring;
	public GameObject ringPrefab;
	public float scaleRate = 0.2f;
	private float ringTimer;
	public float ringDuration = 3.0f;
	public Material ringMaterial;

	// Use this for initialization
	void Start () {
		ringTimer = ringDuration;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetKeyDown(KeyCode.E) && ring == null)
		{
			ring = (GameObject)Instantiate(ringPrefab);
			ring.collider.isTrigger = true;
			ring.transform.position = transform.position;
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
