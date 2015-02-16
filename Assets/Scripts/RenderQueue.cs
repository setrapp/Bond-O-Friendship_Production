using UnityEngine;
using System.Collections;

public class RenderQueue : MonoBehaviour {
	public Renderer targetRenderer;
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
		if (targetRenderer == null)
		{
			targetRenderer = GetComponent<Renderer>();
		}

		if (targetRenderer != null && targetRenderer.material != null)
		{
			targetRenderer.material.renderQueue = (int)renderBase + renderOffset;
		}
	}
}
