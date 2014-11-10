using UnityEngine;
using System.Collections;

public class WallFade : MonoBehaviour {

	public GameObject field;
	private Color myColor;
	private float alpha;
	private float timer;


	// Use this for initialization
	void Start () {
		timer = 1.0f;
		alpha = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
		//myColor = new Color(renderer.material.color.r,renderer.material.color.g,renderer.material.color.b,alpha);
		//renderer.material.color = myColor;

		if(field.GetComponent<WallFadeField>().fadenow == true)
		{
			timer -= Time.deltaTime*10.0f;
			//alpha = timer;
		}
		if(timer <= 0)
		{
			transform.GetComponent<Collider>().enabled = false;
			GetComponent<Renderer>().enabled = false;

		}
	
	}

}
