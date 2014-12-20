using UnityEngine;
using System.Collections;

public class FreezeWall : MonoBehaviour {

	public GameObject icePrefab;
	private GameObject ice;
	public GameObject leavesPrefab;
	private GameObject leaves;
	public ParticleSystem rain;
	public ParticleSystem rain2;
	public ParticleSystem rain3;
	public Light rainLight;
	private bool freezing;
	private bool dropLeaves;
	private float extents;
	private Vector3 collisionPoint;
	private Vector3 collisionNormal;
	private Color iceColor;
	private Color startColor;
	private GameObject objects;

	// Use this for initialization
	void Start () {
		iceColor = new Color (0.2f, 1.0f, 1.0f);
		startColor = GetComponent<Renderer>().material.color;
		objects = GameObject.Find ("Objects");
	}
	
	// Update is called once per frame
	void Update () {
		if(freezing == true && ice == null)
		{
			if(GetComponent<TextureSeasons>().season == 1)
			{
				ice = (GameObject)Instantiate(icePrefab);
				ice.transform.position = new Vector3(collisionPoint.x, collisionPoint.y, 0);
				ice.transform.right = -collisionNormal;
				ice.transform.position = new Vector3(ice.transform.position.x, ice.transform.position.y, -0.9f);
				ice.transform.parent = objects.transform;
			}
		}

		if(dropLeaves == true && leaves == null)
		{
			if(GetComponent<TextureSeasons>().season == 2)
			{
				leaves = (GameObject)Instantiate(leavesPrefab);
				leaves.transform.position = new Vector3(collisionPoint.x, collisionPoint.y, 0);
				leaves.transform.position = new Vector3(leaves.transform.position.x, leaves.transform.position.y, -10.0f);
				leaves.transform.parent = objects.transform;
				Destroy(leaves, leaves.GetComponent<ParticleSystem>().startLifetime);
			}
		}

		if(GetComponent<TextureSeasons>().season == 3)
		{
			rain.enableEmission = true;
			rain2.enableEmission = true;
			rain3.enableEmission = true;
			if(rainLight.intensity > 0.36)
				rainLight.intensity -= Time.deltaTime * 0.001f;
		}
		else
		{
			rain.enableEmission = false;
			rain2.enableEmission = false;
			rain3.enableEmission = false;
			if(rainLight.intensity < 0.49)
				rainLight.intensity += Time.deltaTime * 0.001f;
		}

		if(freezing == true && ice != null && ice.transform.position.z - transform.position.z >= -5.0f)
		{
			ice.transform.position += new Vector3(0, 0, -0.01f);
			GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, iceColor, Time.deltaTime*0.4f);
		}

		if((freezing == false || GetComponent<TextureSeasons>().season != 1)  && ice != null && ice.transform.position.z - transform.position.z <= 0)
		{
			ice.transform.position += new Vector3(0, 0, 0.01f);
			GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, startColor, Time.deltaTime*0.5f);
		}

		if((freezing == false || GetComponent<TextureSeasons>().season != 1)  && ice != null && ice.transform.position.z - transform.position.z >= -1.0f)
			Destroy(ice);
	}

	void OnCollisionEnter(Collision col){
		if(col.transform.tag == "Converser" || col.transform.tag == "Fluff")
		{
			freezing = true;
			dropLeaves = true;
			collisionPoint = col.contacts[0].point;
			collisionNormal = col.contacts[0].normal;
		}
	}

	void OnCollisionExit(Collision col){
		if(col.transform.tag == "Converser" || col.transform.tag == "Fluff")
		{
			freezing = false;
			dropLeaves = false;
		}
	}
}
