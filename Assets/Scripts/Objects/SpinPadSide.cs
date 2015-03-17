using UnityEngine;
using System.Collections;

public class SpinPadSide : MonoBehaviour {
	[HideInInspector]
	public SpinPad spinPad;
	public PlayerInput.Player targetPlayer;
	public Renderer padRenderer;
	public bool activating = false;
	public bool activated = false;
	public float totalActivateTime = 1;
	public float timeActivating = 0;
	public Color startTint;
	public Color activeTint;
	private Color playerColor;

	void Start()
	{
		if (padRenderer == null)
		{
			padRenderer = GetComponent<Renderer>();
		}

		playerColor = Globals.Instance.player1.character.colors.attachmentColor;
		if (targetPlayer == PlayerInput.Player.Player2)
		{
			playerColor = Globals.Instance.player2.character.colors.attachmentColor;
		}
		padRenderer.material.color = startTint * playerColor;
	}

	void Update()
	{
		// Attempt to update the pad's time spent activating.
		float oldTimeActivating = timeActivating;
		if (activating)
		{
			if (timeActivating < totalActivateTime)
			{
				timeActivating = Mathf.Min(timeActivating + Time.deltaTime, totalActivateTime);
			}
		}
		else if (timeActivating > 0)
		{
			timeActivating = Mathf.Max(timeActivating - Time.deltaTime, 0);
		}

		// If the pad is more or less active, change the color.
		if (oldTimeActivating != timeActivating)
		{
			float portionComplete = timeActivating / totalActivateTime;
			padRenderer.material.color = ((startTint * (1 - portionComplete)) + (activeTint * portionComplete)) * playerColor;
		}

		activated = (timeActivating >= totalActivateTime) || spinPad.activated;
	}

	void OnTriggerEnter(Collider col)
	{
		GameObject targetObject = Globals.Instance.player1.gameObject;
		if (targetPlayer == PlayerInput.Player.Player2)
		{
			targetObject = Globals.Instance.player2.gameObject;
		}

		if (col.gameObject == targetObject)
		{
			activating = true;
		}
	}

	void OnTriggerExit(Collider col)
	{
		GameObject targetObject = Globals.Instance.player1.gameObject;
		if (targetPlayer == PlayerInput.Player.Player2)
		{
			targetObject = Globals.Instance.player2.gameObject;
		}

		if (col.gameObject == targetObject)
		{
			activating = false;
		}
	}
}
