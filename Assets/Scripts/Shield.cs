using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {

	public Color myColor;
	public float alphaFloat;
	public float alphaSpeed = 5.0f;
	public bool isActivated = false;

	// Use this for initialization
	void Start () {
		alphaFloat = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		myColor = new Color(0.25f,0.05f,0.8f,alphaFloat);
		renderer.material.color = myColor;
		if(isActivated == true)
		{
			//alphaFloat = 0.5f;
			if(alphaFloat < 0.5f)
			{
				alphaFloat += Time.deltaTime*alphaSpeed;
			}
		}
		else if(isActivated == false)
		{
			//alphaFloat = 0.0f;
			if(alphaFloat > 0.0f)
			{
				alphaFloat -= Time.deltaTime*alphaSpeed;
			}

		}
	}

	void Activate()
	{
		isActivated = true;
		//print("activated");
	}
	void DeActivate()
	{
		isActivated = false;
		//print("deactivated");
	}
}
