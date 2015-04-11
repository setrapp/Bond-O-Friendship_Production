using UnityEngine;
using System.Collections;

public class MaterialBondReaction : MonoBehaviour {

	public Renderer targetRenderer;
	public bool reactingToBond = false;
	public float nonBondedAlpha = 1.0f;
	public float bondedAlpha = 0.5f;
	private Color mainColor;

	void Start()
	{
		if (targetRenderer == null)
		{
			targetRenderer = GetComponent<Renderer>();
		}

		mainColor = targetRenderer.material.GetColor("_Color");

		ReactToPlayerBond(true);
	}

	void Update()
	{
		ReactToPlayerBond(false);
	}

	private void ReactToPlayerBond(bool forceReact)
	{
		if (reactingToBond != Globals.Instance.playersBonded || forceReact)
		{
			reactingToBond = Globals.Instance.playersBonded;
			if (reactingToBond)
			{
				mainColor.a = bondedAlpha;

			}
			else
			{
				mainColor.a = nonBondedAlpha;
			}
			targetRenderer.material.SetColor("_Color", mainColor);
		}
	}
}
