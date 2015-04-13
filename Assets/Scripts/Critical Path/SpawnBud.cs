using UnityEngine;
using System.Collections;

public class SpawnBud : MonoBehaviour {

	public ParticleSystem part;
	public GameObject bud;

	private float fadeTimer = 1.0f;
	private bool fading;
	private bool spawned;
	private Color playerColor;
	private GameObject newBud;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(fading == true)
		{
			fadeTimer -= Time.deltaTime;
			newBud.transform.localScale = new Vector3(2.0f - fadeTimer*2, 2.0f - fadeTimer*2, 2.0f - fadeTimer*2);
			GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.6f*fadeTimer);

			if(fadeTimer <= 0)
				Destroy(gameObject);
		}
	}

	void OnTriggerEnter (Collider col) {
		if(spawned == false && col.name != "Fluff(Clone)")
		{

			if (col == Globals.Instance.player1.character.bodyCollider)
			{
				playerColor = Globals.Instance.player1.character.colors.attachmentColor;
			}
			else if (col == Globals.Instance.player2.character.bodyCollider)
			{
				playerColor = Globals.Instance.player2.character.colors.attachmentColor;
			}
			ParticleSystem nodePart = (ParticleSystem)Instantiate(part);
			nodePart.startColor = playerColor;
			nodePart.transform.position = transform.position;
			nodePart.transform.localScale = new Vector3(1.9f, 1.9f, 1.9f);
			//part.transform.parent = transform;
			//part.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
			Destroy(nodePart.gameObject, 2.0f);
			newBud = (GameObject)Instantiate(bud);
			newBud.transform.position = transform.position;
			newBud.transform.localScale = new Vector3(0, 0, 0);
			newBud.GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 0.6f);
			fading = true;
			spawned = true;
		}
	}
}
