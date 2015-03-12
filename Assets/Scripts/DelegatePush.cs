using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DelegatePush : MonoBehaviour {

	[EnumFlags]
	public PushableLocalDirections pushableLocalDirections = PushableLocalDirections.X_POSITIVE;
	public GameObject pushDelegate;
	public GameObject pushDelegator;
	public Collider baseCollider;
	public Rigidbody solePusherOptional;
	public bool pushedByPlayer = false;
	public PlayerInput.Player pushingPlayerOptional;
	public bool convertToLocalDirection = true;
	public float pushForce = 10;

	void Start()
	{
		if (pushedByPlayer)
		{
			solePusherOptional = Globals.Instance.player1.character.body;
			if (pushingPlayerOptional == PlayerInput.Player.Player2)
			{
				solePusherOptional = Globals.Instance.player2.character.body;
			}
		}

		if (pushDelegator == null)
		{
			pushDelegator = gameObject;
		}
		if (baseCollider == null)
		{
			baseCollider = GetComponent<Collider>();
		}
	}

	void OnCollisionStay(Collision col)
	{
		if (!baseCollider.isTrigger)
		{
			AttemptPush(col.collider.GetComponent<Rigidbody>());
		}
	}

	void OnTriggerStay(Collider col)
	{
		if (baseCollider.isTrigger)
		{
			AttemptPush(col.GetComponent<Rigidbody>());
		}
	}

	private void AttemptPush(Rigidbody collidingBody)
	{
		if (Time.deltaTime <= 0)
		{
			return;
		}

		if (collidingBody != null && (solePusherOptional == null || collidingBody == solePusherOptional))
		{
			DelegatedPush delegatedPush = new DelegatedPush();
			delegatedPush.delegator = pushDelegator;
			delegatedPush.pusher = collidingBody.gameObject;
			delegatedPush.pushForce = (collidingBody.transform.forward * pushForce / Time.deltaTime) * collidingBody.mass;
			delegatedPush.pushForce.z = 0;

			if (CheckPushableDirection(delegatedPush.pushForce))
			{
				pushDelegate.SendMessage("DelegatedPush", delegatedPush, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	private bool CheckPushableDirection(Vector3 attemptedPush)
	{
		Vector3 localPush = transform.InverseTransformDirection(attemptedPush);
		
		bool pushableDirection = false;

		if ((pushableLocalDirections & PushableLocalDirections.X_POSITIVE) == PushableLocalDirections.X_POSITIVE)
		{
			pushableDirection = Vector3.Dot(Vector3.right, localPush) > 0;
		}
		if ((pushableLocalDirections & PushableLocalDirections.Y_POSITIVE) == PushableLocalDirections.Y_POSITIVE)
		{
			pushableDirection = pushableDirection || Vector3.Dot(Vector3.up, localPush) > 0;
		}
		if ((pushableLocalDirections & PushableLocalDirections.Z_POSITIVE) == PushableLocalDirections.Z_POSITIVE)
		{
			pushableDirection = pushableDirection || Vector3.Dot(Vector3.forward, localPush) > 0;
		}
		if ((pushableLocalDirections & PushableLocalDirections.X_NEGATIVE) == PushableLocalDirections.X_NEGATIVE)
		{
			pushableDirection = pushableDirection || Vector3.Dot(-Vector3.right, localPush) > 0;
		}
		if ((pushableLocalDirections & PushableLocalDirections.Y_NEGATIVE) == PushableLocalDirections.Y_NEGATIVE)
		{
			pushableDirection = pushableDirection || Vector3.Dot(-Vector3.up, localPush) > 0;
		}
		if ((pushableLocalDirections & PushableLocalDirections.Z_NEGATIVE) == PushableLocalDirections.Z_NEGATIVE)
		{
			pushableDirection = pushableDirection || Vector3.Dot(-Vector3.forward, localPush) > 0;
		}

		return pushableDirection;
	}

	[System.Flags]
	public enum PushableLocalDirections
	{
		X_POSITIVE = 1 << 0,
		Y_POSITIVE = 1 << 1,
		Z_POSITIVE = 1 << 2,
		X_NEGATIVE = 1 << 3,
		Y_NEGATIVE = 1 << 4,
		Z_NEGATIVE = 1 << 5
	};
}

public class DelegatedPush
{
	public GameObject delegator;
	public GameObject pusher;
	public Vector3 pushForce;
}
