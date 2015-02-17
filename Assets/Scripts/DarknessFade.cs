using UnityEngine;
using System.Collections;

public class DarknessFade : MonoBehaviour
{
	public DarknessLayers darkness;
	public WaitPad waitPad;
	private Renderer[] renderers;
	public float fadeSpeed = 0.01f;

	void Start()
	{
		if (darkness == null)
		{
			darkness = GetComponent<DarknessLayers>();
		}
		renderers = GetComponentsInChildren<Renderer>();
	}

	void Update()
	{
		if (waitPad != null && waitPad.activated)
		{
			for (int i = 0; i < renderers.Length; i++)
			{
				if (renderers[i] != null)
				{
					Color darkColor = renderers[i].material.color;
					darkColor.a -= fadeSpeed;
					renderers[i].material.color = darkColor;
					if(darkColor.a <= 0)
					{
						Destroy(renderers[i].gameObject);
						renderers[i] = null;
					}
				}
				
			}

			bool allFaded = true;
			for (int i = 0; i < renderers.Length && allFaded; i++)
			{
				if (renderers[i] != null)
				{
					allFaded = false;
				}
			}
			if (allFaded)
			{
				Destroy(gameObject);
			}
		}
	}
}
