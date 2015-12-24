using UnityEngine;
using System.Collections;

public class WaitPad : MonoBehaviour {

	public bool pOonPad = false;
	public bool pTonPad = false;
	protected Color mycolor;
	protected float red;
	public float waitRate = 1;
	public bool neverActivate = false;
	public bool activated = false;
	public float portionComplete;
	[HideInInspector]
	public Renderer renderer;

	// Use this for initialization
	virtual protected void Start()
	{
		red = 0.1f;
		//turnTime = 0.3f;
		renderer = GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	virtual protected void Update()
	{
		mycolor = new Color(red,0.3f,0.5f, renderer.material.color.a);
		renderer.material.color = mycolor;

		if(pOonPad == true && pTonPad == true)
		{
			//renderer.material.color = Color.magenta;
			if(red < 1.0f)
				red += Time.deltaTime*waitRate;
			portionComplete += Time.deltaTime*waitRate;
		}
		if(pOonPad == false || pTonPad == false)
		{
			if(red > 0.1f)
				red -= Time.deltaTime*waitRate;
			portionComplete -= Time.deltaTime*waitRate;
		}
		if(red >= 1 && !neverActivate)
		{
			activated = true;
		}
		if(activated)
		{
			//print ("activated");
		}

		portionComplete = Mathf.Clamp01(portionComplete);
	}
	virtual protected void OnTriggerEnter(Collider collide)
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
	virtual protected void OnTriggerExit(Collider collide)
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
