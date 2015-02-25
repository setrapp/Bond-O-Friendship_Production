using UnityEngine;
using System.Collections;

public class SpawnLeaves : MonoBehaviour {

	private GameObject leaf;
	private GameObject[] leaves;
	private GameObject player1;
	private GameObject player2;
	
	private int season;
	private int leafSelector;
	private int leafCount;

	private float xPos;
	private float yPos;
	private float halfWidth;
	private float halfHeight;
	private float area;
	private float fadeSpeed;
	private Color startColor;
	private int leafArraySize;
	private ManageSeasons manager;
	private GameObject leaf1Prefab;
	private GameObject leaf2Prefab;
	private GameObject leaf3Prefab;
	private GameObject leaf4Prefab;
	private GameObject leaf5Prefab;
	private GameObject leaf6Prefab;
	private GameObject leaf7Prefab;
	private float spawnRange;

	public float nearPlayerDistance;

	private float randRot;

	// Use this for initialization
	void Start () {
		halfWidth = GetComponent<Collider>().bounds.extents.x;
		halfHeight = GetComponent<Collider>().bounds.extents.y;

		player1 = Globals.Instance.player1.gameObject;
		player2 = Globals.Instance.player2.gameObject;

		startColor = new Color(0, 0, 0, 0);

		manager = GameObject.Find ("Seasons Manager").GetComponent<ManageSeasons> ();
		area = (halfWidth * halfHeight) * manager.leafDensity;
		leafArraySize = Mathf.RoundToInt(area) + 1;
		leaves = new GameObject[leafArraySize];

		leaf1Prefab =manager.leaf1Prefab;
		leaf2Prefab =manager.leaf2Prefab;
		leaf3Prefab =manager.leaf3Prefab;
		leaf4Prefab =manager.leaf4Prefab;
		leaf5Prefab =manager.leaf5Prefab;
		leaf6Prefab =manager.leaf6Prefab;
		leaf7Prefab =manager.leaf7Prefab;

		spawnRange = manager.leafSpawnRange;
	}
	
	// Update is called once per frame
	void Update () {

		randRot = Random.Range(0.0f,360.0f);
		if (Globals.Instance == null)
		{
			return;
		}

		season = manager.season;
		player1 = Globals.Instance.player1.gameObject;
		player2 = Globals.Instance.player2.gameObject;
		nearPlayerDistance = Vector3.Distance(transform.position, player1.transform.position);
		if (Vector3.Distance(transform.position, player2.transform.position) < nearPlayerDistance)
		{
			nearPlayerDistance = Vector3.Distance(transform.position, player2.transform.position);
		}

		if(season == 0 && leafCount < area)
		{
			if(Vector3.Distance(transform.position, player1.transform.position) < spawnRange || Vector3.Distance(transform.position, player2.transform.position) < spawnRange)
			{
				xPos = Random.Range(transform.position.x - halfWidth, transform.position.x + halfWidth);
				yPos = Random.Range(transform.position.y - halfHeight, transform.position.y + halfHeight);

				if (Physics.Raycast(new Vector3(xPos, yPos, -1.5f), Vector3.forward, Mathf.Infinity))
				{
					leafSelector = Random.Range(1, 8);
					if (leafSelector == 1)
						leaf = (GameObject)Instantiate(leaf1Prefab, new Vector3(xPos, yPos, -1.5f), Quaternion.Euler(0, 0, randRot));
					if (leafSelector == 2)
						leaf = (GameObject)Instantiate(leaf2Prefab, new Vector3(xPos, yPos, -1.5f), Quaternion.Euler(0, 0, randRot));
					if (leafSelector == 3)
						leaf = (GameObject)Instantiate(leaf3Prefab, new Vector3(xPos, yPos, -1.5f), Quaternion.Euler(0, 0, randRot));
					if (leafSelector == 4)
						leaf = (GameObject)Instantiate(leaf4Prefab, new Vector3(xPos, yPos, -1.5f), Quaternion.Euler(0, 0, randRot));
					if (leafSelector == 5)
						leaf = (GameObject)Instantiate(leaf5Prefab, new Vector3(xPos, yPos, -1.5f), Quaternion.Euler(0, 0, randRot));
					if (leafSelector == 6)
						leaf = (GameObject)Instantiate(leaf6Prefab, new Vector3(xPos, yPos, -1.5f), Quaternion.Euler(0, 0, randRot));
					if (leafSelector == 7)
						leaf = (GameObject)Instantiate(leaf7Prefab, new Vector3(xPos, yPos, -1.5f), Quaternion.Euler(0, 0, randRot));

					//leaf.transform.position = new Vector3(xPos, yPos, -1.0f); 
					//leaf.transform.position = new Vector3(xPos, yPos, -2); 
					leaves[leafCount] = leaf;
					leafCount++;
					leaf.transform.parent = GameObject.Find("Objects").transform;
					leaf.GetComponent<Renderer>().material.color = startColor;
				}
			}

		}

		if (leafCount > 0 && season == 0)
		{
			if(Vector3.Distance(transform.position, player1.transform.position) > spawnRange * 1.5f && Vector3.Distance(transform.position, player2.transform.position) > spawnRange * 1.5f)
			{
				//Debug.Log(leafArraySize);
				for(int i = 0; i < leafArraySize; i++)
				{
					//					if(leaves[i] != null)
					//						leaves[i].GetComponent<Renderer>().material.color = new Color(0, 0, 0, 0);
					if(leaves[i] != null)
					{
						Destroy(leaves[i].gameObject);
						//Debug.Log (gameObject.name + " " + leaves.Length);
						leafCount--;
					}
				}	
			}

		}
		
		if(season == 0 && leafCount > 0)
		{

			for(int i = 0; i < leafArraySize; i++)
			{
				if(leaves[i] != null)
				{
					fadeSpeed = Random.Range(0.4f, 0.7f);
					Color alpha = leaves[i].GetComponent<Renderer>().material.color;
					if(alpha.a < 1.0f)
					{
						alpha.a += Time.deltaTime*fadeSpeed*0.5f;
						alpha.r += Time.deltaTime*fadeSpeed*0.5f;
						alpha.g += Time.deltaTime*fadeSpeed*0.5f;
						alpha.b += Time.deltaTime*fadeSpeed*0.5f;
					}
					leaves[i].GetComponent<Renderer>().material.color = alpha;
				}
			}
		}

		if(season != 0 && leafCount > 0)
		{
			for(int i = 0; i < leafArraySize; i++)
			{
				if(leaves[i] != null)
				{
					fadeSpeed = Random.Range(0.4f, 0.7f);
					Color alpha = leaves[i].GetComponent<Renderer>().material.color;
					alpha.a -= Time.deltaTime*fadeSpeed*0.5f;
					alpha.r -= Time.deltaTime*fadeSpeed*0.5f;
					alpha.g -= Time.deltaTime*fadeSpeed*0.5f;
					alpha.b -= Time.deltaTime*fadeSpeed*0.5f;
					leaves[i].GetComponent<Renderer>().material.color = alpha;
					if(alpha.a <= 0)
					{
						Destroy(leaves[i].gameObject);
						leafCount--;
					}
				}
			}
		}
	}
}
