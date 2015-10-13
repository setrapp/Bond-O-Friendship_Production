using UnityEngine;
using System.Collections;

public class DarknessTrigger : MonoBehaviour {

	public SetShaderData_DarkAlphaMasker targetMask = null;
	public bool forceOn = false;

	void Start()
	{
		if (targetMask == null && Globals.Instance != null)
		{
			targetMask = Globals.Instance.darknessMask;
		}

		if (targetMask != null)
		{
			targetMask.trigger = this;
			if (!targetMask.fadeIn && !forceOn)
			{
				gameObject.SetActive(false);
			}
		}
	}
}
