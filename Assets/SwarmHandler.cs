using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwarmHandler : MonoBehaviour {

    public int fireFlyCount = 12;
    public GameObject fireFlyPrefab;
    public List<GameObject> fireFlyList;
    public GameObject fireFly;

	// Use this for initialization
	void Start () {

	    for(int i=0; i<fireFlyCount;i++)
        {
           fireFly = (GameObject)Instantiate(fireFlyPrefab, transform.position, Random.rotation);
           fireFlyList.Add(fireFly);
           fireFly.transform.parent = transform;
        }

	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < fireFlyCount; i++)
        {
           
        }
	}
}
