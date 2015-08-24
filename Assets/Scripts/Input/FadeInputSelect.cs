using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FadeInputSelect : MonoBehaviour {

	public List<Renderer> inputSelectRenderers;
	private List<Color> inputSelectColorsEmpty = new List<Color>();
	private List<Color> inputSelectColorsFull = new List<Color>();


	public float f = 0.0f;
	public float duration = 2.0f;
	public bool fadeOut;
	public bool fadeIn;

	// Use this for initialization
	void Awake () 
	{
		foreach (Renderer renderer in inputSelectRenderers)
		{

			renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 0.0f);
			inputSelectColorsEmpty.Add(renderer.material.color);
			inputSelectColorsFull.Add(new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 1.0f));
		}
	
	}
	
	public void FadeIn()
	{	if (f != 1) 
	{
			f = Mathf.Clamp (f + Time.deltaTime / duration, 0.0f, 1.0f);
			for (int i = 0; i < inputSelectRenderers.Count; i++) {		
				inputSelectRenderers [i].material.color = Color.Lerp (inputSelectColorsEmpty [i], inputSelectColorsFull [i], f);
			}
			
		}
	}
	
	public void FadeOut()
	{
		if (f != 0) {
			f = Mathf.Clamp (f - Time.deltaTime / duration, 0.0f, 1.0f);
			for (int i = 0; i < inputSelectRenderers.Count; i++) {				
				inputSelectRenderers [i].material.color = Color.Lerp (inputSelectColorsEmpty [i], inputSelectColorsFull [i], f);
			}
			
		} 
	}

}
