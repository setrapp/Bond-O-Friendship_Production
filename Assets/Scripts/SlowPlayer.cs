using UnityEngine;
using System.Collections;

public class SlowPlayer : MonoBehaviour {

	private bool slowing = false;
	private SimpleMover targetMover;
	private WaypointSeek seeker;
	private PartnerLink partnerLink;
	private static int otherPartners = 0;
	private float decayRate = 0.9f;
	public float slowRate = 0.2f;
	public float slowDistance;
	private int targetTransgressions = 0;
	public int transgressionThreshold;

	// Use this for initialization
	void Start () {
		partnerLink = GetComponent<PartnerLink>();
		seeker = GetComponent<WaypointSeek>();
	}
	
	// Update is called once per frame
	void Update () {
		if(seeker == null || targetMover == null)
			return;
		if(seeker.orbit == true && (transform.position - targetMover.transform.position).sqrMagnitude <= Mathf.Pow(slowDistance, 2))
		{
			if (slowing == false)
			{
				slowing = true;
				targetMover.externalSpeedMultiplier -= slowRate * Mathf.Pow(decayRate, otherPartners);
				otherPartners++;
			}
		}
		else if (slowing == true)
		{
			slowing = false;
			targetMover.externalSpeedMultiplier += slowRate * Mathf.Pow(decayRate, otherPartners);
			otherPartners--;
		}
	}

	void LinkPartner(){
		if (partnerLink != null && partnerLink.Partner != null)
		{
			targetMover = partnerLink.Partner.GetComponent<SimpleMover>();
		}
	}

	public void RespondTransgression()
	{
		targetTransgressions++;
		if (targetTransgressions >= transgressionThreshold)
		{
			OneWayPartner follow = GetComponent<OneWayPartner>();
			if (follow != null)
			{
				follow.followTarget = true;
			}
			WaypointSeek seek = GetComponent<WaypointSeek>();
			if (seek != null)
			{
				seek.orbit = true;
			}
		}
	}
}
