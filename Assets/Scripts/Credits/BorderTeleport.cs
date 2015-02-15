using UnityEngine;
using System.Collections;

public class BorderTeleport : MonoBehaviour {

	private GameObject player1;
	private GameObject player2;
	private Vector3 player1ViewPort;
	private Vector3 player2ViewPort;
	private bool teleportedP1;
	private bool teleportedP2;
	private float borderMin = -0.03f;
	private float borderMax = 1.03f;
	private float trailTimerP1;
	private float trailTimerP2;
	private float trailTimeMax = 0.3f;
	
	// Use this for initialization
	void Start () {
		player1 = Globals.Instance.player1.gameObject;
		player2 = Globals.Instance.player2.gameObject;
		trailTimerP1 = trailTimeMax;
		trailTimerP2 = trailTimeMax;
	}
	
	// Update is called once per frame
	void Update () {
		if(teleportedP1 == true)
		{
			trailTimerP1 -= Time.deltaTime;
			if(trailTimerP1 < 0)
			{
				Globals.Instance.player1.character.leftTrail.enabled = true;
				Globals.Instance.player1.character.midTrail.enabled = true;
				Globals.Instance.player1.character.rightTrail.enabled = true;
				teleportedP1 = false;
				trailTimerP1 = trailTimeMax;
			}
		}
		if(teleportedP2 == true)
		{
			trailTimerP2 -= Time.deltaTime;
			if(trailTimerP2 < 0)
			{
				Globals.Instance.player2.character.leftTrail.enabled = true;
				Globals.Instance.player2.character.midTrail.enabled = true;
				Globals.Instance.player2.character.rightTrail.enabled = true;
				teleportedP2 = false;
				trailTimerP2 = trailTimeMax;
			}
		}
		if(Camera.main.WorldToViewportPoint(player1.transform.position).x < borderMin)
		{
			player1ViewPort = Camera.main.WorldToViewportPoint(player1.transform.position);
			player1.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(borderMax, player1ViewPort.y, player1ViewPort.z));
			teleportedP1 = true;
		}
		if(Camera.main.WorldToViewportPoint(player1.transform.position).x > borderMax)
		{
			player1ViewPort = Camera.main.WorldToViewportPoint(player1.transform.position);
			player1.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(borderMin, player1ViewPort.y, player1ViewPort.z));
			teleportedP1 = true;
		}
		if(Camera.main.WorldToViewportPoint(player1.transform.position).y < borderMin)
		{
			player1ViewPort = Camera.main.WorldToViewportPoint(player1.transform.position);
			player1.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(player1ViewPort.x, borderMax, player1ViewPort.z));
			teleportedP1 = true;
		}
		if(Camera.main.WorldToViewportPoint(player1.transform.position).y > borderMax)
		{
			player1ViewPort = Camera.main.WorldToViewportPoint(player1.transform.position);
			player1.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(player1ViewPort.x, borderMin, player1ViewPort.z));
			teleportedP1 = true;
		}
		if(Camera.main.WorldToViewportPoint(player2.transform.position).x < borderMin)
		{
			player2ViewPort = Camera.main.WorldToViewportPoint(player2.transform.position);
			player2.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(borderMax, player2ViewPort.y, player2ViewPort.z));
			teleportedP2 = true;
		}
		if(Camera.main.WorldToViewportPoint(player2.transform.position).x > borderMax)
		{
			player2ViewPort = Camera.main.WorldToViewportPoint(player2.transform.position);
			player2.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(borderMin, player2ViewPort.y, player2ViewPort.z));
			teleportedP2 = true;
		}
		if(Camera.main.WorldToViewportPoint(player2.transform.position).y < borderMin)
		{
			player2ViewPort = Camera.main.WorldToViewportPoint(player2.transform.position);
			player2.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(player2ViewPort.x, borderMax, player2ViewPort.z));
			teleportedP2 = true;
		}
		if(Camera.main.WorldToViewportPoint(player2.transform.position).y > borderMax)
		{
			player2ViewPort = Camera.main.WorldToViewportPoint(player2.transform.position);
			player2.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(player2ViewPort.x, borderMin, player2ViewPort.z));
			teleportedP2 = true;
		}
		if(teleportedP1 == true)
		{
			Globals.Instance.player1.character.leftTrail.enabled = false;
			Globals.Instance.player1.character.midTrail.enabled = false;
			Globals.Instance.player1.character.rightTrail.enabled = false;
		}
		if(teleportedP2 == true)
		{
			Globals.Instance.player2.character.leftTrail.enabled = false;
			Globals.Instance.player2.character.midTrail.enabled = false;
			Globals.Instance.player2.character.rightTrail.enabled = false;
		}
	}
}
