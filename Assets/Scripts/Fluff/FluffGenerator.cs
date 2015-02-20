using UnityEngine;
using System.Collections;

public class FluffGenerator : MonoBehaviour {

	public GameObject fluffPrefab;
	public Material greenFluffMaterial;
	//public float fluffSound;
	public float spawnRate = 3.0f;
	public float minimumForce = 3.0f;
	public float maximumForce = 10.0f;

	public GameObject generatorTop;

	private float spawnTimer;
	private int colorPicker;
	private float velocity;
	private Vector3 targetAngle;
	private Vector3 baseTarget;
	public BondAttachable bondAttachable;

	// Use this for initialization
	void Start () {
		spawnTimer = spawnRate;
		baseTarget = generatorTop.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		spawnTimer -= Time.deltaTime;
		//Debug.Log(targetAngle);
		if(targetAngle != Vector3.zero)
		{
			//Debug.Log(spawnTimer);
			if(spawnTimer >= spawnRate/2)
				generatorTop.transform.position = Vector3.Lerp(generatorTop.transform.position, targetAngle.normalized, 2 * Time.deltaTime);
			else
				generatorTop.transform.position = Vector3.Lerp(generatorTop.transform.position, baseTarget, 1 * Time.deltaTime);
		}

		if(spawnTimer <= 0)
		{
			GenerateFluff();
			spawnTimer = spawnRate;
		}
	}

	void GenerateFluff () {
		GameObject fluffObject = (GameObject)Instantiate(fluffPrefab);
		colorPicker = Random.Range(0, 2);
		if(colorPicker == 1)
		{
			MeshRenderer[] meshRenderers = fluffObject.GetComponentsInChildren<MeshRenderer>();
			for (int i = 0; i < meshRenderers.Length; i++)
			{
				meshRenderers[i].material = greenFluffMaterial;
			}
		}
		fluffObject.transform.position = transform.position;
		fluffObject.transform.parent = gameObject.transform;
		Fluff fluff = fluffObject.GetComponent<Fluff>();
		velocity = Random.Range(minimumForce, maximumForce);
		if(Random.Range(0,2) == 1)
			targetAngle = Quaternion.Euler(0, 0, Random.Range(60, 211)) * new Vector3(velocity,velocity,0);
		else
			targetAngle = Quaternion.Euler(0, 0, Random.Range(-120, 31)) * new Vector3(velocity,velocity,0);
		//fluff.target = transform.position + targetAngle;
		fluff.creator = bondAttachable;
		fluff.Pass(targetAngle, this.gameObject);
	}
}
