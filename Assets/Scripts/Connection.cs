using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Connection : MonoBehaviour {
	public PartnerAttachment attachment1;
	public PartnerAttachment attachment2;
	public float minDistanceToDrain;
	public float distancePerDrain;
	public float scalePerPulse;
	public float drained = 0;
	public float endsWidth;
	public float midWidth;
	public float pulseSpeed;
	public float pulseAmplitude;
	public int nextFluctuationDirection = 1;
	public bool bouncePulseOnReject = false;
	public float partnerDist;
	public GameObject Shield;
	public float attachPointDistance = 0.2f;
	public GameObject bondCollider;
	public GameObject linkPrefab;
	public List<ConnectionLink> links;
	public float distancePerLink;


	void Update()
	{
		// TODO Move connections to attachpoints.

		if (attachment1.partner != null || attachment2.partner != null)
		{
			// TODO this should not check the distance between attachments but the total distance of the connection.
			float connectionSqrDist = (attachment1.position - attachment2.position).sqrMagnitude;
			if (connectionSqrDist < Mathf.Pow(distancePerLink * links.Count, 2))
			{
				// TODO if the connection is short enough maybe treat it as a single link
				if (links.Count > 2)
				{
					RemoveLink();
				}
			}
			else if (connectionSqrDist > Mathf.Pow(distancePerLink * (links.Count + 1), 2))
			{
				AddLink();
			}

			partnerDist = Vector3.Distance(attachment1.partner.transform.position, attachment2.partner.transform.position);
			Shield.transform.position = (attachment1.partner.transform.position + attachment2.partner.transform.position) * 0.5f;

			// Determine how much has been drained from the partners.
			float maxDistance = (distancePerDrain) * (Mathf.Min(attachment1.partner.transform.localScale.x, attachment2.partner.transform.localScale.x));
			float actualDrain = ((attachment2.position - attachment1.position).magnitude - minDistanceToDrain) / maxDistance;
			attachment1.partner.fillScale = Mathf.Clamp(attachment1.partner.fillScale - (actualDrain - drained), 0, 1);
			attachment2.partner.fillScale = Mathf.Clamp(attachment2.partner.fillScale - (actualDrain - drained), 0, 1);
			drained = Mathf.Clamp(actualDrain, 0, 1);

			// Base the width of the connection on how much has been drained beyond the partners' capacity.
			float actualMidWidth = midWidth * Mathf.Clamp(1 - (actualDrain - drained), 0, 1);

			// Place attachment points for each partner.
			Vector3 betweenPartners = (attachment2.position - attachment1.position).normalized;
			attachment1.position = attachment1.partner.transform.position + betweenPartners * attachment1.partner.transform.localScale.magnitude * attachPointDistance;
			attachment2.position = attachment2.partner.transform.position - betweenPartners * attachment2.partner.transform.localScale.magnitude * attachPointDistance;

			// Set connection points.
			Vector3 midpoint = (attachment1.position + attachment2.position) / 2;
			//transform.position = midpoint;
			//GetComponent<Rigidbody>().MovePosition(midpoint);
			links[0].transform.position = attachment1.position;
			links[links.Count - 1].transform.position = attachment2.position;
			attachment1.lineRenderer.SetVertexCount(links.Count / 2 + 1);
			for (int i = 0; i < links.Count / 2; i++)
			{
				//attachment1.lineRenderer.SetPosition(i, links[i].transform.position);
			}
			attachment2.lineRenderer.SetVertexCount(links.Count / 2 + 1);
			for (int i = 1; i < links.Count / 2; i++)
			{
				if (links.Count % 2 == 0)
				{
					attachment2.lineRenderer.SetPosition(i, links[i + links.Count / 2 - 1].transform.position);
				}
				else
				{
					attachment2.lineRenderer.SetPosition(i, links[i + links.Count / 2].transform.position);
				}
			}


			if (links.Count % 2 == 0)
			{
				attachment1.lineRenderer.SetPosition(links.Count / 2, midpoint);
				attachment2.lineRenderer.SetPosition(0, midpoint);
			}
			/*attachment1.lineRenderer.SetPosition(0, attachment1.position);
			attachment1.lineRenderer.SetPosition(1, midpoint);
			attachment2.lineRenderer.SetPosition(0, midpoint);
			attachment2.lineRenderer.SetPosition(1, attachment2.position);
			attachment1.lineRenderer.SetWidth(endsWidth, actualMidWidth);
			attachment2.lineRenderer.SetWidth(actualMidWidth, endsWidth);*/

			// Disconnect if too far apart.
			if (actualMidWidth <= 0)
			{
				//connected = false;
				BreakConnection();

			}

			if (partnerDist < 1.5f)
			{
				//Shield.SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				//Shield.SendMessage("DeActivate", SendMessageOptions.DontRequireReceiver);
			}

			//bondCollider.transform.localScale = new Vector3(Vector3.Distance(attachment1.partner.transform.position, attachment2.partner.transform.position) - (attachment1.partner.transform.localScale.x + attachment2.partner.transform.localScale.x) * 0.65f, 0.5f, 10.0f);
			//bondCollider.transform.up = Vector3.Cross(attachment1.partner.transform.position - attachment2.partner.transform.position, Vector3.forward);
			
			// TODO collider stuff
			//((BoxCollider)GetComponent<Collider>()).size = new Vector3(Vector3.Distance(attachment1.partner.transform.position, attachment2.partner.transform.position) - (attachment1.partner.transform.localScale.x + attachment2.partner.transform.localScale.x) * 0.65f, 0.5f, 10.0f);
			//transform.up = Vector3.Cross(attachment1.partner.transform.position - attachment2.partner.transform.position, Vector3.forward);

		}
	}
	public void BreakConnection()
	{
		attachment1.partner.connections.Remove(this);
		attachment2.partner.connections.Remove(this);
		Destroy(gameObject);
	}

	public void AttachPartners(PartnerLink partner1, PartnerLink partner2)
	{
		Vector3 betweenPartners = (partner2.transform.position - partner1.transform.position).normalized;

		attachment1.partner = partner1;
		attachment1.position = partner1.transform.position + betweenPartners * partner1.transform.localScale.magnitude * attachPointDistance;

		attachment2.partner = partner2;
		attachment2.position = partner2.transform.position - betweenPartners * partner1.transform.localScale.magnitude * attachPointDistance;

		Color color1 = attachment1.partner.headRenderer.material.color;
		Color color2 = attachment2.partner.headRenderer.material.color;
		Color midColor = attachment1.partner.headRenderer.material.color + attachment2.partner.headRenderer.material.color;
		attachment1.lineRenderer.SetColors(color1, midColor);
		attachment2.lineRenderer.SetColors(midColor, color2);

		links = new List<ConnectionLink>();
		Debug.Log(attachment1.position + " " + attachment2.position);
		ConnectionLink startLink = ((GameObject)Instantiate(linkPrefab, attachment1.position, Quaternion.identity)).GetComponent<ConnectionLink>();
		ConnectionLink endLink = ((GameObject)Instantiate(linkPrefab, attachment2.position, Quaternion.identity)).GetComponent<ConnectionLink>();
		Debug.Log(startLink.transform.position);
		startLink.jointNext.connectedBody = endLink.body;
		endLink.jointPrevious.connectedBody = startLink.body;

		links.Add(startLink);
		links.Add(endLink);
	}

	private void AddLink()
	{
		// Create new link in connection and add it to the center of the list.
		ConnectionLink newLink = ((GameObject)Instantiate(linkPrefab, attachment1.position + ((attachment2.position - attachment1.position) / 2), Quaternion.identity)).GetComponent<ConnectionLink>();
		int index = links.Count / 2;
		links.Insert(index, newLink);

		// Connect the new link to its neighbors.
		ConnectionLink previousLink = links[index - 1];
		ConnectionLink nextLink = links[index + 1];
		newLink.jointPrevious.connectedBody = previousLink.body;
		newLink.jointNext.connectedBody = nextLink.body;
		previousLink.jointNext.connectedBody = newLink.body;
		nextLink.jointPrevious.connectedBody = newLink.body;
	}

	private void RemoveLink()
	{
		// Remove the center link and connect its remaining neighbors together.
		int index = links.Count / 2;
		ConnectionLink previousLink = links[index - 1];
		ConnectionLink nextLink = links[index + 1];
		previousLink.jointNext.connectedBody = nextLink.body;
		nextLink.jointNext.connectedBody = previousLink.body;
		links.RemoveAt(index);
	}

	void OnTriggerEnter(Collider collide)
	{
		if(collide.gameObject.tag == "enemyPulse")
		{
			//print(collide.gameObject.tag + " " + collide.gameObject.name);
			BreakConnection();
		}
	}
}

[System.Serializable]
public class PartnerAttachment
{
	public PartnerLink partner;
	public Vector3 position;
	public LineRenderer lineRenderer;
}
