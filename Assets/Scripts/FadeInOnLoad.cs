using UnityEngine;
using System.Collections;

public class FadeInOnLoad : MonoBehaviour {

	public Renderer fadeRenderer;
	public IslandContainer triggerIsland;
	public float fadeTime = 1;
	
	
	void Start()
	{
		if (fadeRenderer == null)
		{
			fadeRenderer = GetComponent<Renderer>();
		}
	
		if (fadeRenderer != null && CameraSplitter.Instance != null && CameraSplitter.Instance.splitCamera1 != null)
		{
			Color fadeColor = CameraSplitter.Instance.splitCamera1.backgroundColor;
			fadeColor.a = 1;
			fadeRenderer.material.color = fadeColor;
		}
		else
		{
			Destroy(gameObject);
		}
		
		if (triggerIsland != null && triggerIsland.spawnOnStart)
		{
			fadeRenderer.enabled = true;
		}
	}
	
	void Update()
	{
		if (triggerIsland != null && triggerIsland.island != null && fadeRenderer != null && CameraSplitter.Instance != null && CameraSplitter.Instance.splitCamera1 != null)
		{
			if (fadeTime <= 0)
			{
				Destroy(gameObject);
			}
		
			Color fadeColor = CameraSplitter.Instance.splitCamera1.backgroundColor;
			fadeColor.a = Mathf.Max(fadeRenderer.material.color.a - (1 / fadeTime * Time.deltaTime), 0);
			fadeRenderer.material.color = fadeColor;
			if (fadeColor.a <= 0)
			{
				Destroy(gameObject);
			} 
		}
	}
}
