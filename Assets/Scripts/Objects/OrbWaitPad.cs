using UnityEngine;
using System.Collections;

public class OrbWaitPad : WaitPad {

	public Material activatedSphereColor;
	private int triggersLit = 0;
	public int maxTriggers;
	public bool fullyLit;
	public GameObject[] activationSpheres;
	public ParticleSystem activatedParticle;
	public GameObject optionalGate;
	private bool gateClosing;
	private float gateCloseSpeed;
	private float gateXPos;
	public float gateCloseTime = 20;
	private int sphereCount;
	public bool requirePlayers = true;
    public AudioSource complete;

	override protected void Start () {
        red = 0.8f;
		waitRate = 0.3f;
		activationSpheres = new GameObject[maxTriggers];
		for(int i = 0; i < transform.parent.childCount; i++)
		{
			if(transform.parent.GetChild(i).name == "Activation Sphere" && activationSpheres[sphereCount] == null)
			{
				activationSpheres[sphereCount] = transform.parent.GetChild(i).gameObject;
				sphereCount++;
			}
		}
		if (activatedParticle != null)
		{
			activatedParticle.Stop();
		}
		if(optionalGate != null)
		{
			gateCloseSpeed = optionalGate.transform.localScale.x/(gateCloseTime*120);
			gateXPos = optionalGate.transform.position.x - optionalGate.transform.localScale.x/2;
		}
	}

	override protected void Update()
	{
        mycolor = new Color(red, 0.8f, 0.3f);
		GetComponent<Renderer>().material.color = mycolor;

		if((!requirePlayers || pOonPad == true && pTonPad == true) && fullyLit)
		{
			//renderer.material.color = Color.magenta;
			if(red < 1.0f)
			red += Time.deltaTime*waitRate;
		}
		if(pOonPad == false || pTonPad == false)
		{
			if(red > 0.1f)
			red -= Time.deltaTime;
		}
		if (fullyLit && (red >= 1 || !requirePlayers))
		{
			activated = true;
		}
		if(activated)
		{
			//print ("activated");
		}
		if(gateClosing == true)
		{
			optionalGate.transform.localScale = new Vector3(optionalGate.transform.localScale.x - gateCloseSpeed, optionalGate.transform.localScale.y, optionalGate.transform.localScale.z);
			optionalGate.transform.position = new Vector3(gateXPos + optionalGate.transform.localScale.x/2, optionalGate.transform.position.y, optionalGate.transform.position.z);
			if(optionalGate.transform.localScale.x <= 0)
			{
				Destroy(optionalGate);
				gateClosing = false;
			}
		}
	}
	void OnTriggerEnter(Collider collide)
	{
		if(collide.name == "Blossom(Clone)")
		{
			for(int i = 0; i < triggersLit+1; i++)
			{
				if(fullyLit == false && activationSpheres[i] != null)
				{
					DepthMaskHandler slotDepthMask = activationSpheres[i].GetComponent<DepthMaskHandler>();
					if (slotDepthMask != null)
					{
						slotDepthMask.CreateDepthMask();
					}

					Destroy(collide.gameObject);
					if(complete != null)
					{
						complete.Play();
					}
					activationSpheres[i].GetComponent<Renderer>().material = activatedSphereColor;
					ParticleSystem tempParticle = (ParticleSystem)Instantiate(activatedParticle);
					tempParticle.transform.position = activationSpheres[i].transform.position;
					Destroy(tempParticle.gameObject, 2.0f);
					activationSpheres[i] = null;
					//activatedParticle.Play();
					if(optionalGate != null && triggersLit >= maxTriggers-1)
						gateClosing = true;
					//break;
				}
			}
			triggersLit++;
			if(triggersLit >= maxTriggers)
				fullyLit = true;
		}
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
