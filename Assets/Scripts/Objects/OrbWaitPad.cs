using UnityEngine;
using System.Collections;

public class OrbWaitPad : MonoBehaviour {

	public bool pOonPad = false;
	public bool pTonPad = false;
	public Material activatedSphereColor;
	private Color mycolor;
	private float red;
	private float turnTime;
	public bool activated = false;
	private int triggersLit = 0;
	public int maxTriggers;
	private bool fullyLit;
	public GameObject[] activationSpheres;
	public ParticleSystem activatedParticle;
	public GameObject optionalGate;

	// Use this for initialization
	void Start () {
		red = 0.1f;
		turnTime = 0.3f;
		activationSpheres = new GameObject[transform.parent.childCount];
		for(int i = 0; i < transform.parent.childCount; i++)
		{
			if(transform.parent.GetChild(i).name == "Activation Sphere" && activationSpheres[i] == null)
					activationSpheres[i] = transform.parent.GetChild(i).gameObject;
		}
		if (activatedParticle != null)
		{
			activatedParticle.Stop();
		}
	}
	
	// Update is called once per frame
	void Update () {
		mycolor = new Color(red,0.3f,0.5f);
		GetComponent<Renderer>().material.color = mycolor;

	if(pOonPad == true && pTonPad == true)
		{
			//renderer.material.color = Color.magenta;
			if(red < 1.0f)
			red += Time.deltaTime*turnTime;
		}
		if(pOonPad == false || pTonPad == false)
		{
			if(red > 0.1f)
			red -= Time.deltaTime;
		}
		if(red >= 1)
		{
			activated = true;
		}
		if(activated)
		{
			//print ("activated");
		}
	}
	void OnTriggerEnter(Collider collide)
	{
		if(collide.name == "Blossom")
		{
			for(int i = 0; i < transform.parent.childCount; i++)
			{
				if(fullyLit == false && activationSpheres[i] != null)
				{
					Destroy(collide.gameObject);
					activationSpheres[i].GetComponent<Renderer>().material = activatedSphereColor;
					ParticleSystem tempParticle = (ParticleSystem)Instantiate(activatedParticle);
					tempParticle.transform.position = activationSpheres[i].transform.position;
					activationSpheres[i] = null;
					//activatedParticle.Play();
					if(optionalGate != null)
						Destroy(optionalGate);
					break;
				}
			}
			triggersLit++;
			if(triggersLit == maxTriggers)
				fullyLit = true;
		}
		if(fullyLit == true)
		{
			if(collide.gameObject.name == "Player 1")
			{
				pOonPad = true;
				//print("1");
			}
			if(collide.gameObject.name == "Player 2")
			{
				pTonPad = true;
				//print ("2");
			}
		}

	}
	void OnTriggerExit(Collider collide)
	{
		if(collide.gameObject.name == "Player 1")
		{
			pOonPad = false;
		}
		if(collide.gameObject.name == "Player 2")
		{
			pTonPad = false;
		}
	}
}
