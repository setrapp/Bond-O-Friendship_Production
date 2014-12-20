using UnityEngine;
using System.Collections;

public class AutoConnection : MonoBehaviour {
	public ConnectionAttachable attachable1;
	public ConnectionAttachable attachable2;
	public GameObject connectionPrefab;
	public Vector3 connectionOffset1;
	public Vector3 connectionOffset2;
	public bool connectToPlayer;
	public PlayerInput.Player playerNumber;

	void Start()
	{
		if (connectToPlayer && Globals.Instance != null)
		{
			if (attachable1 == null)
			{
				if (playerNumber == PlayerInput.Player.Player1)
				{
					attachable1 = Globals.Instance.player1.GetComponent<ConnectionAttachable>();
				}
				else if (playerNumber == PlayerInput.Player.Player2)
				{
					attachable1 = Globals.Instance.player2.GetComponent<ConnectionAttachable>();
				}
			}
			if (attachable2 == null)
			{
				if (playerNumber == PlayerInput.Player.Player1)
				{
					attachable2 = Globals.Instance.player1.GetComponent<ConnectionAttachable>();
				}
				else if (playerNumber == PlayerInput.Player.Player2)
				{
					attachable2 = Globals.Instance.player2.GetComponent<ConnectionAttachable>();
				}
			}
		}

		if (attachable1 && attachable2 != null)
		{
			Connection newConnection = ((GameObject)Instantiate(connectionPrefab, Vector3.zero, Quaternion.identity)).GetComponent<Connection>();
			attachable1.connections.Add(newConnection);
			attachable2.connections.Add(newConnection);
			ConnectionStatsHolder statsHolder = GetComponent<ConnectionStatsHolder>();
			if (statsHolder != null && statsHolder.stats != null)
			{
				newConnection.stats = statsHolder.stats;
			}
			// TODO this should be able to happen in reverse order (pulling attachments is buggy).
			Vector3 attachPos1 = attachable1.transform.position + attachable1.transform.InverseTransformDirection(connectionOffset1);
			Vector3 attachPos2 = attachable2.transform.position + attachable2.transform.InverseTransformDirection(connectionOffset2);
			newConnection.AttachPartners(attachable1, attachPos1, attachable2, attachPos2);
		}
	}
}
