using UnityEngine;
using System.Collections;

public class FluffGenerator : MonoBehaviour {

	public GameObject fluffPrefab;
	public Material greenFluffMaterial;
	public float spawnRate = 3.0f;
	public float velocityRange = 5.0f;
	public float minimumVelocity = 5.0f;

	private float spawnTimer;
	private int colorPicker;
	private GameObject fluff;
	private float xVelocity;
	private float yVelocity;

	// Use this for initialization
	void Start () {
		spawnTimer = spawnRate;
	}
	
	// Update is called once per frame
	void Update () {
		spawnTimer -= Time.deltaTime;

		if(spawnTimer <= 0)
		{
			GenerateFluff();
			spawnTimer = spawnRate;
		}
	}

	void GenerateFluff () {
		fluff = (GameObject)Instantiate(fluffPrefab);
		colorPicker = Random.Range(0, 2);
		if(colorPicker == 1)
		{
			MeshRenderer[] meshRenderers = fluff.GetComponentsInChildren<MeshRenderer>();
			for (int i = 0; i < meshRenderers.Length; i++)
			{
				meshRenderers[i].material = greenFluffMaterial;
			}
		}
		fluff.transform.position = transform.position;
		fluff.transform.parent = gameObject.transform;
		MovePulse movePulse = fluff.GetComponent<MovePulse>();
		xVelocity = Random.Range(-velocityRange, velocityRange);
		if(xVelocity < 0)
			xVelocity -= minimumVelocity;
		else
			xVelocity += minimumVelocity;
		yVelocity = Random.Range(-velocityRange, velocityRange);
		if(xVelocity < 0)
			yVelocity -= minimumVelocity;
		else
			yVelocity += minimumVelocity;
		movePulse.ReadyForPass();
//		movePulse.target = transform.position + new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), 0.0f);
		movePulse.target = transform.position + new Vector3(xVelocity, yVelocity, 0.0f);
	}
}
