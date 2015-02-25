using UnityEngine;
using System.Collections;

public class DepthMaskHolder : MonoBehaviour {
	private static DepthMaskHolder instance = null;
	public static DepthMaskHolder Instance
	{
		get
		{
			if (instance == null && Globals.Instance != null)
			{

				GameObject depthMaskHolderObj = (GameObject)Instantiate(Globals.Instance.depthMaskHolderPrefab);
				if (depthMaskHolderObj != null)
				{
					instance = depthMaskHolderObj.GetComponent<DepthMaskHolder>();
				}
			}
			return instance;
		}
	}
}
