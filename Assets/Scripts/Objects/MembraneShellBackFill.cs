using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MembraneShellBackFill : MonoBehaviour {

	public MembraneShellFill triggerFill;
	private bool filling = false;
	public Renderer backFillRenderer;
	public float fadeSpeed = 1;
	public bool resetOnBlack = true;
	public Island completedLevel;

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

					if (color.r < 0) { color.r = 0; }
					if (color.g < 0) { color.g = 0; }
					if (color.b < 0) { color.b = 0; }

					backFillRenderer.material.color = color;
					if (backFillRenderer.material.color.r <= 0 && backFillRenderer.material.color.g <= 0 && backFillRenderer.material.color.b <= 0)
					{
						if (completedLevel != null && Globals.Instance != null && Globals.Instance.levelsCompleted != null)
						{
							Globals.Instance.levelsCompleted[(int)completedLevel.islandId] = true;
						}
						Globals.Instance.fromContinue = true;
						Globals.Instance.ResetOrExit();
					}
				}
			}
		}
	}
}
