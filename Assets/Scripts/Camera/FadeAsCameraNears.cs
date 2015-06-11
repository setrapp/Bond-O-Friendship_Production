using UnityEngine;
using System.Collections;

public class FadeAsCameraNears : MonoBehaviour {


	private CameraSplitter cameraHolder;

	private float distance = 0.0f;
	private float alphaValue = 0.0f;

	private Color objectColor;


	// Use this for initialization
	void Start () 
	{
		cameraHolder = CameraSplitter.Instance;
		objectColor = GetComponent<Renderer> ().material.color;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (cameraHolder == null)
			cameraHolder = CameraSplitter.Instance;

		distance = Mathf.Abs (transform.position.z - cameraHolder.transform.position.z);

		if (distance <= 100.0f) {
			distance -= 50.0f;
			alphaValue = distance / 50.0f;

			objectColor.a = alphaValue;
		}

		if (GetComponent<Renderer> ().material.color.a != objectColor.a) 
		{
			GetComponent<Renderer> ().material.color = objectColor;
		}
	
	}
}
