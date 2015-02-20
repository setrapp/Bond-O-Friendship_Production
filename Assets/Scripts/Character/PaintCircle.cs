using UnityEngine;
using System.Collections;

public class PaintCircle : MonoBehaviour {


    public bool erased;
	public float myLife;
	public Vector3 mySize;
	public float sizeRand;
	public Color paintCircColor;
	public float rSizemin;
	public float rSizemax;
	public float rLifemin;
	public float rLifemax;

	// Use this for initialization
	void Start () {
		sizeRand = Random.Range(rSizemin,rSizemax);
		myLife = Random.Range(rLifemin,rLifemax);
        erased = false;
		//paintCircColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () {
		//sizeRand = Random.Range(0.5f,3.0f);
		//myLife = Random.Range(6.0f,7.0f)
		mySize = new Vector3(sizeRand,sizeRand,0.001f);
		transform.localScale = mySize;
		gameObject.GetComponent<Renderer>().material.color = paintCircColor;
		myLife -= Time.deltaTime;
		if(myLife <= 0)
		{
            if (!erased)
                sizeRand -= Time.deltaTime * 2.0f;
            else
                sizeRand -= Time.deltaTime * 10.0f;
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
