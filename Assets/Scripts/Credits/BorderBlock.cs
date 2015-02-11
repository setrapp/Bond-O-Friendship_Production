using UnityEngine;
using System.Collections;

public class BorderBlock : MonoBehaviour {
	
	private GameObject player1;
	private GameObject player2;
	private Vector3 player1ViewPort;
	private Vector3 player2ViewPort;
	private float borderMin = -0.005f;
	private float borderMax = 1.005f;
	private CameraSplitter cameraSplit;

	
	// Use this for initialization
	void Start () {
		player1 = Globals.Instance.player1.gameObject;
		player2 = Globals.Instance.player2.gameObject;
		cameraSplit = GameObject.Find("Camera System").GetComponent<CameraSplitter>();
		cameraSplit.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {

		if(Camera.main.WorldToViewportPoint(player1.transform.position).x < borderMin)
		{
			player1ViewPort = Camera.main.WorldToViewportPoint(player1.transform.position);
			player1.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(borderMin, player1ViewPort.y, player1ViewPort.z));
		}
		if(Camera.main.WorldToViewportPoint(player1.transform.position).x > borderMax)
		{
			player1ViewPort = Camera.main.WorldToViewportPoint(player1.transform.position);
			player1.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(borderMax, player1ViewPort.y, player1ViewPort.z));
		}
		if(Camera.main.WorldToViewportPoint(player1.transform.position).y < borderMin)
		{
			player1ViewPort = Camera.main.WorldToViewportPoint(player1.transform.position);
			player1.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(player1ViewPort.x, borderMin, player1ViewPort.z));
		}
		if(Camera.main.WorldToViewportPoint(player1.transform.position).y > borderMax)
		{
			player1ViewPort = Camera.main.WorldToViewportPoint(player1.transform.position);
			player1.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(player1ViewPort.x, borderMax, player1ViewPort.z));
		}

		if(Camera.main.WorldToViewportPoint(player2.transform.position).x < borderMin)
		{
			player2ViewPort = Camera.main.WorldToViewportPoint(player2.transform.position);
			player2.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(borderMin, player2ViewPort.y, player2ViewPort.z));
		}
		if(Camera.main.WorldToViewportPoint(player2.transform.position).x > borderMax)
		{
			player2ViewPort = Camera.main.WorldToViewportPoint(player2.transform.position);
			player2.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(borderMax, player2ViewPort.y, player2ViewPort.z));
		}
		if(Camera.main.WorldToViewportPoint(player2.transform.position).y < borderMin)
		{
			player2ViewPort = Camera.main.WorldToViewportPoint(player2.transform.position);
			player2.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(player2ViewPort.x, borderMin, player2ViewPort.z));
		}
		if(Camera.main.WorldToViewportPoint(player2.transform.position).y > borderMax)
		{
			player2ViewPort = Camera.main.WorldToViewportPoint(player2.transform.position);
			player2.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(player2ViewPort.x, borderMax, player2ViewPort.z));
		}
	}
}
