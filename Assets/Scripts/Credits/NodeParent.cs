using UnityEngine;
using System.Collections;

public class NodeParent : MonoBehaviour {

	public int childrenCounter;
	public int activeChildren;
	public static int activeParents;
	public Color childZeroColor;
	public FadeToBeContinued fadeTarget = null;
	private bool active;
	
	private void SendFade()
	{
		fadeTarget.gameObject.SetActive(true);
		fadeTarget.StartFade();
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(activeChildren == transform.childCount && active == false)
		{
			activeParents++;
			active = true;
			if(activeParents == transform.parent.childCount)
				SendFade();
		}
	}
}
