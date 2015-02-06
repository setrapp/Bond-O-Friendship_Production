using UnityEngine;
using System.Collections;

public class PaintCircle : MonoBehaviour {

	public float myLife;
	public Vector3 mySize;
	public float sizeRand;
	public Color paintCircColor;
	// Use this for initialization
	void Start () {
		myLife = Random.Range(6.0f,7.0f);
		sizeRand = Random.Range(0.5f,3.0f);
		//paintCircColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
	
	}
	
	// Update is called once per frame
	void Update () {
		mySize = new Vector3(sizeRand,sizeRand,0.001f);
		transform.localScale = mySize;
		gameObject.GetComponent<Renderer>().material.color = paintCircColor;
		myLife -= Time.deltaTime;
		if(myLife <= 0)
		{
			sizeRand -= Time.deltaTime*2.0f;
			//paintCircColor.a -= Time.deltaTime;
		}

		if(sizeRand <= 0)
		{
			Destroy(gameObject);
		}
		/*
		if(paintCircColor.a <= 0)
		{
			Destroy(gameObject);
		}*/

	}
}
