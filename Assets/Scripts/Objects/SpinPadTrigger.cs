using UnityEngine;
using System.Collections;

public class SpinPadTrigger : MonoBehaviour {
	public SpinPadSide targetSidePad;

	void OnTriggerEnter(Collider col)
	{
		GameObject targetObject;
		if (targetSidePad.targetPlayer == PlayerInput.Player.Player1)
		{
			targetObject = Globals.Instance.player1.gameObject;
		}
		else
		{
			targetObject = Globals.Instance.player2.gameObject;
		}

		if (col.gameObject == targetObject)
		{
			targetSidePad.activating = true;
		}
	}

	void OnTriggerExit(Collider col)
	{
		GameObject targetObject;
		if (targetSidePad.targetPlayer == PlayerInput.Player.Player1)
		{
			targetObject = Globals.Instance.player1.gameObject;
		}
		else
		{
			targetObject = Globals.Instance.player2.gameObject;
		}

		if (col.gameObject == targetObject)
		{
			targetSidePad.activating = false;
		}
	}
}
