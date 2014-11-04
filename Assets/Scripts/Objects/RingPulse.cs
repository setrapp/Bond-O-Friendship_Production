using UnityEngine;
using System.Collections;

public class RingPulse : MonoBehaviour {

	private float lifeTime;
	private float alpha;
	private Color mycolor;
	private float scaleRate;

	// Use this for initialization
	void Start () {
		scaleRate = 0.1f;
		lifeTime = 2.0f;
		alpha = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
		mycolor = new Color(0.8f,0.1f,0.5f,alpha);

		transform.localScale += new Vector3(scaleRate,scaleRate,0);
		renderer.material.color = mycolor;
		lifeTime -= Time.deltaTime;
		alpha -= Time.deltaTime*0.3f;
		if(lifeTime <= 0)
		{
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter(Collider collide)
	{
		if(collide.gameObject.name == "Shield")
		{
			gameObject.collider.enabled = false;
		}
		if(collide.gameObject.tag == "Converser")
		{
			gameObject.collider.enabled = false;
			print("Collide");
			collide.gameObject.GetComponent<PartnerLink>().BreakAllConnections();
		}
	}
}
