using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConversationManager : MonoBehaviour
{
	private static ConversationManager instance;
	public static ConversationManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = GameObject.FindGameObjectWithTag("Globals").GetComponent<ConversationManager>();
			}
			return instance;
		}
	}
	[SerializeField]
	public List<Conversation> conversations;

	void Start()
	{
		if (conversations == null)
		{
			conversations = new List<Conversation>();
		}

		for (int i = 0; i < conversations.Count; i++)
		{
			DefineConversation(conversations[i]);
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	public void DefineConversation(Conversation conversation)
	{
		if (conversation.partner1 != null && conversation.partner2 != null)
		{
			conversation.initiateDistance = Mathf.Max(conversation.partner1.converseDistance, conversation.partner2.converseDistance);
			conversation.warningDistance = Mathf.Max(conversation.partner1.converseDistance * conversation.partner1.warningThreshold, conversation.partner2.converseDistance * conversation.partner2.warningThreshold);
			conversation.breakingDistance = Mathf.Max(conversation.partner1.converseDistance * conversation.partner1.breakingThreshold, conversation.partner2.converseDistance * conversation.partner2.breakingThreshold);
		}

	}

	public bool StartConversation(PartnerLink partner1, PartnerLink partner2)
	{
		// Find conversation and return false if not found or already in progress.
		Conversation startedConversation = FindConversation(partner1, partner2);
		if (startedConversation == null || startedConversation.inProgress)
		{
			return false;
		}

		// Start conversation and setup parameters.
		startedConversation.inProgress = true;

		partner1.SetPartner(partner2);
		partner2.SetPartner(partner1);
		if (startedConversation.partner1Leads)
		{
			startedConversation.partner1.SetLeading(true);
			startedConversation.partner2.SetLeading(false);
		}
		else
		{
			startedConversation.partner1.SetLeading(false);
			startedConversation.partner2.SetLeading(true);
		}

		return true;
	}

	public bool EndConversation(PartnerLink partner1, PartnerLink partner2)
	{
		// Find conversation and return false if not found or not in progress.
		Conversation endedConversation = FindConversation(partner1, partner2);
		if (endedConversation == null || !endedConversation.inProgress)
		{
			return false;
		}

		endedConversation.inProgress = false;
		endedConversation.partner1.SetPartner(null);
		endedConversation.partner1.SetLeading(false);
		endedConversation.partner2.SetPartner(null);
		endedConversation.partner2.SetLeading(false);

		return true;
	}

	public Conversation FindConversation(PartnerLink partner1, PartnerLink partner2)
	{
		Conversation foundConversation = null;
		for (int i = 0; i < conversations.Count && foundConversation == null; i++)
		{
			if ((conversations[i].partner1 == partner1 && conversations[i].partner2 == partner2) || (conversations[i].partner1 == partner2 && conversations[i].partner2 == partner1))
			{
				foundConversation = conversations[i];
			}
		}
		return foundConversation;
	}

	public Conversation[] FindConversations(PartnerLink participant)
	{
		List<Conversation> foundConversations = new List<Conversation>();
		for (int i = 0; i < conversations.Count; i++)
		{
			if (conversations[i].partner1 == participant || conversations[i].partner2 == participant)
			{
				foundConversations.Add(conversations[i]);
			}
		}
		Conversation[] conversationArray = new Conversation[foundConversations.Count];
		for (int i = 0; i < foundConversations.Count; i++)
		{
			conversationArray[i] = foundConversations[i];
		}
		return conversationArray;
	}
}

[System.Serializable]
public class Conversation
{
	public string title;
	public PartnerLink partner1;
	public PartnerLink partner2;
	public bool partner1Leads;
	[HideInInspector]
	public bool inProgress;
	[HideInInspector]
	public float initiateDistance;
	[HideInInspector]
	public float warningDistance;
	[HideInInspector]
	public float breakingDistance;
}