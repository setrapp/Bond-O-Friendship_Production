using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FadeMainMenu : MonoBehaviour {

	public List<Renderer> player1ControlsRenderers;
	private List<Color> player1ControlsColorsEmpty = new List<Color>();
	private List<Color> player1ControlsColorsFull = new List<Color>();

	public List<Renderer> player2ControlsRenderers;
	private List<Color> player2ControlsColorsEmpty = new List<Color>();
	private List<Color> player2ControlsColorsFull = new List<Color>();

	public List<Renderer> playersSharedControlsRenderers;
	private List<Color> playersSharedControlsColorsEmpty = new List<Color>();
	private List<Color> playersSharedControlsColorsFull = new List<Color>();

	public List<Renderer> textRenderers;
	private List<Color> textColorsEmpty = new List<Color>();
	private List<Color> textColorsFull = new List<Color>();

	public float f = 0.0f;
	public float duration = 1.0f;

	private bool colorsSet = false;

	// Use this for initialization
	void Start () 
	{
		foreach (Renderer renderer in player1ControlsRenderers)
		{
			renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 0.0f);
			player1ControlsColorsEmpty.Add(renderer.material.color);
			player1ControlsColorsFull.Add(new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 1.0f));
		}
		foreach (Renderer renderer in player2ControlsRenderers)
		{
			renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 0.0f);
			player2ControlsColorsEmpty.Add(renderer.material.color);
			player2ControlsColorsFull.Add(new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 1.0f));
		}
		foreach (Renderer renderer in playersSharedControlsRenderers)
		{
			renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 0.0f);
			playersSharedControlsColorsEmpty.Add(renderer.material.color);
			playersSharedControlsColorsFull.Add(new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 1.0f));
		}
		//foreach (Renderer renderer in textRenderers)
		//{
		//	renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 0.0f);
		//	textColorsEmpty.Add(renderer.material.color);
		//	textColorsFull.Add(new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 1.0f));
		//}
		
	}

	public void FadeIn()
	{
		if (f != 1) 
		{
			f = Mathf.Clamp (f + Time.deltaTime / duration, 0.0f, 1.0f);
			//Player 1 Controls
			for (int i = 0; i < player1ControlsRenderers.Count; i++) 
			{
			    player1ControlsRenderers [i].material.color = Color.Lerp (player1ControlsColorsEmpty [i], player1ControlsColorsFull [i], f);
			}
			//Player 2 Controls
			for (int i = 0; i < player2ControlsRenderers.Count; i++) 
			{			
				player2ControlsRenderers [i].material.color = Color.Lerp (player2ControlsColorsEmpty [i], player2ControlsColorsFull [i], f);
			}
			//Shared Controls
			for (int i = 0; i < playersSharedControlsRenderers.Count; i++) {

				playersSharedControlsRenderers [i].material.color = Color.Lerp (playersSharedControlsColorsEmpty [i], playersSharedControlsColorsFull [i], f);
			}
			//Text
			for (int i = 0; i < textRenderers.Count; i++) 
			{
				textRenderers [i].material.color = Color.Lerp (textColorsEmpty [i], textColorsFull [i], f);
			}

		}
	}

	public void FadeOut()
	{
		if (f != 0) {
			f = Mathf.Clamp (f - Time.deltaTime / duration, 0.0f, 1.0f);
			//Player 1 Controls
			for (int i = 0; i < player1ControlsRenderers.Count; i++)
			{
					player1ControlsRenderers [i].material.color = Color.Lerp (player1ControlsColorsEmpty [i], player1ControlsColorsFull [i], f);
			}
			//Player 2 Controls
			for (int i = 0; i < player2ControlsRenderers.Count; i++) 
			{
					player2ControlsRenderers [i].material.color = Color.Lerp (player2ControlsColorsEmpty [i], player2ControlsColorsFull [i], f);
			}
			//Player Shared Controls
			for (int i = 0; i < playersSharedControlsRenderers.Count; i++) 
			{
					playersSharedControlsRenderers [i].GetComponent<Renderer> ().material.color = Color.Lerp (playersSharedControlsColorsEmpty [i], playersSharedControlsColorsFull [i], f);
			}
			//Text 
			for (int l = 0; l < textRenderers.Count; l++) 
			{
					textRenderers [l].material.color = Color.Lerp (textColorsEmpty [l], textColorsFull [l], f);
				textRenderers[l].GetComponent<ClusterNodeColorSpecific>().lit = false;
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
				renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 0.0f);
				textColorsEmpty.Add(renderer.material.color);
				textColorsFull.Add(new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 1.0f));
				colorSetCount++;
			}
		}
		
		if (colorSetCount == textRenderers.Count)
			colorsSet = true;


	}
	
	
}
