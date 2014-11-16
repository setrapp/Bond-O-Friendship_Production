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
	public float addLinkDistance;
	public float removeLinkDistance;
	private float connectionLength;
	public float ConnectionLength
	{
		get
		{
			if (!lengthFresh)
			{
				connectionLength = 0;
				for(int i = 1; i < links.Count; i++)
				{
					connectionLength += (links[i].transform.position - links[i - 1].transform.position).magnitude;
				}
				lengthFresh = true;
			}
			return connectionLength;
		}
	}
	private bool lengthFresh = false;
	public float upOrderSpring = 1000;
	public float upOrderDamper = 10;
	public float downOrderSpring = 0;
	public float downOrderDamper = 0;


	void Update()
	{
		lengthFresh = false;

		if (attachment1.partner != null || attachment2.partner != null)
		{
			int oddLinkCount = (links.Count % 2 == 0) ? links.Count - 1 : links.Count;
			if (ConnectionLength < removeLinkDistance * oddLinkCount)
			{
				if (links.Count > 2)
				{
					RemoveLink();
					if (links.Count > 2 && links.Count % 2 == 0)
					{
						RemoveLink();
					}
				}
			}
			else if (ConnectionLength > addLinkDistance * (links.Count + 1))
			{
				AddLink();
				if (links.Count % 2 == 0)
				{
					AddLink();
				}
			}

			Vector3 linkDir = Vector3.zero;
			Vector3 linkScale = Vector3.zero;
			for (int i = 0; i < links.Count; i++)
			{
				linkDir = Vector3.zero;
				linkScale = links[i].transform.localScale;
				if (i == 0)
				{
					linkDir = links[i + 1].transform.position - links[i].transform.position;
					linkScale.y = (links[i + 1].transform.position - links[i].transform.position).magnitude;
					links[i].collider.center = new Vector3(0, 0.5f, 0);
				}
				else if (i == links.Count - 1)
				{
					linkDir = links[i].transform.position - links[i - 1].transform.position;
					linkScale.y = (links[i].transform.position - links[i - 1].transform.position).magnitude;
					links[i].collider.center = new Vector3(0, -0.5f, 0);
				}
				else
				{
					linkDir = links[i + 1].transform.position - links[i - 1].transform.position;
					float magFromPrevious = (links[i].transform.position - links[i - 1].transform.position).magnitude;
					float magToNext = (links[i + 1].transform.position - links[i].transform.position).magnitude;
					linkScale.y = magFromPrevious / 4 + magToNext / 4;
					links[i].collider.center = new Vector3(0, (magFromPrevious - magToNext) / 2, 0);
				}
				links[i].transform.up = linkDir;
				links[i].transform.localScale = linkScale;
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
			links[0].transform.position = attachment1.position;
			links[links.Count - 1].transform.position = attachment2.position;
			attachment1.lineRenderer.SetVertexCount(links.Count / 2 + 1);
			for (int i = 0; i < links.Count / 2; i++)
			{
				attachment1.lineRenderer.SetPosition(i, links[i].transform.position);
			}
			attachment2.lineRenderer.SetVertexCount(links.Count / 2 + 1);
			for (int i = 1; i < links.Count / 2 + 1; i++)
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
				Vector3 fakeMidpoint = (links[links.Count / 2].transform.position + links[links.Count / 2 -1].transform.position) / 2;
				attachment1.lineRenderer.SetPosition(links.Count / 2, fakeMidpoint);
				attachment2.lineRenderer.SetPosition(0, fakeMidpoint);
			}
			else
			{
				attachment1.lineRenderer.SetPosition(links.Count / 2, links[links.Count / 2].transform.position);
				attachment2.lineRenderer.SetPosition(0, links[links.Count / 2].transform.position);
			}
			
			attachment1.lineRenderer.SetWidth(endsWidth, actualMidWidth);
			attachment2.lineRenderer.SetWidth(actualMidWidth, endsWidth);

			// Disconnect if too far apart.
			if (actualMidWidth <= 0)
			{
				//connected = false;
				//BreakConnection();

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

		//links = new List<ConnectionLink>();
		//ConnectionLink startLink = ((GameObject)Instantiate(linkPrefab, attachment1.position, Quaternion.identity)).GetComponent<ConnectionLink>();
		//ConnectionLink endLink = ((GameObject)Instantiate(linkPrefab, attachment2.position, Quaternion.identity)).GetComponent<ConnectionLink>();
		//startLink.jointNext.connectedBody = endLink.body;
		//endLink.jointPrevious.connectedBody = startLink.body;
		
		//startLink.transform.parent = transform;
		//endLink.transform.parent = transform;

		//links.Add(startLink);
		//links.Add(endLink);
		links[0].transform.position = attachment1.position;
		links[1].transform.position = attachment2.position;
	}

	private void AddLink()
	{
		// Create new link in connection and add it to the center of the list.
		Vector3 midpoint = (links[links.Count / 2].transform.position + links[links.Count / 2 - 1].transform.position) / 2;
		ConnectionLink newLink = ((GameObject)Instantiate(linkPrefab, midpoint, Quaternion.identity)).GetComponent<ConnectionLink>();
		int index = links.Count / 2;
		newLink.transform.parent = transform;
		newLink.orderLevel = links.Count / 2;
		links.Insert(index, newLink);

		// Connect the new link to its neighbors.
		ConnectionLink previousLink = links[index - 1];
		ConnectionLink nextLink = links[index + 1];
		newLink.jointPrevious.connectedBody = previousLink.body;
		newLink.jointNext.connectedBody = nextLink.body;
		if (previousLink.jointNext != null)
		{
			previousLink.jointNext.connectedBody = newLink.body;
		}
		if (nextLink.jointNext != null)
		{
			nextLink.jointPrevious.connectedBody = newLink.body;
		}

		//RepositionLinks();
		WeightJoints();
	}

	private void RemoveLink()
	{
		// Remove the center link and connect its remaining neighbors together.
		int index = links.Count / 2;
		ConnectionLink previousLink = links[index - 1];
		ConnectionLink nextLink = links[index + 1];
		if (previousLink.jointNext != null)
		{
			previousLink.jointNext.connectedBody = nextLink.body;
		}
		if (nextLink.jointNext != null)
		{
			nextLink.jointPrevious.connectedBody = previousLink.body;
		}
		Destroy(links[index].gameObject);
		links.RemoveAt(index);

		//RepositionLinks();
		WeightJoints();
	}

	private void RepositionLinks()
	{
		float newLinkDistance = connectionLength / (links.Count - 1);
		for (int i = 1; i < links.Count - 1; i++)
		{
			Vector3 fromPrevious = (links[i].transform.position - links[i - 1].transform.position).normalized;
			//links[i].transform.position = links[i - 1].transform.position + (fromPrevious * newLinkDistance);
			links[i].jointPrevious.connectedAnchor = fromPrevious * newLinkDistance / 2;
			links[i].jointNext.connectedAnchor = -fromPrevious * newLinkDistance / 2;

			//links[i].jointPrevious.maxDistance = newLinkDistance;
			//links[i].jointNext.maxDistance = newLinkDistance;
		}
	}

	private void WeightJoints()
	{
		for (int i = 1; i < links.Count - 1; i++)
		{
			if (links[i].jointPrevious != null)
			{
				if (links[i].orderLevel < links[i - 1].orderLevel)
				{
					links[i].jointPrevious.spring = downOrderSpring;
					links[i].jointPrevious.damper = downOrderDamper;
				}
				else if (links[i].orderLevel == links[i - 1].orderLevel)
				{
					links[i].jointPrevious.spring = upOrderSpring / 2;
					links[i].jointPrevious.damper = upOrderDamper / 2;
				}
				else
				{
					links[i].jointPrevious.spring = upOrderSpring;
					links[i].jointPrevious.damper = upOrderDamper;
				}
			}
			if (links[i].jointNext != null)
			{
				if (links[i].orderLevel < links[i + 1].orderLevel)
				{
					links[i].jointNext.spring = downOrderSpring;
					links[i].jointNext.damper = downOrderDamper;
				}
				else if (links[i].orderLevel == links[i - 1].orderLevel)
				{
					links[i].jointNext.spring = upOrderSpring / 2;
					links[i].jointNext.damper = upOrderDamper / 2;
				}
				else
				{
					links[i].jointNext.spring = upOrderSpring;
					links[i].jointNext.damper = upOrderDamper;
				}
			}
		}
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
