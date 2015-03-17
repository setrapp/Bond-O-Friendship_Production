using UnityEngine;
using System.Collections;

public class SpinPadPushee : MonoBehaviour {
	public SpinPadSide targetSidePad;
	public SpinPad targetSpinPad;
	private bool exitting = false;
	public Collider pusheeCollider;

	void Start()
	{
		if (pusheeCollider == null)
		{
			pusheeCollider = GetComponent<Collider>();
		}
	}

	void Update()
	{
		if (!pusheeCollider.collider.enabled)
		{
			pusheeCollider.collider.enabled = true;
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if (targetSidePad.targetPlayer == PlayerInput.Player.Player1 && col.gameObject == Globals.Instance.player1.gameObject)
		{
			targetSpinPad.player1Pushing = true;
		}
		else if (targetSidePad.targetPlayer == PlayerInput.Player.Player2 && col.gameObject == Globals.Instance.player2.gameObject)
		{
			targetSpinPad.player2Pushing = true;
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (targetSidePad.targetPlayer == PlayerInput.Player.Player1 && col.gameObject == Globals.Instance.player1.gameObject)
		{
			targetSpinPad.player1Pushing = false;
		}
		else if (targetSidePad.targetPlayer == PlayerInput.Player.Player2 && col.gameObject == Globals.Instance.player2.gameObject)
		{
			targetSpinPad.player2Pushing = false;
		}
		pusheeCollider.enabled = false;
	}
}
