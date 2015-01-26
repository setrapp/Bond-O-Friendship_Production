using UnityEngine;
using System.Collections;

public class TextureSeasons : MonoBehaviour {

	public Material summer;
	public Material summer2;
	public Material summer3;
	public Material summerWall;
	public Material fall;
	public Material fall2;
	public Material fall3;
	public Material fallWall;
	public Material winter;
	public Material winter2;
	public Material winter3;
	public Material winterWall;
	public Material spring;
	public Material spring2;
	public Material spring3;
	public Material springWall;

	private int season;
	private float changeSpeed = 0.01f;
	private Renderer seasonRenderer;

	// Use this for initialization
	void Start () {
		seasonRenderer = GetComponent<Renderer>();

	}
	
	// Update is called once per frame
	void Update () {
		season = GameObject.Find ("Seasons Manager").GetComponent<ManageSeasons> ().season;
		switch(season)
		{
		case 0:
			if(name == "Season")
				seasonRenderer.material.color = Color.Lerp(seasonRenderer.material.color, fall.color, changeSpeed);
			if(name == "Season2")
				seasonRenderer.material.color = Color.Lerp(seasonRenderer.material.color, fall2.color, changeSpeed);
			if(name == "Season3")
				seasonRenderer.material.color = Color.Lerp(seasonRenderer.material.color, fall3.color, changeSpeed);
			if(name == "Wall")
				seasonRenderer.material.color = Color.Lerp(seasonRenderer.material.color, fallWall.color, changeSpeed);
			break;
		case 1:
			if(name == "Season")
				seasonRenderer.material.color = Color.Lerp(seasonRenderer.material.color, winter.color, changeSpeed);
			if(name == "Season2")
				seasonRenderer.material.color = Color.Lerp(seasonRenderer.material.color, winter2.color, changeSpeed);
			if(name == "Season3")
				seasonRenderer.material.color = Color.Lerp(seasonRenderer.material.color, winter3.color, changeSpeed);
			if(name == "Wall")
				seasonRenderer.material.color = Color.Lerp(seasonRenderer.material.color, winterWall.color, changeSpeed);
			break;
		case 2:
			if(name == "Season")
				seasonRenderer.material.color = Color.Lerp(seasonRenderer.material.color, spring.color, changeSpeed);
			if(name == "Season2")
				seasonRenderer.material.color = Color.Lerp(seasonRenderer.material.color, spring2.color, changeSpeed);
			if(name == "Season3")
				seasonRenderer.material.color = Color.Lerp(seasonRenderer.material.color, spring3.color, changeSpeed);
			if(name == "Wall")
				seasonRenderer.material.color = Color.Lerp(seasonRenderer.material.color, springWall.color, changeSpeed);
			break;
		case 3:
			if(name == "Season")
				seasonRenderer.material.color = Color.Lerp(seasonRenderer.material.color, summer.color, changeSpeed);
			if(name == "Season2")
				seasonRenderer.material.color = Color.Lerp(seasonRenderer.material.color, summer2.color, changeSpeed);
			if(name == "Season3")
				seasonRenderer.material.color = Color.Lerp(seasonRenderer.material.color, summer3.color, changeSpeed);
			if(name == "Wall")
				seasonRenderer.material.color = Color.Lerp(seasonRenderer.material.color, summerWall.color, changeSpeed);
			break;
		}

	}
}
