using UnityEngine;
using System.Collections;

public class SpinPad : WaitPad
{
	public bool autoSpin = true;
	public bool flipDirection = false;
	private float rotatingDirection = 1;
	public SpinPadSide player1Pad;
	public SpinPadSide player2Pad;
	public GameObject finishRing;
	public GameObject ripplePrefab;
	private float spinSpeed = 0;
	public float maxSpinSpeed = 60.0f;
	public float spinAcceleration = 1.0f;
	public float goalRotation = 360.0f;
	public float currentRotation = 0;
	public float ringMinSize = 0.01f;
	public float rinMaxSize = 1.15f;
	private bool rippleFired = false;
	private Vector3 torque;
	public bool player1Pushing = false;
	public bool player2Pushing = false;
	public GameObject centerPushee;
	public Renderer player1Pushee;
	public Renderer player2Pushee;
	public float pusheeDisappearInterval = 0.1f;
	private float pusheeStartAlpha = 1;
	private GameObject player1PusheeAnchor;
	private GameObject player2PusheeAnchor;
	public float maxTorque;
	public float torqueDecay;
	public bool spiralPushees = true;
	private float pushStartDistance = 0;
	public float pushEndDistance = 0.0f;
	private float oldRotation = 0;
	public Rigidbody body;
	private Vector3 startRotation;

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

		// Setup anchor for player 1's pushee to move towards or away from (non-auto, spiraling pads).
		pushStartDistance = transform.position.y - player1Pushee.transform.position.y;
		player1PusheeAnchor = new GameObject();
		player1PusheeAnchor.transform.parent = transform;
		player1PusheeAnchor.transform.position = transform.position + ((player1Pushee.transform.position - transform.position) - (transform.right * pushStartDistance * rotatingDirection));
		player1PusheeAnchor.name = "Player 1 Pushee Anchor";

		// Setup anchor for player 2's pushee to move towards or away from (non-auto, spiraling pads).
		player2PusheeAnchor = new GameObject();
		player2PusheeAnchor.transform.parent = transform;
		player2PusheeAnchor.transform.position = transform.position + ((player2Pushee.transform.position - transform.position) + (transform.right * pushStartDistance * rotatingDirection));
		player2PusheeAnchor.name = "Player 2 Pushee Anchor";

		// Place pushable handles into place (non-auto pads).
		player1Pushee.transform.position = player1PusheeAnchor.transform.position + (transform.right * pushStartDistance * rotatingDirection);
		player2Pushee.transform.position = player2PusheeAnchor.transform.position - (transform.right * pushStartDistance * rotatingDirection);

		// Store initial rotation.
		startRotation = transform.rotation.eulerAngles;
		oldRotation = startRotation.z;

		// Alter components based on whether the pad is auto spin or not.
		if (!autoSpin)
		{
			centerPushee.SetActive(true);
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
			Destroy(player1Pad.GetComponent<Rigidbody>());
			Destroy(player2Pad.GetComponent<Rigidbody>());
		}
		else
		{
			centerPushee.SetActive(false);
		}
		//player1Pushee.gameObject.SetActive(false);
		//player2Pushee.gameObject.SetActive(false);


	}

	protected override void Update()
	{
		pOonPad = player1Pad.activated;
		pTonPad = player2Pad.activated;

		if (pOonPad && pTonPad && !activated)
		{
			if (autoSpin)
			{
				// Auto spinning with acceleration.
				spinSpeed = Mathf.Min(spinSpeed + spinAcceleration * Time.deltaTime, maxSpinSpeed);
				transform.Rotate(new Vector3(0, 0, spinSpeed * Time.deltaTime));
				currentRotation += spinSpeed * Time.deltaTime;
			}
			else
			{
				// Track rotation caused by torque, if not an auto spin pad.
				float rotation = transform.rotation.eulerAngles.z;
				if (player1Pushing && player2Pushing)
				{
					if (body.isKinematic)
					{
						body.isKinematic = false;
					}
						
					float rotChange = body.angularVelocity.z;
					currentRotation += rotChange * rotatingDirection;
				}
				else
				{
					if (!body.isKinematic)
					{
						body.isKinematic = true;
					}
				}

				transform.rotation = Quaternion.Euler(startRotation.x, startRotation.y, rotation);
				oldRotation = rotation;
			}

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

			// Scale the finish ring and position spiralling pushee handles based on how much of the required rotation is complete.
			float portionComplete = currentRotation / goalRotation;
			float ringSize = (ringMinSize * (1 - portionComplete)) + (rinMaxSize * portionComplete);
			finishRing.transform.localScale = new Vector3(ringSize, ringSize, ringSize);
			if (!autoSpin)
			{
				if (spiralPushees)
				{
					float pusheeDist = ((pushStartDistance * (1 - portionComplete)) + (pushEndDistance * portionComplete));
					player1Pushee.transform.position = player1PusheeAnchor.transform.position + (transform.right * pusheeDist * rotatingDirection);
					player2Pushee.transform.position = player2PusheeAnchor.transform.position - (transform.right * pusheeDist * rotatingDirection);
				}
				else
				{
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
				}
			}
		}
		else
		{
			spinSpeed = 0;
			if (!body.isKinematic)
			{
				body.isKinematic = true;
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
		

		torque.z -= torqueDecay * Time.deltaTime;
		if (torque.z < 0)
		{
			torque.z = 0;
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

	// Force pad to defer collision handling to side pads.
	protected override void OnTriggerEnter(Collider collide){ }
	protected override void OnTriggerExit(Collider collide) { }
}
