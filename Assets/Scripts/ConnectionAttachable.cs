using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConnectionAttachable : MonoBehaviour {
	public GameObject connectionPrefab;
	[SerializeField]
	public List<Connection> connections;

	public void AttachFluff(MovePulse pulse)
	{
		/*if (pulse != null && (absorbing || pulse.moving) && (pulse.attachee == null || !pulse.attachee.possessive))
		{
			if (pulse.creator != null && pulse.creator != pulseShot)
			{
				pulseShot.volleys = 1;
				pulseShot.volleyPartner = pulse.creator;
				if (pulse.creator.volleyPartner == pulseShot)
				{
					pulseShot.volleys = pulse.creator.volleys + 1;
				}
				if (pulseShot.volleys >= volleysToConnect)
				{
					bool connectionAlreadyMade = false;
					for (int i = 0; i < connections.Count && !connectionAlreadyMade; i++)
					{
						if ((connections[i].attachment1.partner == this && connections[i].attachment2.partner == pulse.creator.partnerLink) || (connections[i].attachment2.partner == this && connections[i].attachment1.partner == pulse.creator.partnerLink))
						{
							connectionAlreadyMade = true;
						}
					}
					if (!connectionAlreadyMade)
					{
						Connection newConnection = ((GameObject)Instantiate(connectionPrefab, Vector3.zero, Quaternion.identity)).GetComponent<Connection>();
						connections.Add(newConnection);
						pulse.creator.partnerLink.connections.Add(newConnection);
						newConnection.AttachPartners(pulse.creator.partnerLink, this);
						pulseShot.volleys = 0;
						pulse.creator.volleys = 0;
					}
				}

				SetFlashAndFill(pulse.creator.partnerLink.headRenderer.material.color);
				pulseShot.lastPulseAccepted = pulse.creator;
			}

			if (fluffsToAdd == null)
			{
				fluffsToAdd = new List<MovePulse>();
			}
			fluffsToAdd.Add(pulse);
		}*/
	}
}
