using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConnectionAttachable : MonoBehaviour {
	public Color attachmentColor;
	public GameObject connectionPrefab;
	[SerializeField]
	public List<Connection> connections;
	public int volleysToConnect;
	public int volleys = 0;
	public ConnectionAttachable volleyPartner;

	public void AttemptConnection(ConnectionAttachable connectionPartner)
	{
		if (connectionPartner == null)
		{
			return;
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
					Connection newConnection = ((GameObject)Instantiate(connectionPrefab, Vector3.zero, Quaternion.identity)).GetComponent<Connection>();
					connections.Add(newConnection);
					connectionPartner.connections.Add(newConnection);
					newConnection.AttachPartners(connectionPartner, this);
					volleys = 0;
					connectionPartner.volleys = 0;
				}
			}
		}
	}
}
