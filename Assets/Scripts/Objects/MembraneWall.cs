using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MembraneWall : MonoBehaviour {
	public AutoMembrane membraneCreator;
	public bool createOnStart = true;
	public bool destroyWhenBroken = true;
	public bool wallIsCentered = true;
	public Vector3 membraneDirection;
	public float membraneLength;
	public bool showPosts = false;
	public GameObject startPost;
	public GameObject endPost;
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

			if (createdMembrane.shapingPoints.Count < 3)
			{
				shapedDistance = (createdMembrane.attachment1.position - createdMembrane.attachment2.position).magnitude;
			}
			else
			{
				Vector3 startPos = createdMembrane.shapingPoints[shapingIndices[0] + 2].transform.position;
				for (int i = 1; i < shapingIndices.Count; i++)
				{
					shapedDistance += (createdMembrane.shapingPoints[shapingIndices[i] + 2].transform.position - startPos).magnitude;
					startPos = createdMembrane.shapingPoints[shapingIndices[i] + 2].transform.position;
				}
				shapedDistance += (createdMembrane.shapingPoints[shapingIndices[shapingIndices.Count - 1] + 2].transform.position - startPos).magnitude;
			}

			return shapedDistance;
		}
	}
	[Header("Breaking Requirements")]
	public int requiredPlayersToBreak = 0;
	[Header("Starting Distance Factors")]
	public float relativeMaxDistance = -1;
	public float relativeRequiredAdd = -1;
	private float requirementDistanceAdd = -1;
	private float preRequirementLength = -1;
	[Header("Live Values")]
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
	}

	void Start()
	{
		if (createOnStart)
		{
			CreateWall();
		}

		startPost.SetActive(showPosts);
		endPost.SetActive(showPosts);
	}

	void Update()
	{
		Membrane createdMembrane = membraneCreator.createdBond as Membrane;
		if (membraneCreator != null && createdMembrane != null)
		{
			if (showPosts)
			{
				createdMembrane.attachment1.attachee.transform.position = startPost.transform.position;
				createdMembrane.attachment2.attachee.transform.position = endPost.transform.position;
			}

			currentLength = createdMembrane.BondLength;
			float shapedDistance = ShapedDistance;
			float maxDistance = shapedDistance * relativeMaxDistance + requirementDistanceAdd;
			relativeActualDistance = currentLength / shapedDistance;

			// Ensure that enough players are attempting to break the membrane.
			bool enoughPlayersBonded = true;
			if (requiredPlayersToBreak > 0)
			{
				int playersBonded = 0;
				if (createdMembrane.IsBondMade(Globals.Instance.player1.character.bondAttachable))
				{
					playersBonded++;
				}
				if (createdMembrane.IsBondMade(Globals.Instance.player2.character.bondAttachable))
				{
					playersBonded++;
				}
				if (playersBonded < requiredPlayersToBreak)
				{
					enoughPlayersBonded = false;
				}
			}

			if (!enoughPlayersBonded)
			{
				maxDistance = -1;
				requirementDistanceAdd = 0;
			}
			else
			{
				// Prevent instant breaking upon meeting requirements by adding extra distance necessary to break.
				if (maxDistance >= 0 && relativeRequiredAdd >= 0)
				{
					float distAddForReq = shapedDistance * relativeRequiredAdd;
					if (requirementDistanceAdd <= 0 && currentLength > maxDistance - distAddForReq)
					{
						maxDistance = currentLength + distAddForReq;
						requirementDistanceAdd = maxDistance - createdMembrane.stats.maxDistance;
						preRequirementLength = currentLength;
					}
					else if (Mathf.Abs(currentLength - preRequirementLength) >= distAddForReq)
					{
						createdMembrane.BreakBond();
					}
				}
			}

			createdMembrane.stats.maxDistance = maxDistance;
		}
	}

	public void CreateWall()
	{
		if (membraneCreator == null || membraneCreator.attachable1 == null || membraneCreator.attachable2 == null)
		{
			return;
		}

		Membrane createdMembrane = membraneCreator.createdBond as Membrane;
		if (shapingIndices.Count != shapingPoints.Count)
		{
			Debug.LogError("Membrane wall has incorrect number of shaping indices. Ensure that shaping point count and shaping index count are equal.");
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
		startPost.transform.position = startPos;
		membraneCreator.attachable2.transform.position = endPos;
		endPost.transform.position = endPos;

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

	private void MembraneBroken(Membrane brokenMembrane)
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
