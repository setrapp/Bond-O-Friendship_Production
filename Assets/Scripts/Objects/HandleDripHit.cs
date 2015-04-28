using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandleDripHit : MonoBehaviour 
{

    public GameObject landPrefab;

    public Vector3 maxLandSize;
    private Vector3 minLandSize;

    private GameObject land;

    private List<GameObject> drips = new List<GameObject>();

    private float t = 0.0f;
    private float duration = 5.0f;

	// Use this for initialization
	void Start () 
    {
        land = (GameObject)Instantiate(landPrefab);
        land.transform.position = transform.position + new Vector3(0, 0, 1.0f);
        minLandSize = new Vector3(0.0f, landPrefab.transform.localScale.y, 0.0f);
	}
	
	// Update is called once per frame
	void Update () 
    {
        drips.RemoveAll(drip => drip == null);
        t = drips.Count > 0 ? Mathf.Clamp(t + (Time.deltaTime / duration), 0.0f, 1.0f) : Mathf.Clamp(t - (Time.deltaTime / duration), 0.0f, 1.0f);

        land.transform.localScale = Vector3.Lerp(minLandSize, maxLandSize, t);

        Debug.Log(drips.Count);
        /*if (land.transform.localScale.x <= maxLandSize)
            {
                land.transform.localScale += new Vector3(Time.deltaTime * 2, 0, Time.deltaTime * 2);
            }
        
            if (land != null && land.transform.localScale.x > 1.0f)
            {
                land.transform.localScale -= new Vector3(Time.deltaTime * 2, 0, Time.deltaTime * 2);
            }*/


	}

    void OnTriggerEnter(Collider collide)
    {
        if (collide.name == "DripPaintCircle(Clone)")
        {
            drips.Add(collide.gameObject);
        }
    }

    void OnTriggerExit(Collider collide)
    {
        if (collide.name == "DripPaintCircle(Clone)")
        {
            drips.Remove(collide.gameObject);
        }
    }
}
