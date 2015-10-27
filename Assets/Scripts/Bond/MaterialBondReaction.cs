using UnityEngine;
using System.Collections;

public class MaterialBondReaction : MonoBehaviour {

	public Renderer targetRenderer;
	public bool reactingToBond = false;
	public float nonBondedAlpha = 1.0f;
	public float bondedAlpha = 0.5f;
	public float changeTime = 1;
	private Color mainColor;
	private bool transitioning = false;

	void Start()
	{
		if (targetRenderer == null)
		{
			targetRenderer = GetComponent<Renderer>();
		}

		mainColor = targetRenderer.material.GetColor("_Color");

		//ReactToPlayerBond(true, false);
	}

	void Update()
	{
		//ReactToPlayerBond(false);
	}

	private void ReactToPlayerBond(bool forceReact, bool forcedReaction = false)
	{
		if (reactingToBond != Globals.Instance.playersBonded || forceReact)
		{
			reactingToBond = forceReact ? forcedReaction : Globals.Instance.playersBonded;
			Color startingColor = mainColor;
			if (reactingToBond)
			{
				mainColor.a = bondedAlpha;
			}
			else
			{
				mainColor.a = nonBondedAlpha;
			}

			if (changeTime > 0)
			{
				StartCoroutine(TransitionToColor(startingColor, mainColor));
			}
			else
			{
				targetRenderer.material.SetColor("_Color", mainColor);
			}
		}
	}

	private IEnumerator TransitionToColor(Color startColor, Color endColor)
	{
		// Wait for previous transitions to end before 
		while (transitioning)
		{
			yield return null;
		}

		Color curColor = startColor;
		float progress = 0;
		transitioning = true;

		while (progress < 1)
		{
			progress = Mathf.Min(progress + (Time.deltaTime / changeTime), 1);
			curColor = (startColor * (1 - progress)) + (endColor * progress);
			targetRenderer.material.SetColor("_Color", curColor);
			yield return null;
		}

		transitioning = false;
	}
}
