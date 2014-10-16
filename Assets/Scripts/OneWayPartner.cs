using UnityEngine;
using System.Collections;

public class OneWayPartner : MonoBehaviour {

	private PartnerLink partnerlink;
	private SimpleSeek seeker;
	public PartnerLink target;
	public bool followTarget = false;
	private bool following = false;
	public float followDistance;


	// Use this for initialization
	void Start () {
		partnerlink = GetComponent<PartnerLink>();
		seeker = GetComponent<SimpleSeek>();
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null)
		{
			if (followTarget && partnerlink.Partner == null)// && (transform.position - target.transform.position).sqrMagnitude <= Mathf.Pow(followDistance, 2))
			{
				partnerlink.SetPartner(target);
				following = true;
			}
			else if (following)
			{
				partnerlink.SetPartner(null);
				following = false;
			}

			if (followTarget && following)
				seeker.SeekPartner();
		}
	}
}
