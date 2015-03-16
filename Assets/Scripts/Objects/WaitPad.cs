using UnityEngine;
using System.Collections;

public class WaitPad : MonoBehaviour {

	public bool pOonPad = false;
	public bool pTonPad = false;
	protected Color mycolor;
	protected float red;
	protected float turnTime;
	public bool activated = false;

	// Use this for initialization
	virtual protected void Start()
	{
		red = 0.1f;
		turnTime = 0.3f;
	}
	
	// Update is called once per frame
	virtual protected void Update()
	{
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
