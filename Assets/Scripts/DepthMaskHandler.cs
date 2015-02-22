using UnityEngine;
using System.Collections;

public class DepthMaskHandler : MonoBehaviour {

	//[HideInInspector]
	public GameObject depthMask;
	public bool autoCreate = true;

	void Update () {
		if (Globals.Instance.visibilityDepthMaskNeeded)
		{
			if (depthMask != null)
			{
				depthMask.transform.position = new Vector3(transform.position.x, transform.position.y, -10.0f);
			}
			else if (autoCreate)
			{
				CreateDepthMask();
			}
		}
		else
		{
			if (depthMask != null)
			{
				Destroy(depthMask);
			}
		}
	}

	public void CreateDepthMask()
	{
		if (depthMask == null)
		{
			depthMask = (GameObject)Instantiate(Globals.Instance.depthMaskPrefab, new Vector3(transform.position.x, transform.position.y, -10.0f), Quaternion.identity);
			depthMask.transform.parent = DepthMaskHolder.Instance.transform;
		}
	}

	void OnDestroy()
	{
		if (depthMask != null)
		{
			Destroy(depthMask.gameObject);
		}
	}
}
