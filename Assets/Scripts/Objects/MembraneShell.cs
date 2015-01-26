using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MembraneShell : MonoBehaviour {
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
			MembraneWall[] existingWalls = GetComponentsInChildren<MembraneWall>();
			for (int i = 0; i < existingWalls.Length; i++)
			{
				existingWalls[i].createOnStart = false;
				existingWalls[i].wallIsCentered = true;
				createdWalls.Add(existingWalls[i]);
			}

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
}
