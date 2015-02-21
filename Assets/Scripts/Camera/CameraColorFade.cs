using UnityEngine;
using System.Collections;

public class CameraColorFade : MonoBehaviour {

	private Color startingColor;
	private Color currentColor;
	public Color fadeColor;
	public float fadeSpeed;
	public bool fading;

	// Use this for initialization
	void Start () {
		fading = true;
		startingColor = Camera.main.backgroundColor;
	}
	
	// Update is called once per frame
	void Update () {
		currentColor = Camera.main.backgroundColor;
		if(fading == true)
			Camera.main.backgroundColor = Color.Lerp(currentColor, fadeColor, Time.deltaTime*fadeSpeed);
		else
			Camera.main.backgroundColor = Color.Lerp(currentColor, startingColor, Time.deltaTime*fadeSpeed);
	}
}
