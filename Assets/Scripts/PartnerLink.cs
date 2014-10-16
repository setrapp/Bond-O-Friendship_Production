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
	private bool leading;
	public bool Leading
	{
		get { return leading; }
	}
	public bool seekingPartner;
	public float seekingSlow = 0.25f;
	public float fadingTime = 3.0f;
	public bool fading = false;
	public Renderer headRenderer;
	public Renderer tailRenderer;
	public Renderer fillRenderer;
	public float converseDistance;
	public float warningThreshold;
	public float breakingThreshold;
	private LineRenderer partnerLine;
	public float partnerLineSize = 0.25f;
	private float partnerLineAlteredSize;
	public float partnerLineShrink = 0.9f;
	[HideInInspector]
	public SimpleMover mover;
	[HideInInspector]
	public Tracer tracer;
	[HideInInspector]
	public ConversationScore conversationScore;
	[HideInInspector]
	public ConversingSpeed conversingSpeed;
	public GameObject callout;
	private bool yielding;
	public bool Yielding
	{
		get { return yielding; }
		set 
		{
			if (value != yielding)
			{
				yielding = value; 
				if (yielding == true)
				{
					SendMessage("StartYielding", SendMessageOptions.DontRequireReceiver);
				}
				else
				{
					SendMessage("EndYielding", SendMessageOptions.DontRequireReceiver);
				}
			}
			
		}
	}
	public float startYieldProximity = 1;
	public float endYieldProximity = 2;
	public float yieldSpeedModifier = -0.5f;
	public float timeToOvertake = 3;
	public float timeToYield = 3;
	public bool inWake = false;
	public bool InWake
	{
		get { return inWake; }
		set
		{
			if (value != inWake)
			{
				inWake = value;
				if (inWake == true)
				{
					SendMessage("EnterWake", SendMessageOptions.DontRequireReceiver);
				}
				else
				{
					SendMessage("ExitWake", SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}

	//Points Related Values
	public bool linkBroken;
	public float timerTime = 5;
	public GameObject pointsGlobal = null;

	//Camera and Tail Related values
	public bool isLeadingnow;
	public float plpaDist = 0;
	public float plpaDist2 = 0;
	public bool isgaining;
	public bool islagging;
	// // // // // //

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
		if (conversationScore == null)
		{
			conversationScore = GetComponent<ConversationScore>();
		}
		if (conversingSpeed == null)
		{
			conversingSpeed = GetComponent<ConversingSpeed>();
		}
		
		partnerLine = GetComponent<LineRenderer>();
		
		pointsGlobal = GameObject.FindGameObjectWithTag("Global Points");
	}
	
	void Update()
	{
		// Fade out if needed.
		/*if (fading)
		{
			if (partner != null)
			{
				ConversationManager.Instance.EndConversation(this, partner);
			}
			seekingPartner = false;
			Color fadeColor = headRenderer.material.color;
			fadeColor.a -= Time.deltaTime / fadingTime;
			Color noColor = fadeColor;
			noColor.a = 0;
			//headRenderer.renderer.material.color = fadeColor;
			mover.maxSpeed -= mover.maxSpeed * (Time.deltaTime / fadingTime);
			mover.SlowDown();
			//tailRenderer.gameObject.SetActive(false);//.renderer.material.color = fadeColor;
			//tracer.DestroyLine();//.lineRenderer.SetColors(noColor, fadeColor);
			//tracer.maxVertices = 0;
		}*/

		// Find a partner.
		if (partner == null && seekingPartner)
		{
			// Find the nearest potential partner that could be conversed with.
			bool enableCallout = false;
			Conversation[] potentialConversations = ConversationManager.Instance.FindConversations(this);
			Conversation nearestConversation = null;
			float minSqrDist = -1;
			for (int i = 0; i < potentialConversations.Length; i++)
			{
				if ((potentialConversations[i].partner1 != null) && (potentialConversations[i].partner2 != null))
				{
					float sqrDist = (potentialConversations[i].partner1.transform.position - potentialConversations[i].partner2.transform.position).sqrMagnitude;
					if (sqrDist < minSqrDist || (minSqrDist < 0 && sqrDist <= Mathf.Pow(potentialConversations[i].breakingDistance, 2)))
					{
						nearestConversation = potentialConversations[i];
						minSqrDist = sqrDist;
					}
				}
			}

			// Determine if the potential partner is actually close enough to converse with.
			if (nearestConversation != null)
			{
				if (minSqrDist <= Mathf.Pow(nearestConversation.initiateDistance, 2) && nearestConversation.partner1.Partner == null && nearestConversation.partner2.Partner == null)
				{
					ConversationManager.Instance.StartConversation(nearestConversation.partner1, nearestConversation.partner2);
				}
				else
				{
					enableCallout = true;
				}
			}

			// Enable callout if needed.
			if (callout != null)
			{
				if (callout.activeInHierarchy != enableCallout)
				{
					callout.SetActive(enableCallout);
					if (enableCallout)
					{
						mover.externalSpeedMultiplier -= seekingSlow;
					}
					else
					{
						mover.externalSpeedMultiplier += seekingSlow;
					}
				}
			}
		}
		
		// Handle partners seperating.
		if (partner != null && conversation != null)
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
		}
		
		//Handles Time between Unlink Occurs and the Points are destroyed
		if(linkBroken == true)
		{
			if (timerTime > 0)
			{
				timerTime -= Time.deltaTime;
			}
			
			if (timerTime <= 0)
			{
				if (pointsGlobal != null)
				{
					pointsGlobal.SendMessage("DestoryPoints", SendMessageOptions.DontRequireReceiver);
				}
				conversation = null;
				linkBroken = false;
			}
			
			if (pointsGlobal != null)
			{
				pointsGlobal.SendMessage("PointsFade", SendMessageOptions.DontRequireReceiver);
			}
		}
		
		//if(timerTime <= 0)
		//{
		/*if (pointsGlobal != null)
			{
				pointsGlobal.SendMessage("UnlinkPartner", SendMessageOptions.DontRequireReceiver);
			}*/
		//	conversation = null;
		//	linkBroken = false;
		//}
		
		//Distance btw Player and Partner for Tail and Camera
		if(partner != null)
		{
			plpaDist = Vector3.Distance(transform.position, partner.transform.position);

			if(plpaDist < plpaDist2)
			{
			isgaining = true;
			islagging = false;
			//print("is gaining");
			}
			else if (plpaDist > plpaDist2)
			{
			isgaining = false;
			islagging = true;
			//print("is not gaining");
			}

			plpaDist2 = plpaDist;
		}

	} //End of Update
	
	void FixedUpdate()
	{
	}
	
	public void SetPartner(PartnerLink partner)
	{
		this.partner = partner;
		
		if (partner != null)
		{
			linkBroken = false;
			timerTime = 5;
			conversation = ConversationManager.Instance.FindConversation(this, partner);
			SendMessage("LinkPartner", SendMessageOptions.DontRequireReceiver);
			// Makes Alpha of All Objects in PointsGroup 1
			if (pointsGlobal != null)
			{
				pointsGlobal.SendMessage("PointsBright", SendMessageOptions.DontRequireReceiver);
			}
		}
		else
		{
			linkBroken = true;
			conversation = null;
			SendMessage("UnlinkPartner", SendMessageOptions.DontRequireReceiver);
		}
		
		
		
	}
	
	public void SetLeading(bool isLead, bool updatePartner = true)
	{
		leading = isLead;
		if (isLead)
		{
			if (conversation.partner1 == this)
			{
				conversation.partner1Leads = true;
				isLeadingnow = true;
			}
			else
			{
				conversation.partner1Leads = false;
			}
			SendMessage("StartLeading", SendMessageOptions.DontRequireReceiver);

			// Cast Points if Player
			if(isPlayer)
			{
				SendMessage("StartPoints", SendMessageOptions.DontRequireReceiver);
			}
			else if(partner != null && partner.isPlayer)
			{
				partner.SendMessage("CanCreatePoints",SendMessageOptions.DontRequireReceiver);
			}
		}
		else
		{
			SendMessage("EndLeading", SendMessageOptions.DontRequireReceiver);
			isLeadingnow = false;
		}
		
		if (partner != null && updatePartner)
		{
			partner.SetLeading(!isLead, false);
		}
	}
	
	public bool ShouldLead(PartnerLink leader)
	{
		Vector3 toLeader = leader.transform.position - transform.position;
		bool far = toLeader.sqrMagnitude >= Mathf.Pow(leader.startYieldProximity, 2);
		bool behind = Vector3.Dot(toLeader, leader.mover.velocity) >= 0;
		return !far || !behind;
	}
	
	public bool ShouldYield(PartnerLink leader)
	{
		Vector3 toLeader = leader.transform.position - transform.position;
		bool far = toLeader.sqrMagnitude >= Mathf.Pow(leader.startYieldProximity + endYieldProximity, 2);
		bool behind = Vector3.Dot(toLeader, leader.mover.velocity) >= 0;
		return !far || !behind;
	}
}
