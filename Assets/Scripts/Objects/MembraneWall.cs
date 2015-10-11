using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MembraneWall : MonoBehaviour {
	public AutoMembrane membraneCreator;
	public bool createOnStart = true;
	public MembraneCreationLink creationLink = null;
	public Space space = Space.World;
	public bool destroyWhenBroken = true;
	public bool wallIsCentered = true;
	public bool baseOnPosts = false;
	public Vector3 membraneDirection;
	public float membraneLength;
	public bool showPosts = false;
	public bool disableAtFar = false;
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
	public string specialBreakerTag = "";
	public int requiredSpecialsToBreak = 0;
	private bool reqsWereMet = true;
	//public AsyncOperation requiredLoading = null;
	public float insufficientDifficulty = 1;
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

		if (creationLink == null)
		{
			creationLink = GetComponent<MembraneCreationLink>();
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

		if (startPost != null)
		{
			startPost.SetActive(showPosts);
		}
		if (endPost != null)
		{
			endPost.SetActive(showPosts);
		}
	}

	void Update()
	{
		Membrane createdMembrane = membraneCreator.createdBond as Membrane;
		if (membraneCreator != null && createdMembrane != null)
		{
			if (showPosts)
			{
				if (startPost != null)
				{
					createdMembrane.attachment1.attachee.transform.position = startPost.transform.position;
				}
				if (endPost != null)
				{
					createdMembrane.attachment2.attachee.transform.position = endPost.transform.position;
				}
			}

			currentLength = createdMembrane.BondLength;
			float shapedDistance = ShapedDistance;
			float maxDistance = shapedDistance * relativeMaxDistance + requirementDistanceAdd;
			if (relativeMaxDistance < 0)
			{
				maxDistance = createdMembrane.stats.maxDistance;
			}
			relativeActualDistance = currentLength / shapedDistance;

			// Ensure that enough players are attempting to break the membrane.
			bool enoughPlayersBonded = true;
			if (requiredPlayersToBreak > 0)
			{
				int playersBonded = 0;
				if (createdMembrane.IsBondMade(Globals.Instance.Player1.character.bondAttachable))
				{
					playersBonded++;
				}
				if (createdMembrane.IsBondMade(Globals.Instance.Player2.character.bondAttachable))
				{
					playersBonded++;
				}
				if (playersBonded < requiredPlayersToBreak)
				{
					enoughPlayersBonded = false;
				}
			}

			// Ensure that enough ring breakers are attempting to break the membrane.
			bool enoughSpecialsBonded = true;
			if (requiredSpecialsToBreak > 0)
			{
				GameObject[] breakers = createdMembrane.BondedObjectsWithTag(specialBreakerTag);
				enoughSpecialsBonded = breakers.Length >= requiredSpecialsToBreak;
			}

			//bool loadingComplete = (requiredLoading == null) || requiredLoading.isDone;

			bool requirementsMet = enoughPlayersBonded && enoughSpecialsBonded;// && loadingComplete;

			if (!requirementsMet)
			{	
				maxDistance = -1;
				if (reqsWereMet != requirementsMet)
				{
					createdMembrane.stats.springForce = membraneCreator.bondOverrideStats.stats.springForce * insufficientDifficulty;
				}
				requirementDistanceAdd = 0;
			}
			else
			{
				if (reqsWereMet != requirementsMet)
				{
					createdMembrane.stats.springForce = membraneCreator.bondOverrideStats.stats.springForce;
				}

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
			reqsWereMet = requirementsMet;

			createdMembrane.stats.maxDistance = maxDistance;

			if (disableAtFar)
			{
				Vector3 midPoint = (startPost.transform.position + endPost.transform.position) / 2;
				float sqrToPlayer1 = (midPoint - Globals.Instance.Player1.transform.position).sqrMagnitude;
				float sqrToPlayer2 = (midPoint - Globals.Instance.Player2.transform.position).sqrMagnitude;
				bool playerNearPosts = (sqrToPlayer1 <= Mathf.Pow(createdMembrane.stats.sparseDetailDistance, 2) || sqrToPlayer2 <= Mathf.Pow(createdMembrane.stats.sparseDetailDistance, 2));

				if (createdMembrane.gameObject.activeSelf && (!playerNearPosts && createdMembrane.currentDetail <= createdMembrane.stats.sparseDetailFactor))
				{
					createdMembrane.gameObject.SetActive(false);
				}
				else if (!createdMembrane.gameObject.activeSelf && playerNearPosts)
				{
					createdMembrane.gameObject.SetActive(true);
				}
			}
		}
	}

	public Membrane CreateWall()
	{
		if (membraneCreator == null || membraneCreator.attachable1 == null || membraneCreator.attachable2 == null)
		{
			return null;
		}

		if (shapingIndices.Count != shapingPoints.Count)
		{
			Debug.LogError("Membrane wall has incorrect number of shaping indices. Ensure that shaping point count and shaping index count are equal.");
		}

		// If posts are already placed correctly, calculate membrane length and direction based on them.
		if (baseOnPosts)
		{
			Vector3 postToPost = endPost.transform.position - startPost.transform.position;
			membraneLength = postToPost.magnitude;
			membraneDirection = postToPost / membraneLength;
		}

		// Update override stats to account for starting distance between endpoints.
		membraneDirection.Normalize();
		if (space == Space.Self)
		{
			membraneDirection = transform.TransformDirection(membraneDirection);
		}
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
		if (startPost != null)
		{
			startPost.transform.position = startPos;
		}
		membraneCreator.attachable2.transform.position = endPos;
		if (endPost != null)
		{
			endPost.transform.position = endPos;
		}

		// Break the membrane track into eigen vectors.
		Vector3 parallel = membraneTrack;
		Vector3 perpendicular = Vector3.Cross(Vector3.forward, membraneTrack);

		// Create shaping points and place them relative to the membrane track.
		if (membraneCreator.shapingPointContainer != null)
		{
			for (int i = 0; i < shapingPoints.Count; i++)
			{
				GameObject newShapingObject = (GameObject)Instantiate(shapingPointPrefab);

				// Position shaping point relative to membrane track.
				newShapingObject.transform.position = startPos + (parallel * shapingPoints[i].position.y) + (perpendicular * shapingPoints[i].position.x);

				// Populate shaping point values, fallback to defaults when necessary.
				ShapingPoint newShapingPoint = newShapingObject.GetComponent<ShapingPoint>();
				if (newShapingPoint != null)
				{
					if (shapingPoints[i].shapingForce < 0)
					{
						shapingPoints[i].shapingForce = defaultShapingForce;
					}
					newShapingPoint.shapingForce = shapingPoints[i].shapingForce;
				}

				if (shapingPoints[i].pointName != null && shapingPoints[i].pointName != "")
				{
					newShapingObject.name = shapingPoints[i].pointName;
				}

				newShapingObject.transform.parent = membraneCreator.shapingPointContainer.transform;
			}
		}

		// Create membrane.
		membraneCreator.CreateBond();
		if (membraneCreator.createdBond != null)
		{
			membraneCreator.createdBond.transform.parent = transform;
		}

		return membraneCreator.createdBond as Membrane;
	}

	private void MembraneBreaking(Membrane BreakingMembrane)
	{
		if (transform.parent != null)
		{
			transform.parent.SendMessage("MembraneWallBreaking", this, SendMessageOptions.DontRequireReceiver);
		}
	}

	private void MembraneBroken(Membrane brokenMembrane)
	{
		for (int i = 0; i < membraneCreator.shapingPointContainer.transform.childCount;i ++)
		{
			Destroy(membraneCreator.shapingPointContainer.transform.GetChild(i).gameObject);
		}

		if (transform.parent != null)
		{
			transform.parent.SendMessage("MembraneWallBroken", this, SendMessageOptions.DontRequireReceiver);
		}
		if (destroyWhenBroken)
		{
			Destroy(gameObject);
		}
	}

	private void MembraneBonding(Membrane bondingMembrane)
	{
		if (transform.parent != null && membraneCreator != null && bondingMembrane != null && bondingMembrane == membraneCreator.createdBond)
		{
			transform.parent.SendMessage("MembraneWallBonding", this, SendMessageOptions.DontRequireReceiver);
		}
	}
}
