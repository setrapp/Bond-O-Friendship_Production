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
		Color renderColor = Globals.Instance.player1.character.colors.attachmentColor;
		if (targetPlayer == PlayerInput.Player.Player2)
		{
			renderColor = Globals.Instance.player2.character.colors.attachmentColor;
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
