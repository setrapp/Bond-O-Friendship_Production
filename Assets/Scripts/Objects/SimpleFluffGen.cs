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
	
	//public GameObject generatorTop;
	
	private float spawnTimer;
	private int colorPicker;
	private GameObject fluff;
	private float velocity;
	private Vector3 targetAngle;
	private Vector3 baseTarget;
	//public PulseShot pulseShot;
	//public ConnectionAttachable connectionAttachable;
	
	// Use this for initialization
	void Start () {
		spawnTimer = spawnRate;
		//baseTarget = generatorTop.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
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
		velocity = maximumForce;
		//if(Random.Range(0,2) == 1)
			//targetAngle = Quaternion.Euler(0, 0, Random.Range(60, 211)) * new Vector3(velocity,velocity,0);
		//else
		targetAngle = Quaternion.Euler(0, 0, angleShot) * new Vector3(velocity,velocity,0);
		//movePulse.target = transform.position + targetAngle;
		//movePulse.creator = conn;
		movePulse.Pass(targetAngle);//, this.gameObject);
	}
}