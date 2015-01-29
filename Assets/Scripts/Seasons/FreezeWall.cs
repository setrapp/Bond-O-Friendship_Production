using UnityEngine;
using System.Collections;

public class FreezeWall : MonoBehaviour {
	
	private GameObject ice;
	private GameObject leaves;
	private bool freezing;
	private bool dropLeaves;
	private float extents;
	private Vector3 collisionPoint;
	private Vector3 collisionNormal;
	private Color iceColor;
	private Color startColor;
	private GameObject objects;
	private ManageSeasons manager;

	// Use this for initialization
	void Start () {
		manager = GameObject.Find ("Seasons Manager").GetComponent<ManageSeasons> ();

		iceColor = new Color (0.2f, 1.0f, 1.0f);
		startColor = GetComponent<Renderer>().material.color;
		objects = GameObject.Find ("Objects");
	}
	
	// Update is called once per frame
	void Update () {
		if(freezing == true && ice == null)
		{
			if(manager.season == 1)
			{
				ice = (GameObject)Instantiate(manager.icePrefab);
				ice.transform.position = new Vector3(collisionPoint.x, collisionPoint.y, 0);
				ice.transform.right = -collisionNormal;
				ice.transform.position = new Vector3(ice.transform.position.x, ice.transform.position.y, -0.9f);
				ice.transform.parent = objects.transform;
			}
		}

		if(dropLeaves == true && leaves == null)
		{
			if(manager.season == 2)
			{
				leaves = (GameObject)Instantiate(manager.petalsPrefab);
				leaves.transform.position = new Vector3(collisionPoint.x, collisionPoint.y, 0);
				leaves.transform.position = new Vector3(leaves.transform.position.x, leaves.transform.position.y, -10.0f);
				leaves.transform.parent = objects.transform;
				Destroy(leaves, leaves.GetComponent<ParticleSystem>().startLifetime);
			}
		}

		if(freezing == true && ice != null && ice.transform.position.z - transform.position.z >= -5.0f)
		{
			ice.transform.position += new Vector3(0, 0, -0.01f);
			GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, iceColor, Time.deltaTime*0.4f);
		}

		if((freezing == false || manager.season != 1)  && ice != null && ice.transform.position.z - transform.position.z <= 0)
		{
			ice.transform.position += new Vector3(0, 0, 0.01f);
			GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, startColor, Time.deltaTime*0.5f);
		}

		if((freezing == false || manager.season != 1)  && ice != null && ice.transform.position.z - transform.position.z >= -1.0f)
			Destroy(ice);
	}

	void OnCollisionEnter(Collision col){
		if(col.transform.tag == "Character" || col.transform.tag == "Fluff")
		{
			freezing = true;
			dropLeaves = true;
			collisionPoint = col.contacts[0].point;
			collisionNormal = col.contacts[0].normal;
		}
	}

	void OnCollisionExit(Collision col){
		if(col.transform.tag == "Character" || col.transform.tag == "Fluff")
		{
			freezing = false;
			dropLeaves = false;
		}
	}
}
