using UnityEngine;
using System.Collections;

public class SpawnLand : MonoBehaviour {

	public GameObject landPrefab;
	public float maxLandSize;

	private GameObject land;
	private Threader threader;
	private bool spawned;

	// Use this for initialization
	void Start () {
		threader = GetComponent<Threader>();
	}
	
	// Update is called once per frame
	void Update () {
		if(threader.activated == true || transform.GetComponentInParent<ThreadParent>().solved == true)
		{
			if(land == null)
			{
				land = (GameObject)Instantiate(landPrefab);
				land.transform.position = transform.position + new Vector3(0, 0, 1.0f);
			}
			else if(land.transform.localScale.x <= maxLandSize)
			{
				land.transform.localScale += new Vector3(Time.deltaTime*2, Time.deltaTime*2, 0);
			}
		}
		else if(land != null && land.transform.localScale.x > 1.0f && transform.GetComponentInParent<ThreadParent>().solved == false)
		{
			land.transform.localScale -= new Vector3(Time.deltaTime*2, Time.deltaTime*2, 0);
		}

	}
}
