using UnityEngine;
using System.Collections;

public class SplitDestroy : MonoBehaviour {

	public GameObject splitDestroyee;
	public GameObject splitCork;
	public float splitDistance;
	public bool shrinkBeforeDestroy = false;
	private Vector3 staringScale;
	public Vector3 endingScale = Vector3.zero;

	void Start()
	{
		if (splitDestroyee == null)
		{
			splitDestroyee = gameObject;
		}
		staringScale = splitDestroyee.transform.localScale;
	}

	void Update()
	{
		if (splitDestroyee == null)
		{
			return;
		}

		if (splitDistance <= 0)
		{
			Destroy(splitDestroyee);
			return;
		}

		Vector3 fromCork = splitDestroyee.transform.position - splitCork.transform.position;
		fromCork.z = 0;

		if (fromCork.sqrMagnitude > 0 || splitDestroyee.transform.localScale != staringScale)
		{
			float corkDist = fromCork.magnitude;

			float progress = Mathf.Clamp01(corkDist / splitDistance);
			splitDestroyee.transform.localScale = (staringScale * (1 - progress)) + (endingScale * progress);

			if (progress >= 1)
			{
				Destroy(splitDestroyee);
				splitDestroyee = null;
			}
		}
	}
}
