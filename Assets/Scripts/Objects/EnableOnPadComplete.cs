using UnityEngine;
using System.Collections;

public class EnableOnPadComplete : MonoBehaviour {

	public WaitPad triggerPad;
	public GameObject targetObject;
	public bool toEnabled = true;
	public bool startOpposite = true;
	private bool actionCompleted = false;
	
	void Awake()
	{
		if (triggerPad == null)
		{
			triggerPad = GetComponent<WaitPad>();
		}
		if (targetObject != null && startOpposite)
		{
			targetObject.SetActive(!toEnabled);
		}
	}

	void Update()
	{
		if (triggerPad != null && targetObject != null && triggerPad.activated && !actionCompleted)
		{
			targetObject.SetActive(toEnabled);
			actionCompleted = true;
		}
	}
}
