using UnityEngine;
using System.Collections;

public class LeverPad : MonoBehaviour//WaitPad
{

	public MembraneWall membraneWall;
	public Membrane membrane1;
	public SpinPadSide wallEnd1;
	public Collider wallEnd1Collider;
	public GameObject rotatee;

    private Vector3 wallEndStartPosition;

	private float oldRotateeRotation;
	//public bool completeOnIn = false;
	//public bool completeOnOut = false;
	public float currentRadius = 5.5f;
	private Vector3 oldWallEndPos1;
	public float fullInRotation = 0;
	public float fullOutRotation = -360;
	public float currentRotation = 0;
	public float rotationProgress = 0;
	public float dragDecreaseSpeed = 500;
	public float dragIncreaseSpeed = 50;
	public float membraneAttachmentSpring = 50;
	public int spinInhibitors = 0;
	public LineRenderer innerLine;
	//public LineRenderer outerLine;
	public Color lineInactiveColor = new Color(0.5f, 0.5f, 0.5f, 0.4f);
	public Color lineNotCompletedColor = new Color(0.75f, 0.75f, 0.75f, 0.5f);
	public Color lineCompletedColor = new Color(1.0f, 1.0f, 1.0f, 0.8f);

    public bool triggered = false;

	void Start()
	{
		rotatee.transform.LookAt(rotatee.transform.position - Vector3.forward, wallEnd1.transform.transform.position - rotatee.transform.position);
		oldRotateeRotation = rotatee.transform.eulerAngles.z;
		oldWallEndPos1 = wallEnd1.transform.position;
        wallEndStartPosition = wallEnd1.transform.position;

		if (innerLine != null)
			Helper.DrawCircle(innerLine, gameObject, Vector3.zero, currentRadius);

		CalculateRotationProgress();
		//float progress = (rotationProgress / 2) + 0.5f;
		membraneWall.membraneLength = currentRadius;

		UpdatePadRotation();

		SetLineColors();
	}

	void Update()
	{
		if (membrane1 == null)
		{
			membrane1 = (Membrane)membraneWall.membraneCreator.createdBond;
			if (membrane1 != null)
			{
				membrane1.stats.attachSpring2 = membraneAttachmentSpring;
			}
		}

		if (membrane1 != null )
		{

			if (PlayersPushing() && !triggered)
				membrane1.stats.attachSpring2 = membraneAttachmentSpring;
			else
			{
				membrane1.stats.attachSpring2 = 0;
                RotateBack();
			}

			// Handle rotation of the pad.
			UpdatePadRotation();
		}

		CalculateRotationProgress();


		if (wallEnd1 != null && wallEnd1Collider != null)
			wallEnd1Collider.transform.position = wallEnd1.transform.position;
		
		
	}

    private void UpdatePadRotation()
    {
        // Calculate the vectors from the center to the edges of the pad where the walls ends are.
        Vector3 toWallEnd1 = wallEnd1.transform.position - rotatee.transform.position;
        toWallEnd1.z = 0;
        toWallEnd1 = toWallEnd1.normalized * (currentRadius);


        // Keep the wall ends moving exactly opposite each other, moving only as fast as the slower of the two.
        Vector3 newWallEndPos1 = rotatee.transform.position + toWallEnd1;

        // Rotate the rotation tracker to point its up vector at the first wall end.
        rotatee.transform.LookAt(rotatee.transform.position - Vector3.forward, wallEnd1.transform.transform.position - rotatee.transform.position);
        float movementDotRight = Vector3.Dot(newWallEndPos1 - oldWallEndPos1, rotatee.transform.right);
        float newRotateeRotation = rotatee.transform.eulerAngles.z;

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

            membrane1.extraStats.defaultShapingForce = 0;
        }

        // Update rotation progress.
        currentRotation += rotationChange;
        oldRotateeRotation = newRotateeRotation;

        // Move wall ends to new positions.
        wallEnd1.transform.position = new Vector3(newWallEndPos1.x, newWallEndPos1.y, wallEnd1.transform.position.z);

        // Maintain a copy of the wall positions.
        oldWallEndPos1 = newWallEndPos1;
    }

	private bool PlayersPushing()
	{
		return membrane1.IsBondMade(Globals.Instance.Player1.character.bondAttachable) || membrane1.IsBondMade(Globals.Instance.Player2.character.bondAttachable);            
	}


    private void RotateBack()
    {
        wallEnd1.transform.position = Vector3.Lerp(wallEnd1.transform.position, wallEndStartPosition, Time.deltaTime);

        if (Vector3.Distance(wallEnd1.transform.position, wallEndStartPosition) < 1f)
            triggered = false;
    }

	private void CalculateRotationProgress()
	{
		float rotationRange = fullInRotation - fullOutRotation;
		float midRotation = (fullInRotation + fullOutRotation) / 2;

		rotationProgress = Mathf.Clamp((currentRotation - midRotation) / (rotationRange / 2), -1, 1);

        if (rotationProgress == -1)
            triggered = true;
	}

	public bool IsAtLimit(SpinLimit desiredLimit)
	{
		bool atLimit = false;
		if ((desiredLimit & SpinLimit.PULL_END) == SpinLimit.PULL_END && rotationProgress >= 1)
		{
			atLimit = true;
		}
		if ((desiredLimit & SpinLimit.PUSH_END) == SpinLimit.PUSH_END && rotationProgress <= -1)
		{
			atLimit = true;
		}
		return atLimit;
	}

	public void SetLineColors()
	{
        if (innerLine != null)
            innerLine.material.color = lineInactiveColor;	
	}

	public enum SpinLimit
	{
		PULL_END = 1,
		PUSH_END = 2
	}
}


