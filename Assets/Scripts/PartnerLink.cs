using UnityEngine;
using System.Collections;

public class PartnerLink : MonoBehaviour {
	public bool isPlayer = false;
	public PartnerLink partner;
	public Renderer headRenderer;
	public Renderer fillRenderer;
	public LineRenderer partnerLine;
	public float partnerLineSize = 0.25f;
	[HideInInspector]
	public SimpleMover mover;
	[HideInInspector]
	public Tracer tracer;
	public SimpleConnection connection;
	public bool empty;

	void Awake()
	{
		if (mover == null)
		{
			mover = GetComponent<SimpleMover>();
		}
		if (tracer == null)
		{
			tracer = GetComponent<Tracer>();
		}
		if (partnerLine == null)
		{
			partnerLine = GetComponent<LineRenderer>();
		}

		fillRenderer.material.color = headRenderer.material.color;
	}
	
	void Update()
	{
		//float fillScale = Mathf.Clamp(1 - ((transform.position - partner.transform.position).magnitude / connection.maxDistance), 0, 1);
		//float maxDistance = (connection.distancePerDrain) * transform.localScale.magnitude;
		//float fillScale = Mathf.Clamp(1 - ((transform.position - partner.transform.position).magnitude / maxDistance), 0, 1);
		float fillScale = 1 - connection.drained;
		fillRenderer.transform.localScale = new Vector3(fillScale, fillScale, fillScale);
		empty = (fillScale <= 0);
		// Handle partners seperating.
		/*if (partner != null && conversation != null)
		{
			// Show that partners are close to separating.
			float sqrDist = (transform.position - partner.transform.position).sqrMagnitude;
			if (sqrDist > Mathf.Pow(conversation.breakingDistance, 2))
			{
				if (partnerLine != null)
				{
					partnerLine.SetVertexCount(0);
				}
				if (partner.partnerLine != null)
				{
					partner.partnerLine.SetVertexCount(0);
				}
				ConversationManager.Instance.EndConversation(this, partner);
			}
			else if (sqrDist > Mathf.Pow(conversation.warningDistance, 2))
			{
				if (partnerLine != null)
				{
					partnerLine.SetWidth(partnerLineSize, partnerLineSize);
					partnerLineAlteredSize = partnerLineSize;
					partnerLine.SetVertexCount(2);
					partnerLine.SetPosition(0, transform.position);
					partnerLine.SetPosition(1, partner.transform.position);
				}
			}
			else
			{
				if (partnerLine != null)
				{
					partnerLineAlteredSize *= partnerLineShrink;
					partnerLine.SetWidth(partnerLineAlteredSize, partnerLineAlteredSize);
					partnerLine.SetVertexCount(2);
					partnerLine.SetPosition(0, transform.position);
					partnerLine.SetPosition(1, partner.transform.position);
					if (partnerLineAlteredSize / partnerLineSize < (1 - partnerLineShrink))
					{
						partnerLine.SetVertexCount(0);
					}
				}
			}
		}*/	

	} //End of Update

	public void SetPartner(PartnerLink partner)
	{
		this.partner = partner;
		
		if (partner != null)
		{
			SendMessage("LinkPartner", SendMessageOptions.DontRequireReceiver);
		}
		else
		{
			SendMessage("UnlinkPartner", SendMessageOptions.DontRequireReceiver);
		}
	}
}
