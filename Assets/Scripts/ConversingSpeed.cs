using UnityEngine;
using System.Collections;

public class ConversingSpeed : MonoBehaviour {
	public SimpleMover mover;
	public PartnerLink partnerLink;
	private float targetSpeed;
	private BoostStatus boostStatus;
	private bool draining;
	private float boostLeft;
	private float boostIncrement;
	private float startMaxSpeed;
	private string boostName = "";

	public enum BoostStatus
	{
		STABLE = 0,
		BOOST,
		DRAIN
	}

	void Start()
	{
		if (mover == null)
		{
			mover = GetComponent<SimpleMover>();
		}
		if (mover != null)
		{
			startMaxSpeed = mover.maxSpeed;
		}
		if (partnerLink == null)
		{
			partnerLink = GetComponent<PartnerLink>();
		}
	}

	void Update()
	{
		if (boostStatus != BoostStatus.STABLE)
		{
			boostLeft -= Time.deltaTime;
			mover.maxSpeed = mover.maxSpeed + (boostIncrement * Time.deltaTime);
			bool boostEnded = false;
			if (boostStatus == BoostStatus.BOOST)
			{
				if (mover.maxSpeed >= targetSpeed)
				{
					mover.maxSpeed = targetSpeed;
					boostEnded = true;
				}
				if (boostEnded || boostLeft <= 0)
				{
					SendMessage("EndSpeedBoost", boostName, SendMessageOptions.DontRequireReceiver);
					boostEnded = true;
				}
			}
			else
			{
				if (mover.maxSpeed <= targetSpeed)
				{
					mover.maxSpeed = targetSpeed;
					boostEnded = true;
				}
				if (boostEnded || boostLeft <= 0)
				{
					SendMessage("EndSpeedDrainEnd", boostName, SendMessageOptions.DontRequireReceiver);
					boostEnded = true;
				}
			}

			if (boostEnded)
			{
				boostLeft = 0;
				boostIncrement = 0;
				boostStatus = BoostStatus.STABLE;
				SendMessage("EndSpeedChange", boostName, SendMessageOptions.DontRequireReceiver);
				boostName = "";
			}
		}
	}

	public void TargetRelativeSpeed(float boostPercentage, float changeRate, string boostName = "")
	{
		targetSpeed = mover.maxSpeed * (1 + boostPercentage);
		TargetAbsoluteSpeed(targetSpeed, changeRate);
	}

	public void TargetAbsoluteSpeed(float targetSpeed, float changeRate, string boostName = "")
	{
		Interrupt();

		this.targetSpeed = targetSpeed;
		if (boostName == null)
		{
			boostName = "";
		}
		this.boostName = boostName;

		if (changeRate <= 0)
		{
			boostLeft = 0;
			boostIncrement = 0;
			boostStatus = BoostStatus.STABLE;
			SendMessage("EndSpeedChange", boostName, SendMessageOptions.DontRequireReceiver);
			boostName = "";
			return;
		}

		if (targetSpeed >= mover.maxSpeed)
		{
			boostLeft = 1 / changeRate;
			boostIncrement = (targetSpeed - mover.maxSpeed) * changeRate;
			boostStatus = BoostStatus.BOOST;
		}
		else
		{
			boostLeft = 1 / changeRate;
			boostIncrement = (targetSpeed - mover.maxSpeed) * changeRate;
			boostStatus = BoostStatus.DRAIN;
		}
	}

	private void Interrupt()
	{
		bool interrupted = false;
		switch (boostStatus)
		{
		case BoostStatus.BOOST:
			SendMessage("InterruptSpeedBoost", boostName, SendMessageOptions.DontRequireReceiver);
			interrupted = true;
			break;
		case BoostStatus.DRAIN:
			SendMessage("InterruptSpeedDrain", boostName, SendMessageOptions.DontRequireReceiver);
			interrupted = true;
			break;
		}

		if (interrupted)
		{
			SendMessage("InterruptSpeedChange", boostName, SendMessageOptions.DontRequireReceiver);
		}
		boostName = "";
	}
}
