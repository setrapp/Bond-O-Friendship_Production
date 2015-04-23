using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FluffConverter : MonoBehaviour {


	public GameObject fluffPrefab;
	//public Material greenFluffMaterial;
	//public float fluffSound;
	//public float spawnRate = 3.0f;
	//public float minimumForce = 3.0f;
	public float maximumForce = 10.0f;
	public float angleShot;

    public float minAngle = 0f;
    public float maxAngle = 0f;

    public GameObject fluffShooter;
	//public Color myColor;
	//public float alpha;
	//public GameObject Point;

	//public GameObject generatorTop;
	

	private int colorPicker;
	private float velocity;
	private Vector3 targetAngle;
	private Vector3 baseTarget;

    public bool fire = false;

    private bool lastFire = false;
	// Use this for initialization
	void Start () {
		//alpha = 1.0f;
		//baseTarget = generatorTop.transform.position;
	}
	
    

    void Update()
    {
        if(fire != lastFire)
        {
            ConvertToFluffs();
        }

        lastFire = fire;
    }
	
	public void ConvertToFluffs () 
    {
        List<GameObject> fluffList = new List<GameObject>();

        for (int i = 0; i < 10; i++)
        {
            GameObject fluffObject = (GameObject)Instantiate(fluffPrefab);
            fluffList.Add(fluffObject);
        }


        foreach (GameObject fluff in fluffList)
        {
            fluff.transform.parent = fluffShooter.transform;
            fluff.transform.localPosition = Vector3.zero;

            Fluff fluffF = fluff.GetComponent<Fluff>();
            float velocity = Random.Range(500F, 1000F);
            angleShot = Random.Range(minAngle, maxAngle);
            
            targetAngle = Quaternion.Euler(0, 0, angleShot) * new Vector3(velocity, velocity, 0);

            targetAngle = -fluffShooter.transform.TransformDirection(targetAngle);
            

            fluffF.Pass(targetAngle);
        }
	}

    void OnTriggerEnter(Collider collide)
    {
        if (collide.name == "Blossom" || collide.name == "Blossom(Clone)")
        {
            fire = !fire;
        }
    }

}
