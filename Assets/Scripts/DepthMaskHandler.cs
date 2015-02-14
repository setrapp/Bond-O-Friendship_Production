using UnityEngine;
using System.Collections;

public class DepthMaskHandler : MonoBehaviour {

	//[HideInInspector]
	public GameObject depthMask;

	void Update () {
		if (Globals.Instance.visibilityDepthMaskNeeded)
		{
			if (depthMask != null)
			{
				depthMask.transform.position = new Vector3(transform.position.x, transform.position.y, -10.0f);
			}
			else
			{
				depthMask = (GameObject)Instantiate(Globals.Instance.depthMaskPrefab, new Vector3(transform.position.x, transform.position.y, -10.0f), Quaternion.identity);
				depthMask.transform.parent = DepthMaskHolder.Instance.transform;
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
}
