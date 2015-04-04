﻿using UnityEngine;
using System.Collections;

public class SpinPadWallSync : MonoBehaviour {

	public MembraneWall membraneWall1;
	public MembraneWall membraneWall2;
	private Membrane membrane1;
	private Membrane membrane2;
	public Rigidbody wallEnd1;
	public Rigidbody wallEnd2;
	public GameObject rotatee;
	public GameObject helmet1;
	public GameObject helmet2;
	private float oldRotateeRotation;
	public float spinRadius = 5.5f;
	private Vector3 center;
	private Vector3 oldWallEndPos1;
	private Vector3 oldWallEndPos2;
	public float rotationProgress;
	public float maxDrag = 100;
	public float minDrag = 1.5f;
	public float dragDecreaseSpeed = 500;
	public float dragIncreaseSpeed = 50;
	public float membraneAttachmentSpring = 50;

	void Start()
	{
		center = (wallEnd1.transform.position + wallEnd2.transform.position) / 2;
		rotatee.transform.position = center;
		rotatee.transform.LookAt(rotatee.transform.position - Vector3.forward, wallEnd1.transform.transform.position - rotatee.transform.position);
		oldRotateeRotation = rotatee.transform.eulerAngles.z;
		oldWallEndPos1 = wallEnd1.transform.position;
		oldWallEndPos2 = wallEnd2.transform.position;

		wallEnd1.drag = maxDrag;
		wallEnd2.drag = maxDrag;
	}

	void Update()
	{
		if (membrane1 == null)
		{
			membrane1 = (Membrane)membraneWall1.membraneCreator.createdBond;
		}
		if (membrane2 == null)
		{
			membrane2 = (Membrane)membraneWall2.membraneCreator.createdBond;
		}

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
		
	}

	private void UpdatePadRotation()
	{
		// Calculate the vectors from the center to the edges of the pad where the walls ends are.
		Vector3 toWallEnd1 = wallEnd1.transform.position - center;
		toWallEnd1.z = 0;
		toWallEnd1 = toWallEnd1.normalized * (spinRadius);

		Vector3 toWallEnd2 = wallEnd2.transform.position - center;
		toWallEnd2.z = 0;
		toWallEnd2 = toWallEnd2.normalized * (spinRadius);


		// Keep the wall ends moving exactly opposite each other, moving only as fast as the slower of the two.
		Vector3 newWallEndPos1 = center + toWallEnd1;
		Vector3 newWallEndPos2 = center + toWallEnd2;
		if ((newWallEndPos2 - oldWallEndPos2).sqrMagnitude < (newWallEndPos1 - oldWallEndPos1).sqrMagnitude)
		{
			newWallEndPos1 = center - toWallEnd2;
		}
		else
		{
			newWallEndPos2 = center - toWallEnd1;
		}

		// Rotate the rotation tracker to point its up vector at the first wall end.
		rotatee.transform.LookAt(rotatee.transform.position - Vector3.forward, wallEnd1.transform.transform.position - rotatee.transform.position);
		float movementDotRight = Vector3.Dot(newWallEndPos1 - oldWallEndPos1, rotatee.transform.right);
		float newRotateeRotation = rotatee.transform.eulerAngles.z;

		// Lower spinning drag when pushing correctly, or raise it otherwise.
		bool rotating = newRotateeRotation != oldRotateeRotation;
		Vector3 centerToPlayer1 = Helper.ProjectVector(rotatee.transform.right, Globals.Instance.player1.transform.position - center);
		Vector3 centerToPlayer2 = Helper.ProjectVector(rotatee.transform.right, Globals.Instance.player2.transform.position - center);
		bool correctPushDirections = Vector3.Dot(centerToPlayer1, centerToPlayer2) < 0;
		float drag = wallEnd1.drag;
		if (correctPushDirections && rotating)
		{
			drag -= dragDecreaseSpeed * Time.deltaTime;
		}
		else
		{
			drag += dragIncreaseSpeed * Time.deltaTime;
		}
		wallEnd1.drag = wallEnd2.drag = Mathf.Clamp(drag, minDrag, maxDrag);

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

		// Update rotation progress.
		rotationProgress += rotationChange;
		oldRotateeRotation = newRotateeRotation;

		// Move wall ends to new positions.
		wallEnd1.transform.position = new Vector3(newWallEndPos1.x, newWallEndPos1.y, wallEnd1.transform.position.z);
		wallEnd2.transform.position = new Vector3(newWallEndPos2.x, newWallEndPos2.y, wallEnd2.transform.position.z);

		// Maintain a copy of the wall positions.
		oldWallEndPos1 = newWallEndPos1;
		oldWallEndPos2 = newWallEndPos2;
	}

	private bool PlayersPushing()
	{
		bool bondedInOrder = membrane1.IsBondMade(Globals.Instance.player1.character.bondAttachable) && membrane2.IsBondMade(Globals.Instance.player2.character.bondAttachable);
		bool bondedReverseOrder = membrane1.IsBondMade(Globals.Instance.player2.character.bondAttachable) && membrane2.IsBondMade(Globals.Instance.player1.character.bondAttachable);

		return bondedInOrder || bondedReverseOrder;
	}
}
