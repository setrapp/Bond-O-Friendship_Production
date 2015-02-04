using UnityEngine;
using System.Collections;

public class PaintCircle : MonoBehaviour {

	public float myLife;
	public Vector3 mySize;
	public float sizeRand;
	// Use this for initialization
	void Start () {
		myLife = Random.Range(20.0f,25.0f);
		sizeRand = Random.Range(1.0f,3.0f);
		mySize = new Vector3(sizeRand,sizeRand,0.001f);
		transform.localScale = mySize;
	
	}
	
	// Update is called once per frame
	void Update () {
		myLife -= Time.deltaTime;
		if(myLife <= 0)
		{
			Destroy(gameObject);
		}
	}
}
