using UnityEngine;
using System.Collections;

public class BondingFunnelStopTrigger : MonoBehaviour {
	public BondingFunnel targetFunnel;
	public Collider triggerer;


	void OnTriggerEnter(Collider col)
	{
		if (col == triggerer)
		{
			targetFunnel.StopBackTracking();
		}
	}
}
