using UnityEngine;
using System.Collections;

public class ManageSeasons : MonoBehaviour {

	public GameObject icePrefab;
	public GameObject petalsPrefab;
	public ParticleSystem rain;
	public ParticleSystem rain2;
	public ParticleSystem rain3;
	public Light rainLight;
	public GameObject leaf1Prefab;
	public GameObject leaf2Prefab;
	public GameObject leaf3Prefab;
	public GameObject leaf4Prefab;
	public GameObject leaf5Prefab;
	public GameObject leaf6Prefab;
	public GameObject leaf7Prefab;
	public int season;
	public float seasonLength = 30.0f;
	public float leafSpawnRange = 30.0f;
	public float leafDensity = 0.5f;
	public Island island;
	public Color fallBackground;
	public Color winterBackground;
	public Color springBackground;
	public Color summerBackground;
	private Color[] seasonBackgrounds;

	public float seasonTimeRemaining;

	void Awake()
	{
		seasonBackgrounds = new Color[4];
		seasonBackgrounds[0] = fallBackground;
		seasonBackgrounds[1] = winterBackground;
		seasonBackgrounds[2] = springBackground;
		seasonBackgrounds[3] = summerBackground;
		if (CameraColorFade.Instance != null)
		{
			CameraColorFade.Instance.FadeToColor(seasonBackgrounds[season]);
		}
		if (island != null)
		{
			island.backgroundColor = seasonBackgrounds[season];
		}
	}

	public void Start () {
		seasonTimeRemaining = seasonLength;
		
	}

	public void Update() {
		seasonTimeRemaining -= Time.deltaTime;
		if(seasonTimeRemaining <= 0)
		{
			season++;
			if(season == 4)
				season = 0;
			seasonTimeRemaining = seasonLength;
			if (CameraColorFade.Instance != null)
			{
				CameraColorFade.Instance.FadeToColor(seasonBackgrounds[season]);
			}
		}

		if(season == 3)
		{
			rain.enableEmission = true;
			rain2.enableEmission = true;
			rain3.enableEmission = true;
			if(rainLight.intensity > 0.36)
				rainLight.intensity -= Time.deltaTime * 0.001f;
		}
		else
		{
			rain.enableEmission = false;
			rain2.enableEmission = false;
			rain3.enableEmission = false;
			if(rainLight.intensity < 0.49)
				rainLight.intensity += Time.deltaTime * 0.001f;
		}
	}

}
