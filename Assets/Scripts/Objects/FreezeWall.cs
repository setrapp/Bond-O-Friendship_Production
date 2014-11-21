using UnityEngine;
using System.Collections;

public class FreezeWall : MonoBehaviour {

	public GameObject icePrefab;
	private GameObject ice;
	private bool freezing;
	private float extents;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(freezing == true && ice == null)
		{
			ice = Instantiate<GameObject>(icePrefab);
			ice.transform.position = new Vector3(transform.position.x + GetComponent<Collider>().bounds.extents.x, transform.position.y, transform.position.z - 3.5f);
		}

		if(freezing == true && ice != null && ice.transform.position.z - transform.position.z >= 0)
		{
			ice.transform.Translate(0, 0, -0.01f);
		}

		if(freezing == false && ice != null && ice.transform.position.z - transform.position.z <= 3.5f)
		{
			ice.transform.Translate(0, 0, 0.01f);
		}

		if(freezing == false && ice != null)
			Destroy(ice);
	}

	void OnTriggerEnter(Collider col){
		if(col.transform.tag == "Converser" || col.transform.tag == "Pulse")
			freezing = true;
	}
	
	void OnTriggerExit(Collider col){
		if(col.transform.tag == "Converser" || col.transform.tag == "Pulse")
		{
			freezing = false;
		}
	}
}
