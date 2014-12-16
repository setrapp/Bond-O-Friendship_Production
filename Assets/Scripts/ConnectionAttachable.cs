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
			AttemptConnection(pulse);
		}
	}

	public Connection AttemptConnection(MovePulse connectionFluff)
	{
		Connection newConnection = null;
		if (connectionFluff == null || connectionFluff.creator == null || connectionFluff.creator == this)
		{
			return newConnection;
		}
		ConnectionAttachable connectionPartner = connectionFluff.creator;

		if (connectionPartner != gameObject)
		{
			volleys = 1;
			volleyPartner = connectionPartner;
			if (connectionPartner.volleyPartner == this)
			{
				volleys = connectionPartner.volleys + 1;
			}
			if (volleys >= volleysToConnect)
			{
				bool connectionAlreadyMade = false;
				for (int i = 0; i < connections.Count && !connectionAlreadyMade; i++)
				{
					if ((connections[i].attachment1.attachee == this && connections[i].attachment2.attachee == connectionPartner) || (connections[i].attachment2.attachee == this && connections[i].attachment1.attachee == connectionPartner))
					{
						connectionAlreadyMade = true;
					}
				}

				// If enough volleys have been passed, and the volleyers are not already connected, establish a new connection.
				if (!connectionAlreadyMade)
				{
					Vector3 connectionPoint = transform.position;
					if (connectAtFluffPoint)
					{
						connectionPoint = connectionFluff.transform.position;
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
}
