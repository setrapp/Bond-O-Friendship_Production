using UnityEngine;
using System.Collections;

public class SpawnLand : MonoBehaviour {

	public GameObject landPrefab;
	public float maxLandSize;

	private GameObject land;
	private Threader threader;
	private bool spawned;
	private Color threadColor;
	private Color childColor;
	private bool removed;
	public float colliderZDepth = 5;
	private Renderer[] renderers;
    public float spawnRate;

	// Use this for initialization
	void Start () {
		threader = GetComponent<Threader>();
		threadColor = GetComponent<Renderer>().material.color;
        spawnRate = 4.0f;
		renderers = GetComponentsInChildren<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if(threader.activated == true || transform.GetComponentInParent<ThreadParent>().solved == true)
		{
			if(land == null)
			{
				land = (GameObject)Instantiate(landPrefab);
				land.transform.position = transform.position + new Vector3(0, 0, 5.0f);
				land.transform.parent = transform.parent;
				Collider landCollider = land.GetComponentInChildren<Collider>();
				if (landCollider != null)
				{
					landCollider.transform.localPosition += new Vector3(0, colliderZDepth, 0);
				}
			}
			else if(land.transform.localScale.x <= maxLandSize)
			{
				land.transform.localScale += new Vector3(Time.deltaTime*spawnRate, 0, Time.deltaTime*spawnRate);
				if (land.transform.localScale.x >= maxLandSize)
				{
					SendMessage("LandFull", SendMessageOptions.DontRequireReceiver);
				}
			}
		}
		else if(land != null && land.transform.localScale.x > 1.0f && transform.GetComponentInParent<ThreadParent>().solved == false)
		{
			land.transform.localScale -= new Vector3(Time.deltaTime*spawnRate, 0, Time.deltaTime*spawnRate);
			if (land.transform.localScale.x < maxLandSize)
			{
				SendMessage("LandNotFull", SendMessageOptions.DontRequireReceiver);
			}
		}
		if(transform.GetComponentInParent<ThreadParent>().solved == true)
		{
			for (int i = 0; i < renderers.Length; i++)
			{
				Color color = renderers[i].material.color;
				color.a -= Time.deltaTime;
				renderers[i].material.color = color;
			}
			/*threadColor.a -= Time.deltaTime;
			childColor.a -= Time.deltaTime;
			GetComponent<Renderer>().material.color = threadColor;
			transform.GetChild(0).GetComponent<Renderer>().material.color = childColor;*/
			if(!removed)
			{
				for(int i = 0; i < GetComponents<Collider>().Length; i++)
					GetComponents<Collider>()[i].enabled = false;
				if(transform.GetChild(0).GetComponent<Collider>().enabled == true)
					transform.GetChild(0).GetComponent<Collider>().enabled = false;

				removed = true;
			}
		}

	}
}
