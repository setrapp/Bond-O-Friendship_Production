using UnityEngine;
using System.Collections;

public class BudFadeOut : MonoBehaviour
{
	public Renderer budRenderer;
	private float timer;
	public GameObject myShadow;
	public Renderer shadowRenderer;
	public bool fadeNow = false;

	// Use this for initialization
	void Start()
	{
		if (budRenderer == null)
		{
			budRenderer = GetComponent<Renderer>();
		}
		if (myShadow != null && shadowRenderer == null)
		{
			shadowRenderer = GetComponent<Renderer>();
		}

		timer = 1.0f;

	}

    // Update is called once per frame
    void Update()
    {
		if (fadeNow)
		{
			if (budRenderer != null)
			{
				Color myColor = budRenderer.material.color;
				myColor.a = timer;
				budRenderer.material.color = myColor;
			}
			if (shadowRenderer != null)
			{
				Color shadowColor = shadowRenderer.material.color;
				shadowColor.a = timer;
				shadowRenderer.material.color = shadowColor;
			}

			//timer -= Time.deltaTime;
			if (timer <= 0)
			{
				Destroy(gameObject);
			}
		}
    }
}
