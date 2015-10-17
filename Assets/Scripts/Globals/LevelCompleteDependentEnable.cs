using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelCompleteDependentEnable : MonoBehaviour {

	public List<GameObject> enableTargets;
	public bool[] requiredLevels;
	public bool enableWhenComplete = true;
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
				bool reqsMet = enableWhenComplete;
				for (int i = 0; i < requiredLevels.Length && i < Globals.Instance.levelsCompleted.Length; i++)
				{
					if (requiredLevels[i] && !Globals.Instance.levelsCompleted[i])
					{
						reqsMet = !enableWhenComplete;
					}
				}

				for (int i = 0; i < enableTargets.Count; i++)
				{
					if (enableTargets[i] != null)
					{
						enableTargets[i].SetActive(reqsMet);
					}
				}
			}
			wasContinue = Globals.Instance.fromContinue;
		}
	}
}
