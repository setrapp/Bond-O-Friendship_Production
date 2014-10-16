using UnityEngine;
using System.Collections;

public class PartnerLink : MonoBehaviour {
	public bool isPlayer = false;
	private PartnerLink partner;
	public PartnerLink Partner
	{
		get { return partner; }
	}
	private Conversation conversation;
	public Conversation Conversation
	{
		get { return conversation; }
	}
	public Renderer headRenderer;
	public Renderer fillRenderer;
	private LineRenderer partnerLine;
	public float partnerLineSize = 0.25f;
	[HideInInspector]
	public SimpleMover mover;
	[HideInInspector]
	public Tracer tracer;
	[HideInInspector]
	public ConversingSpeed conversingSpeed;

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
		if (conversingSpeed == null)
		{
			conversingSpeed = GetComponent<ConversingSpeed>();
		}

		partnerLine = GetComponent<LineRenderer>();
	}
	
	void Update()
	{
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
			conversation = ConversationManager.Instance.FindConversation(this, partner);
			SendMessage("LinkPartner", SendMessageOptions.DontRequireReceiver);
		}
		else
		{
			conversation = null;
			SendMessage("UnlinkPartner", SendMessageOptions.DontRequireReceiver);
		}
	}
}
