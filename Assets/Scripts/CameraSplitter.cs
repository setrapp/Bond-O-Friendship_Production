using UnityEngine;
using System.Collections;

public class CameraSplitter : MonoBehaviour {

	public bool split = false;
	private bool wasSplit = false;
	public float splitDistance;
	public float combineDistance;
	public GameObject combinedCameraSystem;
	public GameObject player1CameraSystem;
	public GameObject player2CameraSystem;

	void Start()
	{
		wasSplit = !split;
		CheckSplit();
	}

	void Update()
	{
		CheckSplit();
	}

	private void CheckSplit()
	{
		//TODO should this go off of player positions?
		if ((player1CameraSystem.transform.position - player2CameraSystem.transform.position).sqrMagnitude < Mathf.Pow(combineDistance, 2))
		{
			split = false;
		}
		else if ((player1CameraSystem.transform.position - player2CameraSystem.transform.position).sqrMagnitude > Mathf.Pow(splitDistance, 2))
		{
			split = true;
		}

		if (split != wasSplit)
		{
			for (int i = 0; i < combinedCameraSystem.transform.childCount; i++)
			{
				combinedCameraSystem.transform.GetChild(i).gameObject.SetActive(!split);
			}
			for (int i = 0; i < player1CameraSystem.transform.childCount; i++)
			{
				player1CameraSystem.transform.GetChild(i).gameObject.SetActive(split);
			}
			for (int i = 0; i < player2CameraSystem.transform.childCount; i++)
			{
				player2CameraSystem.transform.GetChild(i).gameObject.SetActive(split);
			}
		}

		wasSplit = split;
	}
}
