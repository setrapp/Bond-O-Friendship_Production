using UnityEngine;
using System.Collections;

public class TriangleFade : MonoBehaviour {

	private float timer;
	private Color myColor;
	public GameObject waitPad;
	public GameObject waitPad2;
	public GameObject waitPad3;
	private WaitPad pad;
	private WaitPad pad2;
	private WaitPad pad3;
	private Renderer targetRenderer;

	void Start()
	{
		if (waitPad != null)
			pad = waitPad.GetComponent<WaitPad>();
		if (waitPad2 != null)
			pad2 = waitPad2.GetComponent<WaitPad>();
		if (waitPad3 != null)
			pad3 = waitPad3.GetComponent<WaitPad>();

		targetRenderer = GetComponent<Renderer>();
		timer = 0.7f;

	}

	void Update () {
		if((pad != null && pad.activated) || (pad2 != null && pad2.activated) || (pad3 != null && pad3.activated))
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
