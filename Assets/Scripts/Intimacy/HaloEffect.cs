using UnityEngine;
using System.Collections;

public class HaloEffect : MonoBehaviour {

	private FluffHandler fluffHandler;
	private int fluffCountAdjust;
	private Light light;

	public int fluffMax = 10;
	public float haloRange = 8;

	// Use this for initialization
	void Start () {

		light = GetComponent<Light>();
		if(transform.name == "Player 1 Halo")
		{
			fluffHandler = Globals.Instance.player1.gameObject.GetComponent<FluffHandler>();
			transform.parent = Globals.Instance.player1.transform;
		}
		else
		{
			fluffHandler = Globals.Instance.player2.gameObject.GetComponent<FluffHandler>();
			transform.parent = Globals.Instance.player2.transform;
		}
	}
	
	// Update is called once per frame
	void Update () {

		fluffCountAdjust = (int)Mathf.Round(fluffMax/Vector3.Distance(Globals.Instance.player1.transform.position, Globals.Instance.player2.transform.position));
		if(fluffCountAdjust < 1)
			fluffCountAdjust = 1;
		fluffHandler.naturalFluffCount = fluffCountAdjust;
		light.range = haloRange/Vector3.Distance(Globals.Instance.player1.transform.position, Globals.Instance.player2.transform.position);
	}
}
