using UnityEngine;
using System.Collections;

public class DisableOutsideEditor : MonoBehaviour {

	void Start()
	{
		if (!Application.isEditor)
		{
			gameObject.SetActive(false);
		}
	}
}
