using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InhibitSpinPad : MonoBehaviour {

	public SpinPad spinPad;
	[SerializeField]
	public List<Collider> spinIgnoreCollisions;
	[SerializeField]
	public List<Collider> backSpinIgnoreCollisions;

	void OnCollisionEnter(Collision col)
	{
		if (!spinIgnoreCollisions.Contains(col.collider))
		{
			spinPad.spinInhibitors++;
		}
		if (!backSpinIgnoreCollisions.Contains(col.collider))
		{
			spinPad.backSpinInhibitors++;
		}
	}
	
	void OnCollisionExit(Collision col)
	{
		if (!spinIgnoreCollisions.Contains(col.collider))
		{
			spinPad.spinInhibitors--;
		}
		if (!backSpinIgnoreCollisions.Contains(col.collider))
		{
			spinPad.backSpinInhibitors--;
		}
	}
}
