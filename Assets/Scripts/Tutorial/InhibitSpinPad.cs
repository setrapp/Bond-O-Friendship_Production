using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InhibitSpinPad : MonoBehaviour {

	public SpinPad spinPad;
	[SerializeField]
	public List<InhibitIgnore> spinIgnoreCollisions;
	[SerializeField]
	public List<InhibitIgnore> backSpinIgnoreCollisions;
	//public bool collided;
	public bool ignoreStream = true;

	void OnCollisionEnter(Collision col)
	{

		if (ignoreStream && LayerMask.LayerToName(col.gameObject.layer) == "Water")
		{
			return;
		}

		if (!CheckIgnore(col.collider))
		{
			spinPad.spinInhibitors++;
		}
		if (!CheckBackIgnore(col.collider))
		{
			spinPad.spinBackInhibitors++;
		}
	}
	
	void OnCollisionExit(Collision col)
	{
		if (ignoreStream && LayerMask.LayerToName(col.gameObject.layer) == "Water")
		{
			return;
		}
		if (!CheckIgnore(col.collider))
		{
			spinPad.spinInhibitors--;
		}
		if (!CheckBackIgnore(col.collider))
		{
			spinPad.spinBackInhibitors--;
		}
	}

	private bool CheckIgnore(Collider collider)
	{
		bool found = false;
		for (int i = 0; i < spinIgnoreCollisions.Count; i++)
		{
			bool inRange = spinPad.rotationProgress >= spinIgnoreCollisions[i].ignoreMinProgress && spinPad.rotationProgress <= spinIgnoreCollisions[i].ignoreMaxProgress;
			if (spinIgnoreCollisions[i].ignoreCollider == collider && inRange)
			{

				found = true;
			}
		}
		return found;
	}

	private bool CheckBackIgnore(Collider hitCollider)
	{
		bool found = false;
		for (int i = 0; i < backSpinIgnoreCollisions.Count; i++)
		{
			bool inRange = spinPad.rotationProgress >= backSpinIgnoreCollisions[i].ignoreMinProgress && spinPad.rotationProgress <= backSpinIgnoreCollisions[i].ignoreMaxProgress;
			if (backSpinIgnoreCollisions[i].ignoreCollider == hitCollider && inRange)
			{
				found = true;
			}
		}
		return found;
	}
}

[System.Serializable]
public class InhibitIgnore
{
	public Collider ignoreCollider;
	public float ignoreMinProgress = -1f;
	public float ignoreMaxProgress = 1f;
}
