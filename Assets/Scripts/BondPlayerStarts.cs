using UnityEngine;
using System.Collections;

public class BondPlayerStarts : MonoBehaviour {
	public AutoBond player1Start;
	public AutoBond player2Start;

	private void PlayersPlaced()
	{
		if (player1Start != null)
		{
			player1Start.CreateBond();
		}
		if (player2Start != null)
		{
			player2Start.CreateBond();
		}
	}
}
