using UnityEngine;
using System.Collections;

public class ContinueDependentEnable : MonoBehaviour {

	public GameObject enableTarget;
	public bool enableOnContinue = true;

	void Awake()
	{
		if (enableTarget != null && Globals.Instance != null)
		{
			enableTarget.SetActive(enableOnContinue == Globals.Instance.fromContinue);
		}
	}

}
