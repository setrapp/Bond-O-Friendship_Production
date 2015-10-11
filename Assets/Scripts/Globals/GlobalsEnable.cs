using UnityEngine;
using System.Collections;

public class GlobalsEnable : MonoBehaviour {

	public Globals targetGlobals;

	void Awake()
	{
		if (Globals.Instance == null)
		{
			targetGlobals.gameObject.SetActive(true);
		}
		else if (Globals.Instance != targetGlobals)
		{
			//Destroy(targetGlobals.gameObject);
		}
	}
}
