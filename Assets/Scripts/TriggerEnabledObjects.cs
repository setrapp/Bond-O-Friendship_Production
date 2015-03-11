using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerEnabledObjects : MonoBehaviour {
	[SerializeField]
	public List<GameObject> triggerables;
	//public bool startEnabled = false;

	void Awake()
	{
		for (int i = 0; i < triggerables.Count; i++)
		{
			if (triggerables[i] != null)
			{
				triggerables[i].SetActive(false);
			}
		}
	}

	public void ToggleObjects(bool enable)
	{
		for (int i = 0; i < triggerables.Count; i++)
		{
			if (triggerables[i] != null)
			{
				triggerables[i].SetActive(enable);
			}
		}
	}
}
