using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ContinueDependentEnable : MonoBehaviour {

	public List<GameObject> enableTargets;
	public bool enableOnContinue = true;
	private bool wasContinue = false;

	void Awake()
	{
		if (Globals.Instance != null)
		{
			wasContinue = !Globals.Instance.fromContinue;
		}
	}

	void Update()
	{
		if (Globals.Instance != null)
		{
			if (wasContinue != Globals.Instance.fromContinue)
			{
				for (int i = 0; i < enableTargets.Count; i++)
				{
					if (enableTargets[i] != null)
					{
						enableTargets[i].SetActive(enableOnContinue == Globals.Instance.fromContinue);
					}
				}
			}
			wasContinue = Globals.Instance.fromContinue;
		}
		
	}

}
