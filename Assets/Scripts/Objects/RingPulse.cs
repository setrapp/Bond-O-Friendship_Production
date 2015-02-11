using UnityEngine;
using System.Collections;

public class RingPulse : MonoBehaviour {

	public float lifeTime;
	public float alpha;
	public Color mycolor;
	public float scaleRate;
	public float alphaFade;
	public bool smallRing;

	public Color pColor;
	public Color gColor;
	public Color bColor;
	public Color yColor;
	public Color white;

	private int dice;
	// Use this for initialization
	void Start () {
		dice = Random.Range(1,4);
	}
	
	// Update is called once per frame
	void Update () {
		pColor = new Color(0.25f,0.1f,0.25f,alpha);
		gColor = new Color(0.06f,0.25f,0.1f,alpha);
		bColor = new Color(0.1f,0.23f,0.25f,alpha);
		yColor = new Color(0.23f,0.25f,0.1f,alpha);
		white = new Color(1.0f,1.0f,1.0f,alpha);

		if(smallRing == true)
		{
			if(dice == 1)
			{
				mycolor = pColor;
			}
			else if(dice == 2)
			{
				mycolor = gColor;
			}
			else if(dice == 3)
			{
				mycolor = bColor;
			}
			else if(dice == 4)
			{
				mycolor = yColor;
			}
		}
		else
		{
			mycolor = white;
		}

		transform.localScale += new Vector3(scaleRate*Time.deltaTime,scaleRate*Time.deltaTime,0);
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
