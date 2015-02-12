using UnityEngine;
using System.Collections;

public class RenderQueue : MonoBehaviour {
	public Renderer renderer;
	public RenderBase renderBase = RenderBase.GEOMETRY;
	public int renderOffset = 0;

	public enum RenderBase
	{
		BACKGROUND = 1000,
		GEOMETRY = 2000,
		ALPHA_TEST = 2450,
		TRANSPARENT = 3000,
		OVERLAY = 4000
	};

	void Start()
	{
		if (renderer == null)
		{
			renderer = GetComponent<Renderer>();
		}

		if (renderer != null && renderer.material != null)
		{
			renderer.material.renderQueue = (int)renderBase + renderOffset;
		}
	}
}
