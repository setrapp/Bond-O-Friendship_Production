using UnityEngine;
using System.Collections;

public class HaloEffect : MonoBehaviour {

	public PlayerInput.Player attachedPlayer;
	private FluffHandler fluffHandler;
	private int fluffCountAdjust;
	private Light targetLight;
	private DepthMaskHandler targetMask;

	public int fluffMax = 10;
	public float haloRange = 20;
	public float minRange = 10;

	// Use this for initialization
	void Start () {

		targetLight = GetComponent<Light>();
		targetMask = GetComponent<DepthMaskHandler>();
		//TODO attach to attached player rather than using name
		if(attachedPlayer == PlayerInput.Player.Player1)
		{
			fluffHandler = Globals.Instance.player1.gameObject.GetComponent<FluffHandler>();
			transform.parent = Globals.Instance.player1.transform;
			transform.localPosition = Vector3.zero;
		}
		else
		{
			fluffHandler = Globals.Instance.player2.gameObject.GetComponent<FluffHandler>();
			transform.parent = Globals.Instance.player2.transform;
			transform.localPosition = Vector3.zero;
		}
	}
	
	// Update is called once per frame
	void Update () {

		fluffCountAdjust = (int)Mathf.Round(fluffMax - Vector3.Distance(Globals.Instance.player1.transform.position, Globals.Instance.player2.transform.position));
		if(fluffCountAdjust < 1)
			fluffCountAdjust = 1;
		fluffHandler.naturalFluffCount = fluffCountAdjust;
		float maskRange = Mathf.Max(minRange, (haloRange - Vector3.Distance(Globals.Instance.player1.transform.position, Globals.Instance.player2.transform.position)));
		targetLight.range = (haloRange - Vector3.Distance(Globals.Instance.player1.transform.position, Globals.Instance.player2.transform.position));
		if (targetMask != null && targetMask.depthMask != null)
		{
			
			targetMask.depthMask.transform.localScale = new Vector3(maskRange, maskRange, targetMask.depthMask.transform.localScale.z);
		}
	}
}
