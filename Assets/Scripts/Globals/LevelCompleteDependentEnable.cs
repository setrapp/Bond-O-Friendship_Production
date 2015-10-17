using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelCompleteDependentEnable : MonoBehaviour {

	public List<GameObject> enableTargets;
	public bool[] requiredLevels;
	public bool enableWhenComplete = true;

	void Awake()
	{
		if (Globals.Instance != null)
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
	}

}
