using UnityEngine;
using System.Collections;

public class SpinPad : WaitPad
{
	public bool autoSpin = true;
	public SpinPadSide player1Pad;
	public SpinPadSide player2Pad;
	public GameObject finishRing;
	public GameObject ripplePrefab;
	public float spinSpeed = 1.0f;
	public float goalRotation = 360.0f;
	public float currentRotation = 0;
	public float ringMinSize = 0.01f;
	public float rinMaxSize = 1.15f;
	private bool rippleFired = false;
	private Vector3 torque;
	public bool player1Pushing = false;
	public bool player2Pushing = false;
	public GameObject player1Pushee;
	public GameObject player2Pushee;
	private GameObject player1PusheeAnchor;
	private GameObject player2PusheeAnchor;
	public float maxTorque;
	public float torqueDecay;
	private float pushStartDistance = 0;
	public float pushEndDistance = 0.0f;
	private float oldRotation = 0;
	public Rigidbody body;
	private float startRotation;

	protected override void Start()
	{
		player1Pad.spinPad = this;
		player2Pad.spinPad = this;
		finishRing.transform.localScale = new Vector3(ringMinSize, ringMinSize, ringMinSize);

		pushStartDistance = transform.position.y - player1Pushee.transform.position.y;
		player1PusheeAnchor = new GameObject();
		player1PusheeAnchor.transform.parent = transform;
		player1PusheeAnchor.transform.position = transform.position + ((player1Pushee.transform.position - transform.position) - (transform.right * pushStartDistance));
		player1PusheeAnchor.name = "Player 1 Pushee Anchor";
		
		player2PusheeAnchor = new GameObject();
		player2PusheeAnchor.transform.parent = transform;
		player2PusheeAnchor.transform.position = transform.position + ((player2Pushee.transform.position - transform.position) + (transform.right * pushStartDistance));
		player2PusheeAnchor.name = "Player 2 Pushee Anchor";
		
		player1Pushee.transform.position = player1PusheeAnchor.transform.position + (transform.right * pushStartDistance);
		player2Pushee.transform.position = player2PusheeAnchor.transform.position - (transform.right * pushStartDistance);

		startRotation = transform.rotation.eulerAngles.z;
		oldRotation = startRotation;

		if (!autoSpin)
		{
			player1Pushee.SetActive(true);
			player2Pushee.SetActive(true);
			Destroy(player1Pad.GetComponent<Rigidbody>());
			Destroy(player2Pad.GetComponent<Rigidbody>());
		}
		else
		{
			player1Pushee.SetActive(false);
			player2Pushee.SetActive(false);
		}
	}

	protected override void Update()
	{
		pOonPad = player1Pad.activated;
		pTonPad = player2Pad.activated;

		if (pOonPad && pTonPad && !activated)
		{
			if (autoSpin)
			{
				transform.Rotate(new Vector3(0, 0, spinSpeed * Time.deltaTime));
				currentRotation += spinSpeed * Time.deltaTime;
			}
			else
			{
				float rotation = transform.rotation.eulerAngles.z;
				//if (player1Pushing && player2Pushing)
				{
					body.isKinematic = false;
					float rotChange = body.angularVelocity.z;
					currentRotation += rotChange;
				}
				
				transform.rotation = Quaternion.Euler(0, 0, rotation);
				oldRotation = rotation;
			}

			if (currentRotation >= goalRotation)
			{
				if (!body.isKinematic)
				{
					body.angularVelocity = Vector3.zero;
				}
				transform.rotation = Quaternion.Euler(0, 0, startRotation + goalRotation);
				body.isKinematic = true;
				currentRotation = goalRotation;
				FireRipple();
				activated = true;
			}

			float portionComplete = currentRotation / goalRotation;
			float ringSize = (ringMinSize * (1 - portionComplete)) + (rinMaxSize * portionComplete);
			finishRing.transform.localScale = new Vector3(ringSize, ringSize, ringSize);

			if (!autoSpin)
			{
				float pusheeDist = ((pushStartDistance * (1 - portionComplete)) + (pushEndDistance * portionComplete));
				player1Pushee.transform.position = player1PusheeAnchor.transform.position + (transform.right * pusheeDist);
				player2Pushee.transform.position = player2PusheeAnchor.transform.position - (transform.right * pusheeDist);
			}
		}
		else
		{
			body.isKinematic = true;
		}

		player1Pushing = false;
		player2Pushing = false;
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

	private void DelegatedPush(DelegatedPush push)
	{
		if (!autoSpin)
		{
			bool validPushee = false;
			if (push.delegator == player1Pushee)
			{
				player1Pushing = true;
				validPushee = true;
			}
			else if (push.delegator == player2Pushee)
			{
				player2Pushing = true;
				validPushee = true;
			}

			if (validPushee)
			{
				torque += Vector3.Cross((push.delegator.transform.position - transform.position).normalized, push.pushForce);
				torque.x = torque.y = 0;
				if (torque.z > maxTorque)
				{
					torque.z = maxTorque;
				}
				if (torque.z < 0)
				{
					torque.z = 0;
				}
			}
		}
	}

	// Force pad to defer collision handling to side pads.
	protected override void OnTriggerEnter(Collider collide){ }
	protected override void OnTriggerExit(Collider collide) { }
}
