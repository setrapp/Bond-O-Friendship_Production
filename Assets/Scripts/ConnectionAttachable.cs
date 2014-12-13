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
		if (pulse != null)
		{
			Connection newConnection = AttemptConnection(pulse.creator);
			if (newConnection != null)
			{
				//newConnection.
			}
		}
	}

	public Connection AttemptConnection(ConnectionAttachable connectionPartner)
	{
		Connection newConnection = null;
		if (connectionPartner == null)
		{
			return newConnection;
		}

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
				if (!connectionAlreadyMade)
				{
					newConnection = ((GameObject)Instantiate(connectionPrefab, Vector3.zero, Quaternion.identity)).GetComponent<Connection>();
					connections.Add(newConnection);
					connectionPartner.connections.Add(newConnection);
					newConnection.AttachPartners(connectionPartner, this);
					volleys = 0;
					connectionPartner.volleys = 0;
				}
			}
		}

		return newConnection;
	}
}
