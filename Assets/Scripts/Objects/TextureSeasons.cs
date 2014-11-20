﻿using UnityEngine;
using System.Collections;

public class TextureSeasons : MonoBehaviour {

	public Material summer;
	public Material summer2;
	public Material summer3;
	public Material fall;
	public Material fall2;
	public Material fall3;
	public Material winter;
	public Material winter2;
	public Material winter3;
	public Material spring;
	public Material spring2;
	public Material spring3;

	private float timer = 10.0f;
	private float changeSpeed = 0.01f;
	private int season;
	private Renderer seasonRenderer;

	// Use this for initialization
	void Start () {
		seasonRenderer = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if(timer <= 0)
		{
			season++;
			if(season == 4)
				season = 0;
			timer = 10.0f;
		}
		switch(season)
		{
		case 0:
			if(name == "Season")
				seasonRenderer.material.color = Color.Lerp(seasonRenderer.material.color, fall.color, changeSpeed);
			if(name == "Season2")
				seasonRenderer.material.color = Color.Lerp(seasonRenderer.material.color, fall2.color, changeSpeed);
			if(name == "Season3")
				seasonRenderer.material.color = Color.Lerp(seasonRenderer.material.color, fall3.color, changeSpeed);
			break;
		case 1:
			if(name == "Season")
				seasonRenderer.material.color = Color.Lerp(seasonRenderer.material.color, winter.color, changeSpeed);
			if(name == "Season2")
				seasonRenderer.material.color = Color.Lerp(seasonRenderer.material.color, winter2.color, changeSpeed);
			if(name == "Season3")
				seasonRenderer.material.color = Color.Lerp(seasonRenderer.material.color, winter3.color, changeSpeed);
			break;
		case 2:
			if(name == "Season")
				seasonRenderer.material.color = Color.Lerp(seasonRenderer.material.color, winter.color, changeSpeed);
			if(name == "Season2")
				seasonRenderer.material.color = Color.Lerp(seasonRenderer.material.color, winter2.color, changeSpeed);
			if(name == "Season3")
				seasonRenderer.material.color = Color.Lerp(seasonRenderer.material.color, winter3.color, changeSpeed);
			break;
		case 3:
			if(name == "Season")
				seasonRenderer.material.color = Color.Lerp(seasonRenderer.material.color, fall.color, changeSpeed);
			if(name == "Season2")
				seasonRenderer.material.color = Color.Lerp(seasonRenderer.material.color, fall2.color, changeSpeed);
			if(name == "Season3")
				seasonRenderer.material.color = Color.Lerp(seasonRenderer.material.color, fall3.color, changeSpeed);
			break;
		}

	}
}
