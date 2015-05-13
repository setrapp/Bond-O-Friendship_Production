using UnityEngine;
using System.Collections;

public class RingPulse : MonoBehaviour {

	public Renderer renderer;
	public float lifeTime;
	private float fullLifeTime;
	public bool fadeAlpha = true;
	public float alpha;
	public Color mycolor;
	public float scaleRate;
	public bool smallRing;
	[Header("Optional Retraction")]
	public float pauseInterval = -1;
	public float retractInterval = -1;
	private bool paused = false;

	public Color pColor;
	public Color gColor;
	public Color bColor;
	public Color yColor;
	public Color white;

	private int dice;
	// Use this for initialization
	void Start () {
		dice = Random.Range(1,4);

		if (renderer == null)
		{
			renderer = GetComponent<Renderer>();
		}
		fullLifeTime = lifeTime;
	}
	
	// Update is called once per frame
	void Update () {
		pColor = new Color(0.25f,0.1f,0.25f,alpha);
		gColor = new Color(0.06f,0.25f,0.1f,alpha);
		bColor = new Color(0.1f,0.23f,0.25f,alpha);
		yColor = new Color(0.23f,0.25f,0.1f,alpha);

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

		if (pauseInterval >= 0 || retractInterval >= 0)
		{
			float lifeCompleted = 1 - (lifeTime / fullLifeTime);

			if (lifeCompleted >= pauseInterval && !paused)
			{
				paused = true;
				scaleRate = 0;
			}
			if (lifeCompleted >= retractInterval && lifeTime > 0 && paused)
			{
				paused = false;
				float scaleToRemove = Mathf.Max(Mathf.Max(transform.localScale.x, transform.localScale.y), transform.localScale.z);
				scaleRate = -scaleToRemove / lifeTime;
			}
		}

		transform.localScale += new Vector3(scaleRate*Time.deltaTime,scaleRate*Time.deltaTime,0);
		if (fadeAlpha)
		{
			float alphaFade = lifeTime >= 0 ? alpha / lifeTime : 0;
			alpha = Mathf.Max(alpha - Time.deltaTime * alphaFade, 0);
			mycolor.a = alpha;
			renderer.material.color = mycolor;
		}
		lifeTime -= Time.deltaTime;
		
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
