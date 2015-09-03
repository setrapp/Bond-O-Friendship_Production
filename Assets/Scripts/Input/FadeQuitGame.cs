using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FadeQuitGame : MonoBehaviour {

	public Renderer prompt;
	private Color promptFull;
	private Color promptEmpty;

	public List<Renderer> textRenderers;
	private List<Color> textColorsEmpty = new List<Color>();
	private List<Color> textColorsFull = new List<Color>();

	public float f = 0.0f;
	public float duration = 1.0f;

	private bool colorsSet = false;

	// Use this for initialization
	void Start () 
	{
		prompt.material.color = new Color (prompt.material.color.r, prompt.material.color.g, prompt.material.color.b, 0.0f);
		promptEmpty =  prompt.material.color;
		promptFull = new Color (prompt.material.color.r, prompt.material.color.g, prompt.material.color.b, 1.0f);	

		foreach (Renderer renderer in textRenderers) 
		{
			renderer.material.color = new Color (renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 0.0f);
			textColorsEmpty.Add (renderer.material.color);
			textColorsFull.Add (new Color (renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 1.0f));
		}
	}

	public void FadeIn()
	{
		if (f != 1) 
		{
			f = Mathf.Clamp (f + Time.deltaTime / duration, 0.0f, 1.0f);
			//Text Prompt
			prompt.material.color = Color.Lerp(promptEmpty, promptFull, f);
			//Text Interactable
			for (int i = 0; i < textRenderers.Count; i++) 
			{
				textRenderers [i].material.color = Color.Lerp (textColorsEmpty [i], textColorsFull [i], f);
			}

		}
	}

	public void FadeOut()
	{
		if (f != 0) 
		{
			f = Mathf.Clamp (f - Time.deltaTime / duration, 0.0f, 1.0f);
			//Text Prompt
			prompt.material.color = Color.Lerp(promptEmpty, promptFull, f);
			//Interactable Text 
			for (int l = 0; l < textRenderers.Count; l++) 
			{
				textRenderers [l].material.color = Color.Lerp (textColorsEmpty [l], textColorsFull [l], f);
				textRenderers[l].GetComponent<ClusterNode>().lit = false;
			}

		}
	}



	
	
}
