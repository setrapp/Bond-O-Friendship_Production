using UnityEngine;
using System.Collections;

public class SpinPadPushee : MonoBehaviour {
	public SpinPadSide targetSidePad;
	public SpinPad targetSpinPad;

	void OnTriggerEnter(Collider col)
	{
		Debug.Log(col.gameObject.name);
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
			Debug.Log("bye");
			targetSpinPad.player1Pushing = false;
		}
		else if (targetSidePad.targetPlayer == PlayerInput.Player.Player2 && col.gameObject == Globals.Instance.player2.gameObject)
		{
			targetSpinPad.player2Pushing = false;
		}
	}
}
