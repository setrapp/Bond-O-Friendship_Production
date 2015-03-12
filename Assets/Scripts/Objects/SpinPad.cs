using UnityEngine;
using System.Collections;

public class SpinPad : WaitPad
{
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

	protected override void Start()
	{
		player1Pad.spinPad = this;
		player2Pad.spinPad = this;
		finishRing.transform.localScale = new Vector3(ringMinSize, ringMinSize, ringMinSize);
	}

	protected override void Update()
	{
		pOonPad = player1Pad.activated;
		pTonPad = player2Pad.activated;

		if (pOonPad && pTonPad)
		{
			transform.Rotate(new Vector3(0, 0, spinSpeed * Time.deltaTime));
			currentRotation += spinSpeed * Time.deltaTime;

			if (currentRotation >= goalRotation)
			{
				transform.Rotate(new Vector3(0, 0, goalRotation - currentRotation));
				currentRotation = goalRotation;
				FireRipple();
				activated = true;
			}

			float portionComplete = currentRotation / goalRotation;
			float ringSize = (ringMinSize * (1 - portionComplete)) + (rinMaxSize * portionComplete);
			finishRing.transform.localScale = new Vector3(ringSize, ringSize, ringSize);
		}
	}

	private void FireRipple()
	{
		if (rippleFired == false)
		{
			GameObject rippleObj = Instantiate(ripplePrefab, transform.position, Quaternion.identity) as GameObject;
			RingPulse ripple = rippleObj.GetComponent<RingPulse>();
			ripple.transform.localScale = new Vector3(10, 10, 10);
			ripple.scaleRate = 8.0f;
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
