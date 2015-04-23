using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FluffConverter : MonoBehaviour {


	public GameObject fluffPrefab;
	public float maximumForce = 10.0f;
	public float angleShot;

    public float minAngle = 0f;
    public float maxAngle = 0f;

    public GameObject fluffShooter;
	private float velocity;
	private Vector3 targetAngle;

    private GameObject orbToShrink;

    public float pullDuration = 1f;
    public float convertDuration = 2f;
    private float t = 0f;
    private Vector3 orbStartingSize;
    private Vector3 orbStartingPosition;

    private bool pulling = false;

    //Editor Testing.
    public bool fire = false;
    private bool lastFire = false;

	// Use this for initialization
	void Start () {
	}
	
    

    void Update()
    {
        if(fire != lastFire)
        {
            ConvertToFluffs();
        }

        lastFire = fire;

        if (orbToShrink != null)
            PullInOrb();
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
            fluff.transform.position = fluffShooter.transform.position;

            Fluff fluffF = fluff.GetComponent<Fluff>();
            velocity = Random.Range(500F, 1000F);
            angleShot = Random.Range(minAngle, maxAngle);            
            targetAngle = Quaternion.Euler(0, 0, angleShot) * new Vector3(velocity, velocity, 0);

            targetAngle = -fluffShooter.transform.TransformDirection(targetAngle);
            fluffF.Pass(targetAngle);
        }
	}

    public void PullInOrb()
    {
        if(pulling)
        {
            t = Mathf.Clamp(t + (Time.deltaTime / pullDuration), 0.0f, 1.0f);
            orbToShrink.transform.position = Vector3.Lerp(orbStartingPosition, transform.position, t);

            if (t == 1.0f)
            {                
                t = 0.0f;
                pulling = false;
            }
        }
        else
        {
            ShrinkOrb();
        }
    }

    public void ShrinkOrb()
    {

        t = Mathf.Clamp(t + (Time.deltaTime / convertDuration), 0.0f, 1.0f);

        orbToShrink.transform.localScale = Vector3.Lerp(orbStartingSize, Vector3.zero, t);

        orbToShrink.transform.position = Vector3.Lerp(transform.position, fluffShooter.transform.position, t);

        if (t == 1.0f)
        {
            GameObject.Destroy(orbToShrink);
            orbToShrink = null;
            orbStartingSize = Vector3.zero;
            t = 0.0f;

            ConvertToFluffs();
        }

    }

    void OnTriggerEnter(Collider collide)
    {
        if (collide.name == "Blossom" || collide.name == "Blossom(Clone)")
        {
            if (orbToShrink == null)
            {
                orbToShrink = collide.gameObject;
                orbStartingSize = collide.transform.localScale;
                orbStartingPosition = collide.transform.position;
                pulling = true;
                //orbToShrink.transform.position = transform.position;
            }
        }
    }

    void OnTriggerStay(Collider collide)
    {
        if (collide.name == "Blossom" || collide.name == "Blossom(Clone)")
        {
            if (orbToShrink == null)
            {
                orbToShrink = collide.gameObject;
                orbStartingSize = collide.transform.localScale;
                orbStartingPosition = collide.transform.position;
                pulling = true;
            }
        }
    }

}
