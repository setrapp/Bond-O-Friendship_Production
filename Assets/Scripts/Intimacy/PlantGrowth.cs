using UnityEngine;
using System.Collections;

public class PlantGrowth : MonoBehaviour {

	public float setMaxWiltTime;
	public float wiltTimer;
	public int fluffsRequiredPerBud;
	public bool collided;
	public Material plantColor;
	public DepthMaskHandler depthMask;

	public int fluffCount;
	private bool wilting;
	private Bud[] buds;
	private GameObject[] blossoms;
	private bool fullyBloomed;
	private float colorValue;
	private Material plantColorCopy;


	// Use this for initialization
	void Start () {
		buds = GetComponentsInChildren<Bud>();
		blossoms = new GameObject[buds.Length];
		plantColorCopy = new Material(plantColor);
		MeshRenderer[] childRenders = GetComponentsInChildren<MeshRenderer>();
		for(int i = 0; i < childRenders.Length; i++)
		{
			childRenders[i].material = plantColorCopy;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (depthMask != null && depthMask.depthMask != null)
		{
			float maskRange = Mathf.Min(fluffCount, buds.Length * fluffsRequiredPerBud);
			depthMask.depthMask.transform.localScale = new Vector3(maskRange, maskRange, depthMask.depthMask.transform.localScale.z);
		}

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
					blossoms[i].GetComponent<Collider>().enabled = false;
					blossoms[i].GetComponent<Renderer>().material.color = new Color(Random.Range(0, 1.0f), Random.Range(0, 1.0f), Random.Range(0, 1.0f), 0.2f);
					blossoms[i].GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
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
							
							blossoms[j].GetComponent<Collider>().enabled = true;
							blossomRigid.drag = 1;
							blossomRigid.useGravity = false;
							blossomRigid.constraints = RigidbodyConstraints.FreezePositionZ;
							blossomRigid.AddForce(buds[j].transform.up * 10, ForceMode.Impulse);
							/*Light blossomLight = */blossoms[j].AddComponent<Light>();
							//blossomLight.range = 30;
							/*DepthMaskHandler depth = */blossoms[j].AddComponent<DepthMaskHandler>();
							//blossoms[j].GetComponent<DepthMaskHandler>().depthMask.transform.localScale = new Vector3(2,2,2);
							blossoms[j].AddComponent<FluffPopper>();
							FluffStick fluffStick = blossoms[j].AddComponent<FluffStick>();
							fluffStick.maxPullForce = 0.01f;
						}
					}
				}
				if(fluffCount/fluffsRequiredPerBud < i + 1 && blossoms[i] != null)
				{
					Destroy(blossoms[i]);
					blossoms[i] = null;
				}
			}
			if(fullyBloomed == true)
			{
				BudCollision[] allChildren = GetComponentsInChildren<BudCollision>();
				for(int i = 0; i < allChildren.Length; i++)
					allChildren[i].gameObject.GetComponent<Collider>().enabled = false;
			}
			colorValue = (float)fluffCount/(fluffsRequiredPerBud*buds.Length);
			plantColorCopy.color = new Color(colorValue, colorValue, colorValue);
		}
	}
}
