using UnityEngine;
using System.Collections;

public class PointGroups : MonoBehaviour {

	public bool fading = false;
	public bool bright = false;

	private float myAlpha;
	private float fadeConst = 0.2f;

	public GameObject PointsGlobal;


	// Use this for initialization
	void Start () {

		PointsGlobal = GameObject.FindGameObjectWithTag("Global Points");

		transform.parent = PointsGlobal.transform;
	
	}
	
	// Update is called once per frame
	void Update () {

		if(fading == true)
		{
			BroadcastMessage("IsFading",SendMessageOptions.DontRequireReceiver);
		}

		if(bright == true)
		{
			BroadcastMessage("IsBright",SendMessageOptions.DontRequireReceiver);
		}

	}

	public void IsFading()
	{
		fading = true;
		bright = false;
		print ("is fading");
	}

	public void IsBright()
	{
		fading = false;
		bright = true;
	}
}
