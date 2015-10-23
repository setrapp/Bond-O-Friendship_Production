using UnityEngine;
using System.Collections;

public class DisableFarFromPlayers : MonoBehaviour {

	public Transform checkFrom;
	public GameObject disableObject;
	public float disableDistance = 50;

	void Start()
	{
		if (checkFrom == null)
		{
			checkFrom = transform;
		}
	}

	void Update()
	{
		Vector3 toPlayer1 = Globals.Instance.Player1.transform.position - checkFrom.position;
		Vector3 toPlayer2 = Globals.Instance.Player2.transform.position - checkFrom.position;

		float sqrDisableDist = disableDistance * disableDistance;
		if (sqrDisableDist < toPlayer1.sqrMagnitude && sqrDisableDist < toPlayer2.sqrMagnitude)
		{
			if (disableObject.activeSelf)
			{
				disableObject.SetActive(false);
			}
		}
		else
		{
			if (!disableObject.activeSelf)
			{
				disableObject.SetActive(true);
			}
		}
	}
}
