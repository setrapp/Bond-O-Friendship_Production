using UnityEngine;
using System.Collections;

public class SpinPadPushee : MonoBehaviour {
	public bool pushing = false;
	public SpinPadSide targetSidePad;

	void OnTriggerEnter(Collider col)
	{
		GameObject targetObject = Globals.Instance.player1.gameObject;
		if (targetSidePad.targetPlayer == PlayerInput.Player.Player2)
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
		GameObject targetObject = Globals.Instance.player1.gameObject;
		if (targetSidePad.targetPlayer == PlayerInput.Player.Player2)
		{
			targetObject = Globals.Instance.player2.gameObject;
		}

		if (col.gameObject == targetObject)
		{
			targetSidePad.activating = false;
		}
	}
}
