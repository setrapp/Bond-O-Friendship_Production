using UnityEngine;
using System.Collections;

public class PlantGrowth : MonoBehaviour {

	public float setMaxWiltTime;
	public float wiltTimer;
	public int fluffsRequiredPerBud;
	public bool collided;
	public GameObject collidedBud;

	private int fluffCount;
	private Color startColor;
	private Color finalColor;
	private bool wilting;
	private Bud[] buds;
	private GameObject[] blossoms;
	private bool fullyBloomed;

	// Use this for initialization
	void Start () {
		buds = GetComponentsInChildren<Bud>();
		blossoms = new GameObject[buds.Length];
		finalColor = buds[0].GetComponent<Renderer>().material.color;
		startColor = new Color(1, 1, 1, 1);
		for(int i = 0; i < buds.Length; i++)
			buds[i].GetComponent<Renderer>().material.color = startColor;
	}
	
	// Update is called once per frame
	void Update () {
		if(collided == true)
		{
			fluffCount++;
			collided = false;
			if(fullyBloomed == false)
				wilting = true;
		}
		if(wilting == true)
		{
			wiltTimer -= Time.deltaTime;
			if(wiltTimer <= 0)
			{
				wiltTimer = setMaxWiltTime;
				fluffCount--;
				if(fluffCount == 0)
					wilting = false;
			}
			for(int i = 0; i < buds.Length; i++)
			{
				if(fluffCount/fluffsRequiredPerBud > i + 1 && blossoms[i] == null)
				{
					blossoms[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
					blossoms[i].GetComponent<Renderer>().material.color = new Color(Random.Range(0, 1.0f), Random.Range(0, 1.0f), Random.Range(0, 1.0f), 0.2f);

					blossoms[i].transform.localScale = new Vector3(2, 2, 2);
					blossoms[i].transform.parent = transform;
					blossoms[i].transform.position = buds[i].transform.position + buds[i].transform.up*(buds[i].transform.localScale.y/2) - new Vector3(0, 0, 1);
					blossoms[i].name = "Blossom";
					blossoms[i].layer = 16;
					if(i == buds.Length - 1 && blossoms[i] != null)
					{
						fullyBloomed = true;
						wilting = false;
						for(int j = 0; j < blossoms.Length; j++)
						{
							Rigidbody blossomRigid = blossoms[j].AddComponent<Rigidbody>();
							blossoms[j].GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
							blossomRigid.drag = 1;
							blossomRigid.useGravity = false;
							blossomRigid.constraints = RigidbodyConstraints.FreezePositionZ;
							Light blossomLight = blossoms[j].AddComponent<Light>();
							blossomLight.range = 30;
						}
					}
				}
				if(fluffCount/fluffsRequiredPerBud < i + 1 && blossoms[i] != null)
				{
					Destroy(blossoms[i]);
					blossoms[i] = null;
				}
			}
		}
	}
}
