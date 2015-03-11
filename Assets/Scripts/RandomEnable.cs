using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomEnable : MonoBehaviour {
	[SerializeField]
	public List<GameObject> targets;
	public int enableCount = 1;

	void Awake()
	{
		List<GameObject> tempTargets = new List<GameObject>();
		for (int i = 0; i < targets.Count; i++)
		{
			if (targets[i] != null)
			{
				targets[i].gameObject.SetActive(false);
				tempTargets.Add(targets[i]);
			}
		}

		int targetsEnabled = 0;
		while (targetsEnabled < enableCount && tempTargets.Count > 0)
		{
			int randPad = Random.Range(0, tempTargets.Count);
			tempTargets[randPad].SetActive(true);
			tempTargets.RemoveAt(randPad);
			targetsEnabled++;
		}
	}
}
