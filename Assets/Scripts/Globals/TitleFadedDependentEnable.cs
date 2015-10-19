using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TitleFadedDependentEnable : MonoBehaviour {

	public List<GameObject> enableTargets;
	public bool enableWhenFaded = true;
	private bool wasTitleFaded = false;

	void Awake()
	{
		if (Globals.Instance != null)
		{
			wasTitleFaded = !Globals.Instance.titleScreenFaded;
		}
	}

	void Update()
	{
		if (Globals.Instance != null)
		{
			if (wasTitleFaded != Globals.Instance.titleScreenFaded)
			{
				for (int i = 0; i < enableTargets.Count; i++)
				{
					if (enableTargets[i] != null)
					{
						enableTargets[i].SetActive(enableWhenFaded == Globals.Instance.titleScreenFaded);
					}
				}
			}
			wasTitleFaded = Globals.Instance.titleScreenFaded;
		}
	}
}
