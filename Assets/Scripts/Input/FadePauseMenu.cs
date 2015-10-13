using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FadePauseMenu : MonoBehaviour {
	
	public List<Renderer> textRenderers;
	private List<Color> textColorsEmpty = new List<Color>();
	private List<Color> textColorsFull = new List<Color>();

	public float f = 0.0f;
	public float duration = 1.0f;

	public bool colorsSet = false;

	// Use this for initialization
	void Start () 
	{

		//foreach (Renderer renderer in textRenderers)
		//{
		//	renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 0.0f);
		//	textColorsEmpty.Add(renderer.material.color);
		//	textColorsFull.Add(new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 1.0f));
		//}
		
	}

	public void FadeIn()
	{
		if (colorsSet) 
		{
			if (f != 1) {
				f = Mathf.Clamp (f + Time.deltaTime / duration, 0.0f, 1.0f);
				//Player 1 Controls

				//Text
				for (int i = 0; i < textRenderers.Count; i++) {
					textRenderers [i].material.color = Color.Lerp (textColorsEmpty [i], textColorsFull [i], f);
				}

			}
		}
	}

	public void FadeOut()
	{
		if (colorsSet) {
			if (f != 0) {
				f = Mathf.Clamp (f - Time.deltaTime / duration, 0.0f, 1.0f);
				//Player 1 Controls

				//Text 
				for (int l = 0; l < textRenderers.Count; l++) {
					textRenderers [l].material.color = Color.Lerp (textColorsEmpty [l], textColorsFull [l], f);
					textRenderers [l].GetComponent<ClusterNodeColorSpecific> ().lit = false;
				}

			}
		}
	}

	void Update()
	{
		if (!colorsSet)
			SetTextColors ();
	}

	private void SetTextColors()
	{
		int colorSetCount = 0;

		foreach (Renderer renderer in textRenderers)
		{
			if(renderer.GetComponent<ClusterNodeColorSpecific>().colorSet)
			{
				renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 1.0f);
				textColorsEmpty.Add(renderer.material.color);
				textColorsFull.Add(new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 1.0f));
				colorSetCount++;
			}
		}
		
		if (colorSetCount == textRenderers.Count)
			colorsSet = true;


	}
	
	
}
