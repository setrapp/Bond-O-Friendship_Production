using UnityEngine;
using System.Collections;

public class AutoBond : MonoBehaviour {
	public BondAttachable attachable1;
	public BondAttachable attachable2;
	public GameObject bondPrefab;
	public Vector3 bondOffset1;
	public Vector3 bondOffset2;
	public bool bondToPlayer;
	public PlayerInput.Player playerNumber;

	void Start()
	{
		if (bondToPlayer && Globals.Instance != null)
		{
			if (attachable1 == null)
			{
				if (playerNumber == PlayerInput.Player.Player1)
				{
					attachable1 = Globals.Instance.player1.GetComponent<BondAttachable>();
				}
				else if (playerNumber == PlayerInput.Player.Player2)
				{
					attachable1 = Globals.Instance.player2.GetComponent<BondAttachable>();
				}
			}
			if (attachable2 == null)
			{
				if (playerNumber == PlayerInput.Player.Player1)
				{
					attachable2 = Globals.Instance.player1.GetComponent<BondAttachable>();
				}
				else if (playerNumber == PlayerInput.Player.Player2)
				{
					attachable2 = Globals.Instance.player2.GetComponent<BondAttachable>();
				}
			}
		}

		if (attachable1 && attachable2 != null)
		{
			Bond newBond = ((GameObject)Instantiate(bondPrefab, Vector3.zero, Quaternion.identity)).GetComponent<Bond>();
			attachable1.bonds.Add(newBond);
			attachable2.bonds.Add(newBond);
			BondStatsHolder statsHolder = GetComponent<BondStatsHolder>();
			if (statsHolder != null && statsHolder.stats != null)
			{
				newBond.stats = statsHolder.stats;
			}

			Vector3 attachPos1 = attachable1.transform.position + attachable1.transform.InverseTransformDirection(bondOffset1);
			Vector3 attachPos2 = attachable2.transform.position + attachable2.transform.InverseTransformDirection(bondOffset2);
			newBond.AttachPartners(attachable1, attachPos1, attachable2, attachPos2);
		}
	}
}
