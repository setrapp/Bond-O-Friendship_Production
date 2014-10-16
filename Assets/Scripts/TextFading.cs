using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextFading : MonoBehaviour {
	
	public Text text;
	private float alpha = 0;
	public PartnerLink partnerLink;
	public PartnerLink player;
	private Conversation conversation;
	private bool convoStart = false;

	// Use this for initialization
	void Awake () 
	{
		if (partnerLink == null)
		{
			partnerLink = GetComponent<PartnerLink>();
		}
		if (text == null)
		{
			text = GameObject.FindGameObjectWithTag("ConversationTitle").GetComponent<Text>();
		}
		if (player == null && text != null)
		{
			Transform maybePlayer = text.transform;
			while (maybePlayer.tag != "Converser" && maybePlayer != transform.root)
			{
				maybePlayer = maybePlayer.parent;
			}
			if (maybePlayer.tag == "Converser")
			{
				player = maybePlayer.GetComponent<PartnerLink>();
			}
		}

		if (text != null)
		{
			text.color = new Color(1f, 0f, 1f, 0);
			text.text = "";
		}
	}

	void Start()
	{
		conversation = ConversationManager.Instance.FindConversation(partnerLink, player);
	}
	
	// Update is called once per frame
	void Update () {

		if(player != null)
		{
			if (conversation != null)
			{
				var distance = Vector3.Distance(player.transform.position, transform.position);

				if (text != null)
				{
					if (!convoStart)
					{
						alpha = Mathf.Clamp(1 - (distance / (conversation.warningDistance)), 0, 1);
						if (distance <= conversation.initiateDistance)
						{
							convoStart = true;
							text.text = conversation.title;
						}
						else if (distance <= (conversation.warningDistance))
						{
							text.color = new Color(1f, 0f, 1f, alpha);
							text.text = conversation.title;
						}
						
						else
						{
							text.text = "";
						}
					}

					if (alpha > 0 && convoStart)
					{
						alpha = Mathf.Max(alpha - Time.deltaTime, 0);
						text.color = new Color(1f, 0.92f, 0.016f, alpha);
					}
				}
			}
		}
		
	}

	void UnlinkPartner()
	{
		convoStart = false;
	}
}
