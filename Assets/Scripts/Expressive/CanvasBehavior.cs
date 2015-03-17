using UnityEngine;
using System.Collections;

public class CanvasBehavior : MonoBehaviour {

	public GameObject player1;
	public GameObject player2;
	private Color canvasColor;
	private float alpha;
	public bool changeColor = true;

	public GameObject pal1;
	public GameObject pal2;
	public GameObject pal3;
	public GameObject pal4;

	private float colorJitter;

	public float r1;
	public float g1;
	public float b1;
	public float a1;

	public float r2;
	public float g2;
	public float b2;
	public float a2;

	public float r3;
	public float g3;
	public float b3;
	public float a3;

	public float r4;
	public float g4;
	public float b4;
	public float a4;




	// Use this for initialization
	
	void Start () {
		alpha = 0.0f;
		canvasColor = new Color(0.0f,0.0f,0.0f,alpha);
		if (changeColor)
		{
			gameObject.GetComponent<Renderer>().material.color = canvasColor;
		}

	}
	
	// Update is called once per frame
	void Update () {

		pal1.GetComponent<Palette>().r = r1;
		pal1.GetComponent<Palette>().g = g1;
		pal1.GetComponent<Palette>().b = b1;
		pal1.GetComponent<Palette>().a = a1;

		pal2.GetComponent<Palette>().r = r2;
		pal2.GetComponent<Palette>().g = g2;
		pal2.GetComponent<Palette>().b = b2;
		pal2.GetComponent<Palette>().a = a2;

		pal3.GetComponent<Palette>().r = r3;
		pal3.GetComponent<Palette>().g = g3;
		pal3.GetComponent<Palette>().b = b3;
		pal3.GetComponent<Palette>().a = a3;

		pal4.GetComponent<Palette>().r = r4;
		pal4.GetComponent<Palette>().g = g4;
		pal4.GetComponent<Palette>().b = b4;
		pal4.GetComponent<Palette>().a = a4;



		if (changeColor)
		{
			canvasColor = new Color(0.8f, 0.9f, 0.8f, alpha);
			gameObject.GetComponent<Renderer>().material.color = canvasColor;
			if (alpha < 1.0f)
			{
				alpha += Time.deltaTime * 0.5f;
			}
		}
	
	}

	void OnTriggerEnter (Collider collide)
	{
		if(collide.gameObject.name == "Player 1")
		{
			player1 = collide.gameObject;
			player1.GetComponent<Paint>().painting = true;

		}
		if(collide.gameObject.name == "Player 2")
		{
			player2 = collide.gameObject;
			player2.GetComponent<Paint>().painting = true;

		}
	}

	void OnTriggerExit (Collider collide)
	{
		if(collide.gameObject.name == "Player 1")
		{
			player1 = collide.gameObject;
			player1.GetComponent<Paint>().painting = false;
			//print ("Paintfalse");
		}
		if(collide.gameObject.name == "Player 2")
		{
			player2 = collide.gameObject;
			player2.GetComponent<Paint>().painting = false;
			//print ("Paint");
		}
	}
}
