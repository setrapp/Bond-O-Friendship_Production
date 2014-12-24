using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MembraneWall : MonoBehaviour {
	public AutoMembrane membraneCreator;
	public bool createOnStart = true;
	public bool wallIsCentered = true;
	public Vector3 membraneDirection;
	public float membraneLength;
	public float defaultShapingForce = -1;
	public GameObject shapingPointPrefab;
	[SerializeField]
	public List<ShapingPointStats> shapingPoints;
	[Header("Starting Distance Factors")]
	public float relativeMaxDistance = -1;
	
	void Awake()
	{
		if (membraneCreator == null)
		{
			membraneCreator = GetComponent<AutoMembrane>();
		}

		if (membraneCreator != null)
		{
			membraneCreator.createOnStart = false;
		}
	}

	void Start()
	{
		if (createOnStart)
		{
			CreateWall();
		}
	}

	public void CreateWall()
	{
		if (membraneCreator == null || membraneCreator.attachable1 == null || membraneCreator.attachable2 == null)
		{
			return;
		}

		// Update override stats to account for starting distance between endpoints.
		membraneDirection.Normalize();
		Vector3 membraneTrack = membraneDirection * membraneLength;
		if (relativeMaxDistance >= 0)
		{
			membraneCreator.bondOverrideStats.stats.maxDistance = membraneLength * relativeMaxDistance;
		}


		// Set end positions, allowing for either originating from this position, or centering on it.
		Vector3 startPos = transform.position;
		Vector3 endPos = transform.position + membraneTrack;
		if (wallIsCentered)
		{
			startPos -= membraneTrack / 2;
			endPos -= membraneTrack / 2;
		}

		// Place endpoints.
		membraneCreator.attachable1.transform.position = startPos;
		membraneCreator.attachable2.transform.position = endPos;

		// Break the membrane track into eigen vectors.
		Vector3 parallel = membraneTrack;
		Vector3 perpendicular = Vector3.Cross(Vector3.forward, membraneTrack);

		// Create shaping points and place them relative to the membrane track.
		if (membraneCreator.shapingPointContainer != null)
		{
			while(shapingPoints.Count > 0)
			{
				GameObject newShapingObject = (GameObject)Instantiate(shapingPointPrefab);

				// Position shaping point relative to membrane track.
				newShapingObject.transform.position = startPos + (parallel * shapingPoints[0].position.y) + (perpendicular * shapingPoints[0].position.x);
				
				// Populate shaping point values, fallback to defaults when necessary.
				ShapingPoint newShapingPoint = newShapingObject.GetComponent<ShapingPoint>();
				if (newShapingPoint != null)
				{
					if (shapingPoints[0].shapingForce < 0)
					{
						shapingPoints[0].shapingForce = defaultShapingForce;
					}
					newShapingPoint.shapingForce = shapingPoints[0].shapingForce;
				}

				if (shapingPoints[0].pointName != null && shapingPoints[0].pointName != "")
				{
					newShapingObject.name = shapingPoints[0].pointName;
				}

				newShapingObject.transform.parent = membraneCreator.shapingPointContainer.transform;
				shapingPoints.RemoveAt(0);
			}
		}

		// Create membrane.
		membraneCreator.CreateBond();
	}
}
