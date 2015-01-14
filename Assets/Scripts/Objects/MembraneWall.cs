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
	[SerializeField]
	public List<int> shapingIndices;
	private float ShapedDistance
	{
		get
		{
			float shapedDistance = 0;
			Membrane createdMembrane = membraneCreator.createdBond as Membrane;
			Vector3 startPos = createdMembrane.attachment1.position;
			for (int i = 0; i < shapingIndices.Count; i++)
			{
				shapedDistance += (createdMembrane.shapingPoints[shapingIndices[i] + 2].transform.position - startPos).magnitude;
				startPos = createdMembrane.shapingPoints[shapingIndices[i] + 2].transform.position;
			}
			shapedDistance += (createdMembrane.attachment2.position - startPos).magnitude;
			//TODO figure out how to make this work for the ends.
			return shapedDistance;
		}
	}
	[Header("Starting Distance Factors")]
	public float relativeMaxDistance = -1;
	[Header("Live Values")]
	public float shapedDistance; // TODO remove
	public float currentLength;
	public float relativeActualDistance;
	
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

		if (shapingIndices.Count != shapingPoints.Count)
		{
			Debug.LogError("Membrane wall has incorrect number of shaping indices. Ensure that shaping point count and shaping index count are equal.");
		}
	}

	void Start()
	{
		if (createOnStart)
		{
			CreateWall();
		}
	}

	void Update()
	{
		if (membraneCreator != null && membraneCreator.createdBond != null)
		{
			currentLength = membraneCreator.createdBond.BondLength;
			//relativeActualDistance = currentLength / membraneLength;
			shapedDistance = ShapedDistance;
			relativeActualDistance = currentLength / shapedDistance;
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
		if (membraneCreator.createdBond != null)
		{
			membraneCreator.createdBond.transform.parent = transform;
		}
	}
}
