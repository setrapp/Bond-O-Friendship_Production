using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraColorFade : MonoBehaviour {
	private static CameraColorFade instance = null;
	public static CameraColorFade Instance
	{
		get
		{
			if (instance == null)
			{
				instance = GameObject.FindGameObjectWithTag("CameraSystem").GetComponent<CameraColorFade>();
			}
			return instance;
		}
	}
	[SerializeField]
	public List<Camera> cameras;
	//private Color startingColor;
	private Color currentColor;
	public Color fadeColor;
	public float fadeSpeed;
	public bool fading;


	void Start () {
		fading = true;
		for (int i = 0; i < cameras.Count; i++)
		{
			if (cameras[i].clearFlags == CameraClearFlags.Depth || cameras[i].clearFlags == CameraClearFlags.Nothing)
			{
				cameras.RemoveAt(i);
				i--;
			}
		}
		if (cameras.Count > 0)
		{
			currentColor = cameras[0].backgroundColor;
		}
	}

	void Update () {
		if (fading)
		{
			if (fadeSpeed <= 0)
			{
				JumpToColor(fadeColor);
				return;
			}

			currentColor = Color.Lerp(currentColor, fadeColor, Time.deltaTime / fadeSpeed);
			for (int i = 0; i < cameras.Count; i++)
			{
				cameras[i].backgroundColor = currentColor;
			}
			if (cameras.Count > 0 && cameras[0].backgroundColor == fadeColor)
			{
				fading = false;
			}
		}
	}

	public void FadeToColor(Color newColor)
	{
		fadeColor = newColor;
		fading = true;
	}

	public void JumpToColor(Color newColor)
	{
		fadeColor = newColor;
		currentColor = fadeColor;
		for (int i = 0; i < cameras.Count; i++)
		{
			cameras[i].backgroundColor = currentColor;
		}
		fading = false;
	}
}
