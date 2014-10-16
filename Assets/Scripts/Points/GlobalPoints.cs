using UnityEngine;
using System.Collections;

public class GlobalPoints : MonoBehaviour {

	public GameObject currentPoints = null;
	private int recentPoints;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		recentPoints = transform.childCount - 1;
	
	}

	void EndLeading()
	{
		
		//createdPoints.SendMessage("IsFading",SendMessageOptions.DontRequireReceiver);
		//Invoke("DestroyPoints",5.0f);
		//print ("is uncoupled");
		//currentPoints = null;
	}
	
	void PointsFade()
	{
		if (recentPoints >= 0 && recentPoints < transform.childCount)
		{
			transform.GetChild(recentPoints).gameObject.SendMessage("IsFading", SendMessageOptions.DontRequireReceiver);
		}
	}
	
	void PointsBright()
	{
		if (recentPoints >= 0 && recentPoints < transform.childCount)
		{
			transform.GetChild(recentPoints).gameObject.SendMessage("IsBright", SendMessageOptions.DontRequireReceiver);
		}
	}

	private void DestroyPoints()
	{
		if (recentPoints >= 0 && recentPoints < transform.childCount)
		{
			Destroy(transform.GetChild(recentPoints).gameObject);
		}
	}
}
