using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MembraneShellBackFill : MonoBehaviour {

	public MembraneShellFill triggerFill;
	private bool filling = false;
	public Renderer backFillRenderer;
	public float fadeSpeed = 1;

	void Start()
	{
		if (backFillRenderer != null)
		{
			Color color = backFillRenderer.material.color;
			color.a = 0;
			backFillRenderer.material.color = color;
		}
	}

	void Update()
	{
		if (triggerFill != null && triggerFill.atMaxBurst)
		{
			transform.localScale = triggerFill.transform.localScale;
			if (backFillRenderer != null)
			{
				Color color = backFillRenderer.material.color;
				if (color.a < 1)
				{
					
					color.a += Time.deltaTime * fadeSpeed;
					if (color.a >= 1)
					{
						color.a = 1;
						triggerFill.gameObject.SetActive(false);
					}
					backFillRenderer.material.color = color;
				}
				else if (color.r > 0 && color.g > 0 && color.b > 0)
				{
					float maxComponent = Mathf.Max(new float[3] { color.r, color.g, color.b });
					color.r -= Time.deltaTime * fadeSpeed * (color.r / maxComponent);
					color.g -= Time.deltaTime * fadeSpeed * (color.g / maxComponent);
					color.b -= Time.deltaTime * fadeSpeed * (color.b / maxComponent);
					backFillRenderer.material.color = color;
				}
			}
		}
	}
}
