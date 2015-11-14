using UnityEngine;
using System.Collections;

public class SpawnBud : MonoBehaviour {

	public ParticleSystem part;
	public GameObject bud;
	public bool spawned;
	public Color parentColor;
    //public Color BudColor;
	public Transform spawnParent;
	private float fadeTimer = 1.0f;
	private bool fading;
	private Color playerColor;
	private GameObject newBud;
	private bool scaling;
	private bool fullScaled = false;
    public AudioSource spawnBud;

	// Use this for initialization
	void Start () {
		parentColor = transform.parent.GetComponent<Renderer>().material.color;
	}
	
	// Update is called once per frame
	void Update () {
		if(fading == true)
		{
			fadeTimer -= Time.deltaTime;
			if (!scaling)
			{
				newBud.transform.localScale = new Vector3(2.0f - fadeTimer * 2.0f, 2.0f - fadeTimer * 2.0f, 2.0f - fadeTimer * 2.0f);
			}
			transform.parent.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.6f*fadeTimer);

			if(fadeTimer <= 0)
			{
				fading = false;
				scaling = false;
				newBud.transform.parent = transform.parent;
				if (spawnParent != null)
				{
					newBud.transform.parent = spawnParent;
				}
				DestroyInSpace budDestroy = newBud.GetComponent<DestroyInSpace>();
				if (budDestroy)
				{
					budDestroy.spawner = this;
					budDestroy.spawnRenderer = transform.parent.GetComponent<Renderer>();
				}
				SeasonObjectReaction seasonReaction = newBud.GetComponent<SeasonObjectReaction>();
				if (seasonReaction != null)
				{
					seasonReaction.enabled = true;
				}

				fadeTimer = 1.0f;
			}
			//	Destroy(gameObject);

		}
		else if (!fullScaled && newBud != null)
		{
			// Allow fluffs to spawn on fully completed blossom to afford pulling (otherwise blossom can get stuck in corners).
			FluffPlaceholder[] fluffPlaceholders = newBud.GetComponentsInChildren<FluffPlaceholder>();
			for (int i = 0; i < fluffPlaceholders.Length; i++)
			{
				fluffPlaceholders[i].enabled = true;
			}
			fullScaled = true;
		}
	}

	void OnTriggerEnter (Collider col) {
		if(spawned == false && (col.gameObject.tag == "Character" || col.gameObject.layer == LayerMask.NameToLayer("Bond")))
		{

			if (col == Globals.Instance.Player1.character.bodyCollider)
			{
				playerColor = Globals.Instance.Player1.character.colors.baseColor;
			}
			else if (col == Globals.Instance.Player2.character.bodyCollider)
			{
				playerColor = Globals.Instance.Player2.character.colors.baseColor;
			}
			SpawnOrb();
		}
	}

	public void SpawnOrb()
	{
		spawnBud.Play();
		ParticleSystem nodePart = (ParticleSystem)Instantiate(part);
		nodePart.startColor = playerColor;
		nodePart.transform.position = transform.position;
		nodePart.transform.localScale = new Vector3(1.9f, 1.9f, 1.9f);
		//part.transform.parent = transform;
		//part.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
		Destroy(nodePart.gameObject, 2.0f);
		newBud = (GameObject)Instantiate(bud);
		newBud.transform.position = transform.position;
		newBud.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
		//newBud.GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 0.6f);
		//newBud.GetComponent<Renderer>().material.color = BudColor;
		fading = true;
		spawned = true;
		fullScaled = false;
	}
}
