using UnityEngine;
using System.Collections;

public class AutoBond : MonoBehaviour {
	public Bond createdBond;
	public bool createOnStart = true;
	public BondAttachable attachable1;
	public BondAttachable attachable2;
	public GameObject bondPrefab;
	public Vector3 bondOffset1;
	public Vector3 bondOffset2;
	public BondStatsHolder bondOverrideStats;
	public bool bondToPlayer;
	public PlayerInput.Player playerNumber;

	void Start()
	{
		if (createOnStart)
		{
			CreateBond();
		}
	}

	public virtual void CreateBond()
	{
		if (createdBond != null)
		{
			return;
		}

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
			createdBond = ((GameObject)Instantiate(bondPrefab, Vector3.zero, Quaternion.identity)).GetComponent<Bond>();
			attachable1.bonds.Add(createdBond);
			attachable2.bonds.Add(createdBond);
			if (bondOverrideStats == null)
			{
				bondOverrideStats = GetComponent<BondStatsHolder>();
			}
			if (bondOverrideStats != null && bondOverrideStats.stats != null)
			{
				createdBond.stats.Overwrite(bondOverrideStats.stats, true);
			}
			Vector3 attachPos1 = attachable1.transform.position + attachable1.transform.InverseTransformDirection(bondOffset1);
			Vector3 attachPos2 = attachable2.transform.position + attachable2.transform.InverseTransformDirection(bondOffset2);
			createdBond.AttachPartners(attachable1, attachPos1, attachable2, attachPos2);
		}
	}
}
