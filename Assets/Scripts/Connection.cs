using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Connection : MonoBehaviour {
	public ConnectionAttachment attachment1;
	public ConnectionAttachment attachment2;
	public GameObject linkPrefab;
	public List<ConnectionLink> links;
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
	public ConnectionStats stats;

	void Update()
	{
		lengthFresh = false;

		if (attachment1.attachee != null || attachment2.attachee != null)
		{
			bool isCountEven = links.Count % 2 == 0;

			// Round the count of links down to an odd number.
			int oddLinkCount = (links.Count % 2 == 0) ? links.Count - 1 : links.Count;

			// If the connection is too short to require the current number of connections, remove some. (Attempt to keep count odd)
			if (ConnectionLength < stats.removeLinkDistance * oddLinkCount)
			{
				// Maintain the end points.
				if (links.Count > 2)
				{
					RemoveLink();
					// If the link count was not even before, it is now, so remove another to stay odd.
					if (links.Count > 2 && !isCountEven)
					{
						RemoveLink();
					}
				}
			}
			// If the connection length requires more connections, create some.
			else if (ConnectionLength > stats.addLinkDistance * (links.Count + 1))
			{
				AddLink();
				if (links.Count % 2 == 0)
				{
					AddLink();
				}
			}
			isCountEven = links.Count % 2 == 0;

			// Direct, scale, and place link colliders to cover the surface of the connection.
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
					links[i].linkCollider.center = new Vector3(0, 0.5f, 0);
				}
				else if (i == links.Count - 1)
				{
					linkDir = links[i].transform.position - links[i - 1].transform.position;
					linkScale.y = (links[i].transform.position - links[i - 1].transform.position).magnitude;
					links[i].linkCollider.center = new Vector3(0, -0.5f, 0);
				}
				else
				{
					linkDir = links[i + 1].transform.position - links[i - 1].transform.position;
					float magFromPrevious = (links[i].transform.position - links[i - 1].transform.position).magnitude;
					float magToNext = (links[i + 1].transform.position - links[i].transform.position).magnitude;
					linkScale.y = magFromPrevious / 4 + magToNext / 4;
					links[i].linkCollider.center = new Vector3(0, (magFromPrevious - magToNext) / 2, 0);
				}
				links[i].transform.up = linkDir;
				links[i].transform.localScale = linkScale;
			}

			// Base the width of the connection on how much has been drained beyond the partners' capacity.
			float warningDistance = stats.maxDistance * stats.relativeWarningDistance;
			float actualMidWidth = stats.midWidth * Mathf.Clamp(1 - ((connectionLength - warningDistance) / (stats.maxDistance - warningDistance)), 0, 1);

			// Place attachment points for each partner.
			Vector3 betweenPartners = (attachment2.position - attachment1.position).normalized;
			attachment1.position = attachment1.attachee.transform.position + attachment1.attachee.transform.TransformDirection(attachment1.offset);
			attachment2.position = attachment2.attachee.transform.position + attachment2.attachee.transform.TransformDirection(attachment2.offset);

			// Place attachment points with attached characters.
			links[0].transform.position = attachment1.position;
			links[links.Count - 1].transform.position = attachment2.position;

			// Draw lines between connection points.
			attachment1.lineRenderer.SetVertexCount(links.Count / 2 + 1);
			for (int i = 0; i < links.Count / 2; i++)
			{
				attachment1.lineRenderer.SetPosition(i, links[i].transform.position);
			}
			attachment2.lineRenderer.SetVertexCount(links.Count / 2 + 1);
			for (int i = 1; i < links.Count / 2 + 1; i++)
			{
				if (isCountEven)
				{
					attachment2.lineRenderer.SetPosition(i, links[i + links.Count / 2 - 1].transform.position);
				}
				else
				{
					attachment2.lineRenderer.SetPosition(i, links[i + links.Count / 2].transform.position);
				}
			}

			if (isCountEven)
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

			attachment1.lineRenderer.SetWidth(stats.endsWidth, actualMidWidth);
			attachment2.lineRenderer.SetWidth(actualMidWidth, stats.endsWidth);

			// Disconnect if too far apart.
			if (actualMidWidth <= 0)
			{
				BreakConnection();

			}
		}
	}
	public void BreakConnection()
	{
		ConnectionAttachable attachee1 = attachment1.attachee;
		ConnectionAttachable attachee2 = attachment2.attachee;
		if (attachee1 != null)
		{
			attachee1.connections.Remove(this);
			attachee1.SendMessage("ConnectionBroken", attachee2, SendMessageOptions.DontRequireReceiver);
		}
		if (attachee2 != null)
		{
			attachee2.connections.Remove(this);
			attachee2.SendMessage("ConnectionBroken", attachee1, SendMessageOptions.DontRequireReceiver);
		}
		Destroy(gameObject);
	}

	public void AttachPartners(ConnectionAttachable attachee1, Vector3 attachPoint1, ConnectionAttachable attachee2, Vector3 attachPoint2)
	{
		Vector3 betweenPartners = (attachee2.transform.position - attachee1.transform.position).normalized;

		attachment1.attachee = attachee1;
		attachment1.position = attachPoint1;
		attachment1.offset = attachee1.transform.InverseTransformDirection(attachPoint1 - attachee1.transform.position);

		attachment2.attachee = attachee2;
		attachment2.position = attachPoint2;
		attachment2.offset = attachee2.transform.InverseTransformDirection(attachPoint2 - attachee2.transform.position);

		Color color1 = attachment1.attachee.attachmentColor;
		Color color2 = attachment2.attachee.attachmentColor;
		Color midColor = color1 + color2;
		midColor.a = (color1.a + color2.a) / 2;
		attachment1.lineRenderer.SetColors(color1, midColor);
		attachment2.lineRenderer.SetColors(midColor, color2);

		links[0].transform.position = attachment1.position;
		links[1].transform.position = attachment2.position;

		links[0].jointPrevious.connectedBody = attachee1.GetComponent<Rigidbody>();
		links[0].jointNext.connectedBody = links[1].body;
		links[1].jointPrevious.connectedBody = links[0].body;
		links[1].jointNext.connectedBody = attachee2.GetComponent<Rigidbody>();

		WeightJoints();

		attachee1.SendMessage("ConnectionMade", attachee2, SendMessageOptions.DontRequireReceiver);
		attachee2.SendMessage("ConnectionMade", attachee1, SendMessageOptions.DontRequireReceiver);
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

		WeightJoints();
	}

	private void WeightJoints()
	{
		// Weight the strength of joints based on where links and neighbors exist in hierarchy.
		for (int i = 1; i < links.Count - 1; i++)
		{
			int prevLinkOrderLevel = (i > 0) ? links[i - 1].orderLevel : -1;
			int nextLinkOrderLevel = (i < links.Count - 1) ? links[i + 1].orderLevel : -1;

			if (links[i].jointPrevious != null)
			{
				if (links[i].orderLevel < prevLinkOrderLevel)
				{
					links[i].jointPrevious.spring = stats.downOrderSpring;
					links[i].jointPrevious.damper = stats.downOrderDamper;
				}
				else if (links[i].orderLevel == prevLinkOrderLevel)
				{
					links[i].jointPrevious.spring = stats.upOrderSpring / 2;
					links[i].jointPrevious.damper = stats.upOrderDamper / 2;
				}
				else
				{
					links[i].jointPrevious.spring = stats.upOrderSpring;
					links[i].jointPrevious.damper = stats.upOrderDamper;
				}
			}
			if (links[i].jointNext != null)
			{
				if (links[i].orderLevel < nextLinkOrderLevel)
				{
					links[i].jointNext.spring = stats.downOrderSpring;
					links[i].jointNext.damper = stats.downOrderDamper;
				}
				else if (links[i].orderLevel == nextLinkOrderLevel)
				{
					links[i].jointNext.spring = stats.upOrderSpring / 2;
					links[i].jointNext.damper = stats.upOrderDamper / 2;
				}
				else
				{
					links[i].jointNext.spring = stats.upOrderSpring;
					links[i].jointNext.damper = stats.upOrderDamper;
				}
			}
		}

		links[0].jointPrevious.spring = stats.attachSpring1;
		links[0].jointNext.spring = stats.attachSpring1;
		links[links.Count - 1].jointPrevious.spring = stats.attachSpring2;
		links[links.Count - 1].jointNext.spring = stats.attachSpring2;
	}
}

[System.Serializable]
public class ConnectionAttachment
{
	public ConnectionAttachable attachee;
	public Vector3 position;
	public Vector3 offset;
	public LineRenderer lineRenderer;
}

[System.Serializable]
public class ConnectionStats
{
	public float attachSpring1 = 0;
	public float attachSpring2 = 0;
	public float maxDistance = 25;
	public float relativeWarningDistance = 0.5f;
	public float endsWidth = 0.02f;
	public float midWidth = 0.5f;
	public float addLinkDistance = 0.5f;
	public float removeLinkDistance = 0.3f;
	public float upOrderSpring = 10000;
	public float upOrderDamper = 5;
	public float downOrderSpring = 5000;
	public float downOrderDamper = 0;
}
