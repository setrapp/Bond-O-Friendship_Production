using UnityEngine;
using System.Collections;

public class Luminus : MonoBehaviour {

	public JoinTogetherGroup triggerJoinGroup;
	//public GameObject ring;
	private GameObject player1;
	private GameObject player2;
	private float player1Dist;
	private float player2Dist;
	private float maxDist;

	public bool isOn;
	public bool stayOn = false;
	public float onDelay = 0.5f;
	public float maxOffIntensity = 0.5f;
	public float maxIntensity = 1;
	public float pureIntensity = 0;
	public float intensity = 0;
	public float fadeTime = 1;
	public float fadePortion = 1;
	public bool fadingIn = true;
    public AudioSource turnOnSound;

	// Use this for initialization
	void Start () {
		maxDist = 25.0f;
		player1 = GameObject.Find("Player 1");
		player2 = GameObject.Find("Player 2");
	
	}
	
	// Update is called once per frame
	void Update (){

		if (!isOn && triggerJoinGroup != null && triggerJoinGroup.solved)
		{
			isOn = true;
			turnOnSound.Play();
		}

		if (!isOn && triggerJoinGroup != null)
		{
			float totalProgress = 0;
			for (int i = 0; i < triggerJoinGroup.joins.Count; i++)
			{
				totalProgress += triggerJoinGroup.joins[i].progress;
			}
			totalProgress /= triggerJoinGroup.joins.Count;
			pureIntensity = totalProgress * maxOffIntensity;
		}// todofade in and out based on player distance to other luminus, scale up to maxOffIntensity (nonlinear?) and then expand to actual max on completion.

		if (isOn && pureIntensity < maxIntensity)
		{
			if (onDelay > 0)
			{
				pureIntensity += (maxIntensity * Time.deltaTime) / onDelay;
			}
			else
			{
				pureIntensity = maxIntensity;
			}

		}	

		// Fade intensity.
		if (fadingIn && fadePortion < 1)
		{
			if (fadeTime > 0)
			{
				fadePortion += Time.deltaTime / fadeTime;
			}
			else
			{
				fadePortion = 1;
			}
		}
		else if (!fadingIn && fadePortion > 0)
		{
			if (fadeTime > 0)
			{
				fadePortion -= Time.deltaTime / fadeTime;
			}
			else
			{
				fadePortion = 0;
			}
		}

		float intensityLimit = isOn ? maxIntensity : maxOffIntensity;

		intensity = Mathf.Clamp(pureIntensity * fadePortion, 0, intensityLimit);
	}
}
