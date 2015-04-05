using UnityEngine;
using System.Collections;

public class SpinPadPushee : MonoBehaviour {
	public SpinPadSide targetSidePad;
	public SpinPad targetSpinPad;
	private bool exitting = false;
	public Collider pusheeCollider;
	public bool pushing = false;

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
			if (targetSpinPad != null)
			{
				targetSpinPad.player1Pushing = true;
			}
			pushing = true;
		}
		else if (targetSidePad.targetPlayer == PlayerInput.Player.Player2 && col.gameObject == Globals.Instance.player2.gameObject)
		{
			if (targetSpinPad != null)
			{
				targetSpinPad.player2Pushing = true;
			}
			pushing = true;
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (targetSidePad.targetPlayer == PlayerInput.Player.Player1 && col.gameObject == Globals.Instance.player1.gameObject)
		{
			if (targetSpinPad != null)
			{
				targetSpinPad.player1Pushing = false;
			}
			pushing = false;
		}
		else if (targetSidePad.targetPlayer == PlayerInput.Player.Player2 && col.gameObject == Globals.Instance.player2.gameObject)
		{
			if (targetSpinPad != null)
			{
				targetSpinPad.player2Pushing = false;
			}
			pushing = false;
		}
		pusheeCollider.enabled = false;
	}
}
