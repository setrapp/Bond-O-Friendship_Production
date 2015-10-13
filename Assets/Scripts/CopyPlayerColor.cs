using UnityEngine;
using System.Collections;

public class CopyPlayerColor : MonoBehaviour {
	public Renderer renderer;
	public PlayerInput.Player targetPlayer;
	public Color tint = Color.white;
	public bool isAdditive = false;

	void Start()
	{
		if (renderer == null)
		{
			renderer = GetComponent<Renderer>();
		}

		ApplyTint();
	}

	public void ApplyTint(float tintFactor = 1)
	{
		Color renderColor = Globals.Instance.Player1.character.colors.baseColor;
		if (targetPlayer == PlayerInput.Player.Player2)
		{
			renderColor = Globals.Instance.Player2.character.colors.baseColor;
		}
		
		if (isAdditive)
		{
			renderColor += tint * tintFactor;
		}
		else if (tintFactor > 0)
		{
			renderColor *= tint / tintFactor;
		}
		
		renderer.material.color = renderColor;
	}
}
