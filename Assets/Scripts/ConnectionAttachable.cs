using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConnectionAttachable : MonoBehaviour {
	public bool handleFluffAttachment = true;
	public bool connectAtFluffPoint = true;
	public Color attachmentColor;
	public GameObject connectionPrefab;
	[SerializeField]
	public List<Connection> connections;
	public int volleysToConnect;
	public int volleys = 0;
	public ConnectionAttachable volleyPartner;

	public void AttachFluff(MovePulse pulse)
	{
		if (handleFluffAttachment && pulse != null)
		{
			AttemptConnection(pulse.creator, pulse.transform.position);
		}
	}

	public Connection AttemptConnection(ConnectionAttachable connectionPartner, Vector3 contactPosition, bool forceConnection = false)
	{
		Connection newConnection = null;
		if (connectionPartner == null || connectionPartner == this)
		{
			return newConnection;
		}

		if (connectionPartner.gameObject != gameObject)
		{
			volleys = 1;
			volleyPartner = connectionPartner;
			if (connectionPartner.volleyPartner == this)
			{
				volleys = connectionPartner.volleys + 1;
			}

			if (forceConnection || volleys >= volleysToConnect)
			{
				// If enough volleys have been passed, and the volleyers are not already connected, establish a new connection.
				if (!IsConnectionMade(connectionPartner))
				{
					Vector3 connectionPoint = transform.position;
					if (connectAtFluffPoint)
					{
						connectionPoint = contactPosition;
					}

					newConnection = ((GameObject)Instantiate(connectionPrefab, Vector3.zero, Quaternion.identity)).GetComponent<Connection>();
					connections.Add(newConnection);
					connectionPartner.connections.Add(newConnection);
					ConnectionStatsHolder statsHolder = GetComponent<ConnectionStatsHolder>();
					if (statsHolder != null && statsHolder.stats != null)
					{
						newConnection.stats = statsHolder.stats;
					}

					// TODO this should be able to happen in reverse order (pulling attachments is buggy).
					//newConnection.AttachPartners(this, connectionPoint, connectionPartner, connectionPartner.transform.position);
					newConnection.AttachPartners(connectionPartner, connectionPartner.transform.position, this, connectionPoint);
					volleys = 0;
					connectionPartner.volleys = 0;
				}
			}
		}

		return newConnection;
	}

	private void OnDestroy()
	{
		for (int i = 0; i < connections.Count; )
		{
			connections[i].BreakConnection();
		}
	}

	public bool IsConnectionMade(ConnectionAttachable partner)
	{
		if (partner == null)
		{
			return connections.Count > 0;
		}

		bool connectionAlreadyMade = false;
		for (int i = 0; i < connections.Count && !connectionAlreadyMade; i++)
		{
			if ((connections[i].attachment1.attachee == this && connections[i].attachment2.attachee == partner) || (connections[i].attachment2.attachee == this && connections[i].attachment1.attachee == partner))
			{
				connectionAlreadyMade = true;
			}
		}
		return connectionAlreadyMade;
	}
}
