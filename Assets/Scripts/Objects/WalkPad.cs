using UnityEngine;
using System.Collections;

public class WalkPad : WaitPad {
	public Renderer padRenderer;
	public GameObject startPoint;
	public GameObject endPoint;
	public Color startColor;
	public Color endColor;
	public float portionComplete = 0;

	override protected void Start()
	{
		if (padRenderer == null)
		{
			padRenderer = GetComponent<Renderer>();
		}
		padRenderer.material.color = startColor;
	}

	override protected void Update()
	{
		if (pOonPad && pTonPad && !activated)
		{
			CharacterComponents player1 = Globals.Instance.player1.character;
			CharacterComponents player2 = Globals.Instance.player2.character;
		
			Vector3 toEnd = endPoint.transform.position - startPoint.transform.position;
			/*TODO something is wrong with vector projection here.*/
			float player1Progress = (Helper.ProjectVector(player1.transform.position - startPoint.transform.position, toEnd)).magnitude;
			float player2Progress = (Helper.ProjectVector(player2.transform.position - startPoint.transform.position, toEnd)).magnitude;

			portionComplete = player1Progress ;
			if (player2Progress < player1Progress)
			{
				portionComplete = player2Progress;
			}
			portionComplete /= toEnd.magnitude;

			if (portionComplete >= 1)
			{
				activated = true;
				portionComplete = 1;
			}

			padRenderer.material.color = (startColor * (1 - portionComplete)) + (endColor * portionComplete);
			Debug.Log(portionComplete);
		}
	}
}
