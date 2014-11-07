using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleConnection : MonoBehaviour {
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

	void Update()
	{
		if (attachment1.partner != null || attachment2.partner != null)
		{
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
			GetComponent<Rigidbody>().MovePosition(midpoint);
			attachment1.lineRenderer.SetPosition(0, attachment1.position);
			attachment1.lineRenderer.SetPosition(1, midpoint);
			attachment2.lineRenderer.SetPosition(0, midpoint);
			attachment2.lineRenderer.SetPosition(1, attachment2.position);
			attachment1.lineRenderer.SetWidth(endsWidth, actualMidWidth);
			attachment2.lineRenderer.SetWidth(actualMidWidth, endsWidth);

			// Disconnect if too far apart.
			if (actualMidWidth <= 0)
			{
				//connected = false;
				attachment1.partner.connections.Remove(this);
				attachment2.partner.connections.Remove(this);
				Destroy(gameObject);
			}

			if (partnerDist < 1.5f)
			{
				Shield.SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				Shield.SendMessage("DeActivate", SendMessageOptions.DontRequireReceiver);
			}

			//bondCollider.transform.localScale = new Vector3(Vector3.Distance(attachment1.partner.transform.position, attachment2.partner.transform.position) - (attachment1.partner.transform.localScale.x + attachment2.partner.transform.localScale.x) * 0.65f, 0.5f, 10.0f);
			//bondCollider.transform.up = Vector3.Cross(attachment1.partner.transform.position - attachment2.partner.transform.position, Vector3.forward);
			((BoxCollider)GetComponent<Collider>()).size = new Vector3(Vector3.Distance(attachment1.partner.transform.position, attachment2.partner.transform.position) - (attachment1.partner.transform.localScale.x + attachment2.partner.transform.localScale.x) * 0.65f, 0.5f, 10.0f);
			transform.up = Vector3.Cross(attachment1.partner.transform.position - attachment2.partner.transform.position, Vector3.forward);
		}
	}

	public void AttachPartners(PartnerLink partner1, PartnerLink partner2)
	{
		Vector3 betweenPartners = (partner2.transform.position - partner1.transform.position).normalized;

		attachment1.partner = partner1;
		attachment1.position = transform.position + betweenPartners * partner1.transform.localScale.magnitude * attachPointDistance;

		attachment2.partner = partner2;
		attachment2.position = transform.position - betweenPartners * partner1.transform.localScale.magnitude * attachPointDistance;

		Color color1 = attachment1.partner.headRenderer.material.color;
		Color color2 = attachment2.partner.headRenderer.material.color;
		Color midColor = attachment1.partner.headRenderer.material.color + attachment2.partner.headRenderer.material.color;
		attachment1.lineRenderer.SetColors(color1, midColor);
		attachment2.lineRenderer.SetColors(midColor, color2);
	}
}

[System.Serializable]
public class PartnerAttachment
{
	public PartnerLink partner;
	public Vector3 position;
	public LineRenderer lineRenderer;
}
