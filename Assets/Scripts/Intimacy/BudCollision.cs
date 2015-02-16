using UnityEngine;
using System.Collections;

public class BudCollision : MonoBehaviour {

	private PlantGrowth plant;

	// Use this for initialization
	void Start () {
		plant = transform.parent.GetComponent<PlantGrowth>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void AttachFluff (Fluff fluff)
	{
		if(fluff != null && fluff.creator != null)
			plant.collided = true;
	}
}
