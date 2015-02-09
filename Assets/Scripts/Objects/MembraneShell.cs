using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MembraneShell : MonoBehaviour {
	public bool destroyWhenBroken = true;
	[SerializeField]
	public List<MembraneWall> createdWalls;
	public GameObject membraneWallOriginal;
	public bool createOnStart = true;
	public int wallCount = 4;
	

	void Start()
	{
		if (createOnStart)
		{
			CreateShell();
		}
	}

	public void CreateShell()
	{
		if (wallCount < 3)
		{
			wallCount = 3;
		}

		if (createdWalls.Count < wallCount)
		{
			membraneWallOriginal.SetActive(false);

			for (int i = createdWalls.Count; i < wallCount; i++)
			{
				GameObject newWallObject = (GameObject)(Instantiate(membraneWallOriginal));
				MembraneWall newWall = newWallObject.GetComponent<MembraneWall>();
				if (newWall != null)
				{
					newWall.createOnStart = false;
					newWall.wallIsCentered = true;
					createdWalls.Add(newWall);
				}
				newWallObject.transform.parent = transform;
				newWallObject.SetActive(true);
			}

			float angleStep = -360 / wallCount;
			for (int i = 0; i < createdWalls.Count; i++)
			{
				Vector3 toCenterDir = Quaternion.Euler(0, 0, angleStep * i) * Vector3.up;
				float toCenterMag = (createdWalls[i].membraneLength / 2) / Mathf.Tan(angleStep * -0.5f * Mathf.Deg2Rad);
				createdWalls[i].transform.position = transform.position + toCenterDir * toCenterMag;
				createdWalls[i].membraneDirection = Vector3.Cross(toCenterDir, Vector3.forward);
				createdWalls[i].membraneCreator.neighborPrevious = createdWalls[(i > 0) ? i - 1 : createdWalls.Count - 1].membraneCreator;
				createdWalls[i].membraneCreator.neighborNext = createdWalls[(i < createdWalls.Count - 1) ? i + 1 : 0].membraneCreator;
			}

			for (int i = 0; i < createdWalls.Count; i++)
			{
				createdWalls[i].CreateWall();
			}
		}
	}

	public void SilentBreak()
	{
		while (createdWalls.Count > 0)
		{
			MembraneWall removeMembrane = createdWalls[0];
			if (removeMembrane.destroyWhenBroken)
			{
				Destroy(removeMembrane.gameObject);
			}
			Transform parent = transform.parent;
			transform.parent = null;
			MembraneBroken(removeMembrane);
			transform.parent = parent;
		}
	}

	private void MembraneBroken(MembraneWall brokenMembrane)
	{
		if (createdWalls.Count > 0)
		{
			createdWalls.Remove(brokenMembrane);

			if (createdWalls.Count == 0)
			{
				if (transform.parent != null)
				{
					transform.parent.SendMessage("MembraneBroken", this, SendMessageOptions.DontRequireReceiver);
				}
				if (destroyWhenBroken)
				{
					Destroy(gameObject);
				}
			}
		}
	}
}
