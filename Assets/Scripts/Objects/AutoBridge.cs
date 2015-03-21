using UnityEngine;
using System.Collections;

public class AutoBridge : MonoBehaviour {
	public bool extending = false;
	public bool fullyExtended = false;
	private Vector3 startingPos;
	public GameObject endingTarget;
	public float scaleRate = 1;

	void Update()
	{
		if (extending && !fullyExtended)
		{
			Vector3 extendedScale = transform.localScale;
			float scaleIncrement = scaleRate * Time.deltaTime;
			if ((endingTarget.transform.position - startingPos).magnitude < (transform.localScale.z + scaleIncrement) / 2)
			{
				scaleIncrement = (endingTarget.transform.position - startingPos).magnitude - (transform.localScale.z / 2);
				fullyExtended = true;
			}

			extendedScale.z += scaleIncrement;
			transform.localScale = extendedScale;
			//transform.position = startingPos + new Vector3(0, 0, transform.localScale.z / 2);

		}
	}

	public void FunnelSolved(BondingFunnel funnel)
	{
		if (funnel.solved && !extending && !fullyExtended)
		{
			extending = true;
			transform.LookAt(endingTarget.transform);
			startingPos = transform.position - new Vector3(0, 0, transform.localScale.z / 2);
			
		}
	}
}
