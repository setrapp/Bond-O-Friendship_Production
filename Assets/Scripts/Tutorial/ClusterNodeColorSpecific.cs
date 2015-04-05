using UnityEngine;
using System.Collections;

public class ClusterNodeColorSpecific : ClusterNode {

	public PlayerInput.Player neededPlayer;
	private Collider neededCollider;
	public Color colorDesaturation;

	protected override void Start()
	{
		base.Start();

		CharacterComponents neededCharacter = Globals.Instance.player1.character;
		if (neededPlayer == PlayerInput.Player.Player2)
		{
			neededCharacter = Globals.Instance.player2.character;
		}
		neededCollider = neededCharacter.collider;
		nodeRenderer.material.color = neededCharacter.colors.attachmentColor - colorDesaturation;
		startingcolor = nodeRenderer.material.color;
	}

	protected override void OnTriggerEnter(Collider col)
	{
		if (col == neededCollider)
		{
			base.OnTriggerEnter(col);
		}
	}
}
