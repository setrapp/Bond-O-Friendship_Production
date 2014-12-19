using UnityEngine;
using System.Collections;

public class PlayersEstablish : MonoBehaviour {
	public GameObject player1Spawn;
	public GameObject player2Spawn;

	void Awake()
	{
		if (Globals.Instance != null)
		{
			/* TODO check if players already exist in scene.*/
			PlayerInput player1 = null;
			PlayerInput player2 = null;
			GameObject[] characters = GameObject.FindGameObjectsWithTag("Converser");
			for (int i = 0; i < characters.Length; i++)
			{
				PlayerInput player = characters[i].GetComponent<PlayerInput>();
				if (player != null)
				{
					if (player.playerNumber == PlayerInput.Player.Player1)
					{
						player1 = player;
					}
					else if (player.playerNumber == PlayerInput.Player.Player2)
					{
						player2 = player;
					}
				}
			}
			

			// Place player 1.
			if (Globals.Instance.player1 != null)
			{
				if (player1 == null)
				{
					Globals.Instance.player1.transform.parent = player1Spawn.transform.parent;
					Globals.Instance.player1.transform.position = player1Spawn.transform.position;
					Globals.Instance.player1.transform.rotation = player1Spawn.transform.rotation;
					Globals.Instance.player1.transform.localScale = player1Spawn.transform.localScale;
				}
				else
				{
					Destroy(Globals.Instance.player1.gameObject);
					Globals.Instance.player1 = player1;
					player1.canvasPaused = Globals.Instance.canvasPaused;
				}
			}

			// Place player 2.
			if (Globals.Instance.player2 != null)
			{
				if (player2 == null)
				{
					Globals.Instance.player2.transform.parent = player2Spawn.transform.parent;
					Globals.Instance.player2.transform.position = player2Spawn.transform.position;
					Globals.Instance.player2.transform.rotation = player2Spawn.transform.rotation;
					Globals.Instance.player2.transform.localScale = player2Spawn.transform.localScale;
				}
				else
				{
					Destroy(Globals.Instance.player2.gameObject);
					Globals.Instance.player2 = player2;
					player2.canvasPaused = Globals.Instance.canvasPaused;
				}
			}
		}
	}
}

/* TODO why is menu being reopened?*/
