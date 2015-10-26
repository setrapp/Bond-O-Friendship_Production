using UnityEngine;
using System.Collections;

public class SpinPad : WaitPad {

	public MembraneWall membraneWall1;
	public MembraneWall membraneWall2;
	public Membrane membrane1;
	public Membrane membrane2;
	public SpinPadSide wallEnd1;
	public SpinPadSide wallEnd2;
	public StreamMembraneReaction membraneReaction1;
	public StreamMembraneReaction membraneReaction2;
	public Collider wallEnd1Collider;
	public Collider wallEnd2Collider;
	public GameObject rotatee;
	public SpinPadPushee helmet1;
	public SpinPadPushee helmet2;
	private float oldRotateeRotation;
	public bool completeOnIn = false;
	public bool completeOnOut = false;
	[HideInInspector]
	public bool wasCompleted = false;
	public float inRadius = 5.5f;
	public float outRadius = 5.5f;
	public float currentRadius = 5.5f;
	private Vector3 center;
	private Vector3 oldWallEndPos1;
	private Vector3 oldWallEndPos2;
	public float fullInRotation = 0;
	public float fullOutRotation = -360;
	public float currentRotation = 0;
	public float rotationProgress = 0;
	public float dragDecreaseSpeed = 500;
	public float dragIncreaseSpeed = 50;
	public float membraneAttachmentSpring = 50;
	public int spinInhibitors = 0;
	public int spinBackInhibitors = 0;
	public LineRenderer innerLine;
	public LineRenderer outerLine;
	public Color lineInactiveColor = new Color(0.5f, 0.5f, 0.5f, 0.4f);
	public Color lineNotCompletedColor = new Color(0.75f, 0.75f, 0.75f, 0.5f);
	public Color lineCompletedColor = new Color(1.0f, 1.0f, 1.0f, 0.8f);
	public static float nonCompleteThreshold = 0.95f;
	public bool forceComplete = false;

	void Start()
	{
		center = (wallEnd1.transform.position + wallEnd2.transform.position) / 2;
		rotatee.transform.position = center;
		rotatee.transform.LookAt(rotatee.transform.position - Vector3.forward, wallEnd1.transform.transform.position - rotatee.transform.position);
		oldRotateeRotation = rotatee.transform.eulerAngles.z;
		oldWallEndPos1 = wallEnd1.transform.position;
		oldWallEndPos2 = wallEnd2.transform.position;

		if (innerLine != null)
		{
			Helper.DrawCircle(innerLine, innerLine.gameObject, Vector3.zero, inRadius);
		}
		if (outerLine != null)
		{
			Helper.DrawCircle(outerLine, outerLine.gameObject, Vector3.zero, outRadius);
		}

		CalculateRotationProgress();
		float progress = (rotationProgress / 2) + 0.5f;
		currentRadius = (outRadius * (1 - progress)) + (inRadius * progress);
		membraneWall1.membraneLength = membraneWall2.membraneLength = currentRadius;

		UpdatePadRotation();

		SetLineColors();
	}

	void Update()
	{
		if (membrane1 == null)
		{
			membrane1 = (Membrane)membraneWall1.membraneCreator.createdBond;
			if (membrane1 != null)
			{
				membrane1.stats.attachSpring2 = membraneAttachmentSpring;
			}
		}
		if (membrane2 == null)
		{
			membrane2 = (Membrane)membraneWall2.membraneCreator.createdBond;
			if (membrane2 != null)
			{
				membrane2.stats.attachSpring2 = membraneAttachmentSpring;
			}
		}

		CheckHelmets();

		if (membrane1 != null && membrane2 != null)
		{

			if (PlayersPushing())
			{
				membrane1.stats.attachSpring2 = membrane2.stats.attachSpring2 = membraneAttachmentSpring;
			}
			else
			{
				membrane1.stats.attachSpring2 = membrane2.stats.attachSpring2 = 0;
			}

			// Handle rotation of the pad.
			UpdatePadRotation();
		}
		else if (membrane1 != null && membrane2 == null)
		{
			membrane1.stats.attachSpring2 = 0;
		}
		else if (membrane2 != null && membrane1 == null)
		{
			membrane2.stats.attachSpring2 = 0;
		}

		if (!activated)
		{
			CalculateRotationProgress();
			float progress = (rotationProgress / 2) + 0.5f;
			currentRadius = (outRadius * (1 - progress)) + (inRadius * progress);

			if ((completeOnIn && IsAtLimit(SpinLimit.PULL_END)) || (completeOnOut && IsAtLimit(SpinLimit.PUSH_END)) || forceComplete)
			{
				if (!wasCompleted)
				{
					SetLineColors();
					Helper.FirePulse(rotatee.transform.position, Globals.Instance.defaultPulseStats);
					wasCompleted = true;

					if (!neverActivate)
					{
						activated = true;
						if (membrane1 != null)
						{
							membrane1.BreakBond();
						}
						if (membrane2 != null)
						{
							membrane2.BreakBond();
						}

						if (membraneReaction1 != null)
						{
							membraneReaction1.reactionRate = 0;
							membraneReaction1.reactable = false;
						}

						if (membraneReaction2 != null)
						{
							membraneReaction2.reactionRate = 0;
							membraneReaction2.reactable = false;
						}
					}
				}
				forceComplete = false;
			}
			else if (wasCompleted && Mathf.Abs(rotationProgress) < nonCompleteThreshold)
			{
				wasCompleted = false;
			}
		}

		if (activated && membrane1 == null && membrane2 == null)
		{
			rotatee.SetActive(false);
		}

		if (wallEnd1 != null && wallEnd1Collider != null)
		{
			wallEnd1Collider.transform.position = wallEnd1.transform.position;
		}
		if (wallEnd2 != null && wallEnd2Collider != null)
		{
			wallEnd2Collider.transform.position = wallEnd2.transform.position;
		}
	}

	private void UpdatePadRotation()
	{
		// Calculate the vectors from the center to the edges of the pad where the walls ends are.
		Vector3 toWallEnd1 = wallEnd1.transform.position - center;
		toWallEnd1.z = 0;
		toWallEnd1 = toWallEnd1.normalized * (currentRadius);

		Vector3 toWallEnd2 = wallEnd2.transform.position - center;
		toWallEnd2.z = 0;
		toWallEnd2 = toWallEnd2.normalized * (currentRadius);


		// Keep the wall ends moving exactly opposite each other, moving only as fast as the slower of the two.
		Vector3 newWallEndPos1 = center + toWallEnd1;
		Vector3 newWallEndPos2 = center + toWallEnd2;
		if (Vector3.Dot(newWallEndPos1 - oldWallEndPos1, newWallEndPos2 - oldWallEndPos2) < 0)
		{
			if ((newWallEndPos2 - oldWallEndPos2).sqrMagnitude < (newWallEndPos1 - oldWallEndPos1).sqrMagnitude)
			{
				newWallEndPos1 = center - toWallEnd2;
			}
			else
			{
				newWallEndPos2 = center - toWallEnd1;
			}
		}
		else
		{
			// If the wall ends attempted to move in the same direction, do not move.
			newWallEndPos1 = oldWallEndPos1;
			newWallEndPos2 = oldWallEndPos2;
		}

		

		// Rotate the rotation tracker to point its up vector at the first wall end.
		rotatee.transform.LookAt(rotatee.transform.position - Vector3.forward, wallEnd1.transform.transform.position - rotatee.transform.position);
		float movementDotRight = Vector3.Dot(newWallEndPos1 - oldWallEndPos1, rotatee.transform.right);
		float newRotateeRotation = rotatee.transform.eulerAngles.z;

		if (!activated)
		{
			// Maintain continuity of the rotation tracking by handling the crossing of the 360/0 degrees threshold.
			float rotationChange = newRotateeRotation - oldRotateeRotation;
			if (rotationChange > 180)
			{
				rotationChange -= 360;
			}
			else if (rotationChange < -180)
			{
				rotationChange += 360;
			}

			if ((currentRotation >= fullInRotation && rotationChange > 0) || (currentRotation <= fullOutRotation && rotationChange < 0))
			{
				rotationChange = 0;
			}

			if (rotationChange != 0)
			{
				SetLineColors();

				membrane1.extraStats.defaultShapingForce = membrane2.extraStats.defaultShapingForce = 0;
			}

			float changedRotation = currentRotation + rotationChange;
			if (changedRotation >= fullInRotation || changedRotation <= fullOutRotation)
			{
				// Stop spining at ends of rotation range.
				newWallEndPos1 = oldWallEndPos1;
				newWallEndPos2 = oldWallEndPos2;
				if (changedRotation >= fullInRotation)
				{
					currentRotation = fullInRotation;
				}
				else
				{
					currentRotation = fullOutRotation;
				}
				SetLineColors();
			}
			else if ((rotationChange > 0 && spinInhibitors > 0) || (rotationChange < 0 && spinBackInhibitors > 0))
			{
				newWallEndPos1 = oldWallEndPos1;
				newWallEndPos2 = oldWallEndPos2;
			}
			else
			{
				// If not at the ends of the rotation range, update rotation progress.
				currentRotation = changedRotation;
				oldRotateeRotation = newRotateeRotation;
			}
		}

		// Move wall ends to new positions.
		wallEnd1.transform.position = new Vector3(newWallEndPos1.x, newWallEndPos1.y, wallEnd1.transform.position.z);
		wallEnd2.transform.position = new Vector3(newWallEndPos2.x, newWallEndPos2.y, wallEnd2.transform.position.z);

		// Maintain a copy of the wall positions.
		oldWallEndPos1 = newWallEndPos1;
		oldWallEndPos2 = newWallEndPos2;
	}

	private bool PlayersPushing()
	{
		bool wall1Bonded = false;
		if (wallEnd1.playerDependent)
		{
			wall1Bonded = membrane1.IsBondMade(wallEnd1.TargetPlayer.bondAttachable);
		}
		else
		{
			wall1Bonded = membrane1.IsBondMade(Globals.Instance.Player1.character.bondAttachable) || membrane1.IsBondMade(Globals.Instance.Player2.character.bondAttachable);
		}

		bool wall2Bonded = false;
		if (wallEnd2.playerDependent)
		{
			wall2Bonded = membrane2.IsBondMade(wallEnd2.TargetPlayer.bondAttachable);
		}
		else
		{
			wall2Bonded = membrane2.IsBondMade(Globals.Instance.Player1.character.bondAttachable) || membrane2.IsBondMade(Globals.Instance.Player2.character.bondAttachable);
		}

		bool helmetsExist = (helmet1 != null && helmet2 != null) && (helmet1.gameObject.activeInHierarchy && helmet2.gameObject.activeInHierarchy);
		//bool spinInhibited = spinInhibitors > 0;

		return wall1Bonded && wall2Bonded && !helmetsExist;// && !spinInhibited;
	}

	private void CheckHelmets()
	{
		if (helmet1 != null && helmet2 != null && helmet1.gameObject.activeInHierarchy && helmet2.gameObject.activeInHierarchy)
		{
			if (helmet1.pushing && helmet2.pushing)
			{
				helmet1.DestroyAndRipple();
				helmet2.DestroyAndRipple();
			}
		}
	}

	private void CalculateRotationProgress()
	{
		float rotationRange = fullInRotation - fullOutRotation;
		float midRotation = (fullInRotation + fullOutRotation) / 2;

		rotationProgress = Mathf.Clamp((currentRotation - midRotation) / (rotationRange / 2), -1, 1);

		// Calculate portion complete for use in listeners.
		if (completeOnIn && completeOnOut)
		{
			portionComplete = rotationProgress;
		}
		else if (completeOnIn)
		{
			portionComplete = (rotationProgress / 2) + 0.5f;
		}
		else if (completeOnOut)
		{
			portionComplete = (-rotationProgress / 2) + 0.5f;
		}
		else
		{
			portionComplete = 0;
		}
	}

	public bool IsAtLimit(SpinLimit desiredLimit)
	{
		float progressLimit = wasCompleted ? nonCompleteThreshold : 1;
		bool atLimit = false;
		if ((desiredLimit & SpinLimit.PULL_END) == SpinLimit.PULL_END && rotationProgress >= progressLimit)
		{
			atLimit = true;
		}
		if ((desiredLimit & SpinLimit.PUSH_END) == SpinLimit.PUSH_END && rotationProgress <= -progressLimit)
		{
			atLimit = true;
		}
		return atLimit;
	}

	public void SetLineColors()
	{
		/*TODO use inactive color if pushing this direction will no complete the puzzle*/

		if (innerLine != null)
		{
			if (!completeOnIn)
			{
				innerLine.material.color = lineInactiveColor;
			}
			else if (IsAtLimit(SpinLimit.PULL_END))
			{
				innerLine.material.color = lineCompletedColor;
			}
			else
			{
				innerLine.material.color = lineNotCompletedColor;
			}
		}

		if (outerLine != null)
		{
			if (!completeOnOut)
			{
				outerLine.material.color = lineInactiveColor;
			}
			else if (IsAtLimit(SpinLimit.PUSH_END))
			{
				outerLine.material.color = lineCompletedColor;
			}
			else
			{
				outerLine.material.color = lineNotCompletedColor;
			}
		}
		
	}

	private void MembraneWallBroken(MembraneWall brokenMembrane)
	{
		if (brokenMembrane == membraneWall1 || brokenMembrane == membraneWall2)
		{
			brokenMembrane.gameObject.SetActive(false);
		}
	}

	public enum SpinLimit
	{
		PULL_END = 1,
		PUSH_END = 2
	}
}
