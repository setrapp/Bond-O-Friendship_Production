using UnityEngine;
using System.Collections;

public class SpinPad : WaitPad
{
	public bool flipDirection = false;
	public Rigidbody body;
	public SpinPadSide player1Pad;
	public SpinPadSide player2Pad;
	public GameObject finishRing;
	public GameObject ripplePrefab;
	private float rotatingDirection = 1;
	public float goalRotation = 360.0f;
	public float currentRotation = 0;
	public float ringMinSize = 0.01f;
	public float rinMaxSize = 1.15f;
	private bool rippleFired = false;
	[HideInInspector]
	public bool player1Pushing = false;
	[HideInInspector]
	public bool player2Pushing = false;
	public GameObject centerPushee;
	public Renderer player1Pushee;
	public Renderer player2Pushee;
	public float pusheeDisappearInterval = 0.1f;
	private float pusheeStartAlpha = 1;
	private Vector3 startRotation;
	public float oldRotation = 0;
	public float resetDelay = 0.0f;
	public float resetAcceleration = 10.0f;
	public float resetMaxSpeed = 100.0f;
	public float resetSpeed = 0f;
	public bool resetting = false;

	protected override void Start()
	{
		// If desired flip the pad over to rotate in the opposite direction.
		if (flipDirection)
		{
			transform.Rotate(new Vector3(180, 0, 0));
			Transform[] allChildren = GetComponentsInChildren<Transform>();
			for (int i = 0; i < allChildren.Length; i++)
			{
				Transform child = allChildren[i];
				if (child != transform)
				{
					child.localPosition = new Vector3(child.localPosition.x, child.localPosition.y, -child.localPosition.z);
				}
			}
			rotatingDirection = -1;
		}

		// Set starting values for components.
		player1Pad.spinPad = this;
		player2Pad.spinPad = this;
		finishRing.transform.localScale = new Vector3(ringMinSize, ringMinSize, ringMinSize);
		SpinPadPushee[] centralPushees = centerPushee.GetComponentsInChildren<SpinPadPushee>();
		for (int i = 0; i < centralPushees.Length; i++)
		{
			centralPushees[i].gameObject.SetActive(false);
		}

		// Store initial rotation.
		startRotation = transform.rotation.eulerAngles;
		oldRotation = startRotation.z;

		// Set color tints for player starter pushees.
		CopyPlayerColor copier1 = player1Pushee.GetComponent<CopyPlayerColor>();
		if (copier1 != null)
		{
			copier1.tint = player1Pad.startTint;
		}
		CopyPlayerColor copier2 = player2Pushee.GetComponent<CopyPlayerColor>();
		if (copier2 != null)
		{
			copier2.tint = player2Pad.startTint;
		}
		pusheeStartAlpha = player1Pushee.material.color.a;
	}

	protected override void Update()
	{
		pOonPad = player1Pad.activated;
		pTonPad = player2Pad.activated;

		if (pOonPad && pTonPad && !activated)
		{
			resetSpeed = 0;

			// Track the new rotation and set to kinematic if not being pushed.
			float newRotation = transform.rotation.eulerAngles.z;
			if (player1Pushing && player2Pushing)
			{
				if (body.isKinematic)
				{
					body.isKinematic = false;
				}
				
				float rotChange = newRotation - oldRotation;
				if (rotChange < -180)
				{
					rotChange = (360 - oldRotation) + newRotation;
				}
				currentRotation += rotChange;
			}
			else
			{
				if (!body.isKinematic)
				{
					body.isKinematic = true;
				}
			}

			// Save the current rotation for comparison next frame.
			oldRotation = newRotation;

			// Snap to finish and produce success feedback when goal rotation is reached.
			if (currentRotation >= goalRotation)
			{
				if (!body.isKinematic)
				{
					body.angularVelocity = Vector3.zero;
				}
				transform.rotation = Quaternion.Euler(startRotation.x, startRotation.y, startRotation.z + goalRotation);
				body.isKinematic = true;
				currentRotation = goalRotation;
				FireRipple();
				activated = true;
			}
		}
		else
		{
			if (!body.isKinematic)
			{
				body.isKinematic = true;
			}

			// Return to starting orientation when not in use and incomplete.
			if (!activated && !resetting && currentRotation > 0)
			{
				resetting = true;
				resetSpeed = 0;
				StartCoroutine(ResetToStart());
			}
		}

		// Scale the finish ring and position spiralling pushee handles based on how much of the required rotation is complete.
		float portionComplete = currentRotation / goalRotation;
		float ringSize = (ringMinSize * (1 - portionComplete)) + (rinMaxSize * portionComplete);
		finishRing.transform.localScale = new Vector3(ringSize, ringSize, ringSize);
		
		// Fade out player starting pushees.
		float disappearComplete = portionComplete / pusheeDisappearInterval;

		Color pusheeColor1 = player1Pushee.material.color;
		pusheeColor1.a = pusheeStartAlpha * (1 - disappearComplete);
		player1Pushee.material.color = pusheeColor1;

		Color pusheeColor2 = player2Pushee.material.color;
		pusheeColor2.a = pusheeStartAlpha * (1 - disappearComplete);
		player2Pushee.material.color = pusheeColor2;

		if (disappearComplete >= 1)
		{
			player1Pushee.gameObject.SetActive(false);
			player2Pushee.gameObject.SetActive(false);

			SpinPadPushee[] centralPushees = centerPushee.GetComponentsInChildren<SpinPadPushee>();
			for (int i = 0; i < centralPushees.Length; i++)
			{
				centralPushees[i].gameObject.SetActive(false);
			}
		}

		// Only enable player starter pushees when the side pads are in use.
		if (currentRotation / goalRotation < pusheeDisappearInterval)
		{
			if ((player1Pad.activating || player1Pad.activated))
			{
				if (!player1Pushee.gameObject.activeSelf)
				{
					player1Pushee.gameObject.SetActive(true);
				}
			}
			else if (player1Pushee.gameObject.activeSelf && player1Pad.timeActivating <= 0)
			{
				player1Pushee.gameObject.SetActive(false);
			}

			if ((player2Pad.activating || player2Pad.activated))
			{
				if (!player2Pushee.gameObject.activeSelf)
				{
					player2Pushee.gameObject.SetActive(true);
				}
			}
			else if (player2Pushee.gameObject.activeSelf && player2Pad.timeActivating <= 0)
			{
				player2Pushee.gameObject.SetActive(false);
			}
		}
	}

	private void FireRipple()
	{
		if (rippleFired == false)
		{
			GameObject rippleObj = Instantiate(ripplePrefab, transform.position, Quaternion.identity) as GameObject;
			RingPulse ripple = rippleObj.GetComponent<RingPulse>();
			ripple.transform.localScale = new Vector3(10, 10, 10);
			ripple.scaleRate = 9.5f;
			ripple.lifeTime = 0.5f;
			ripple.alpha = 1.0f;
			ripple.alphaFade = 2.1f;
			ripple.mycolor = Color.white;
			//rippleObj.GetComponent<RingPulse>().smallRing = true;

			rippleFired = true;
		}
	}

	private IEnumerator ResetToStart()
	{
		resetting = true;
		yield return new WaitForSeconds(resetDelay);

		while (currentRotation > 0 && (!pOonPad || !pTonPad))
		{
			resetSpeed += resetAcceleration * Time.deltaTime;
			if (resetSpeed > resetMaxSpeed)
			{
				resetSpeed = resetMaxSpeed;
			}

			float backspin = resetSpeed * Time.deltaTime;
			if (currentRotation - backspin < 0)
			{
				backspin = currentRotation;
			}

			transform.Rotate(new Vector3(0, 0, -backspin));
			currentRotation -= backspin;
			oldRotation = transform.rotation.eulerAngles.z;

			yield return null;
		}
		resetting = false;
	}

	// Force pad to defer collision handling to side pads.
	protected override void OnTriggerEnter(Collider collide){ }
	protected override void OnTriggerExit(Collider collide) { }
}
