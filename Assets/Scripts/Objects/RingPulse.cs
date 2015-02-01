using UnityEngine;
using System.Collections;

public class RingPulse : MonoBehaviour {

	public float lifeTime;
	public float alpha;
	public Color mycolor;
	public float scaleRate;
	public float alphaFade;


	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {
		mycolor = new Color(1.0f,1.0f,1.0f,alpha);
		transform.localScale += new Vector3(scaleRate*Time.deltaTime,0,scaleRate*Time.deltaTime);
		GetComponent<Renderer>().material.color = mycolor;
		lifeTime -= Time.deltaTime;
		alpha -= Time.deltaTime*alphaFade;
		if(lifeTime <= 0)
		{
			Destroy(gameObject);
		}

	}

	void FixedUpdate ()
	{


	}


	void OnTriggerEnter(Collider collide)
	{
		//if(collide.gameObject.name == "Shield")
		//{
		//	gameObject.GetComponent<Collider>().enabled = false;
		//}
		//if(collide.gameObject.tag == "Character")
		//{
		//	gameObject.GetComponent<Collider>().enabled = false;
			//print("Collide");
			//collide.gameObject.GetComponent<CharacterComponents>().BreakAllBonds();
		//}
	}
}
