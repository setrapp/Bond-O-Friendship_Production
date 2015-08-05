using UnityEngine;
using System.Collections;

public class FindLuminusTrigger : MonoBehaviour {

	void Start()
	{
		if (Globals.Instance != null && Globals.Instance.darknessMask != null)
		{
			FindClosestLuminus luminusFinder = Globals.Instance.darknessMask.GetComponent<FindClosestLuminus>();
			if (luminusFinder != null)
			{
				luminusFinder.FindAllLumini();
			}
		}
	}
}
