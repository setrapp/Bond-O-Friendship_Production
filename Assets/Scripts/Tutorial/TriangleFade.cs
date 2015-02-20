using UnityEngine;
using System.Collections;

public class TriangleFade : MonoBehaviour {

	private float timer;
	private Color myColor;
	public GameObject waitPad;
	private WaitPad pad;
	private Renderer targetRenderer;

	void Start()
	{
		if (waitPad != null)
		{
			pad = waitPad.GetComponent<WaitPad>();
			
		}
		targetRenderer = GetComponent<Renderer>();
		timer = 0.7f;

	}

	void Update () {
		if(pad != null && pad.activated)
		{
			myColor = new Color(0.6f, 0.6f, 0.8f, timer);
			if (targetRenderer != null)
			{
				targetRenderer.material.color = myColor;
			}
			timer -= Time.deltaTime;
		}
		if(timer <= 0)
		{
			Destroy(gameObject);
		}
		
	}

}
