using UnityEngine;
using System.Collections;

public class SimpleFluffGen : MonoBehaviour {

	public GameObject fluffPrefab;
	public Material greenFluffMaterial;
	//public float fluffSound;
	public float spawnRate = 3.0f;
	public float minimumForce = 3.0f;
	public float maximumForce = 10.0f;
	public int angleShot;
	public Color myColor;
	public float alpha;
	public GameObject Point;

	//public GameObject generatorTop;
	
	private float spawnTimer;
	private int colorPicker;
	private float velocity;
	private Vector3 targetAngle;
	private Vector3 baseTarget;
	
	// Use this for initialization
	void Start () {
		spawnTimer = spawnRate;
		alpha = 1.0f;
		//baseTarget = generatorTop.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		myColor = new Color(0.6f, 0.6f,1.0f,alpha);

		if(alpha > 0.2f)
			alpha -= Time.deltaTime * 2.0f;

		if(Point != null)
		{
			Point.GetComponent<Renderer>().material.color = myColor;
		}

		spawnTimer -= Time.deltaTime;
		//Debug.Log(targetAngle);
		//if(targetAngle != null && targetAngle != Vector3.zero)
		//{
			//Debug.Log(spawnTimer);
			//if(spawnTimer >= spawnRate/2)
				//generatorTop.transform.position = Vector3.Lerp(generatorTop.transform.position, targetAngle.normalized, 2 * Time.deltaTime);
			//else
				//generatorTop.transform.position = Vector3.Lerp(generatorTop.transform.position, baseTarget, 1 * Time.deltaTime);
		//}
		
		if(spawnTimer <= 0)
		{
			if(alpha < 1.0f)
				alpha = 1.0f;
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
		velocity = maximumForce;
		//if(Random.Range(0,2) == 1)
			//targetAngle = Quaternion.Euler(0, 0, Random.Range(60, 211)) * new Vector3(velocity,velocity,0);
		//else
		targetAngle = Quaternion.Euler(0, 0, angleShot) * new Vector3(velocity,velocity,0);
        fluff.Pass(targetAngle);
	}
}