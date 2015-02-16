using UnityEngine;
using System.Collections;

public class CamerWriteDepth : MonoBehaviour {
	public Camera targetCamera;
	public DepthTextureMode depthTextureMode;

	void Start()
	{
		if (targetCamera == null)
		{
			targetCamera = GetComponent<Camera>();
		}

		if (targetCamera != null)
		{
			targetCamera.depthTextureMode = depthTextureMode;
		}
	}
}
