using UnityEngine;
using System.Collections;

public class PullApartWall : MonoBehaviour {

	public GameObject PullPuzz;

	public Color myColor;
	public float alpha;


	// Use this for initialization
	void Start () {
		alpha = 0.7f;
	
	}
	
	// Update is called once per frame
	void Update () {
		myColor = new Color(0.2f,0.0f,0.7f,alpha);
		GetComponent<Renderer>().material.color = myColor;
		if(PullPuzz == null)
		{
			alpha -= Time.deltaTime;
		}
		if(alpha <= 0)
		{
			Destroy(gameObject);
		}

	
	}
}
