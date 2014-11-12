using UnityEngine;
using System.Collections;

public class MovePulse : MonoBehaviour {
	public bool passed = false;
	public PulseShot creator;
	public PartnerLink volleyTarget;
	public int volleys;
	public float capacity;
	public Vector3 target;
	private float moveSpeed = 2;
	public float rotationSpeed = 50.0f;
	//public GameObject pulseCreator;
	public PulseShot volleyPartner;
	public TrailRenderer trail;
	public bool moving = false;
	public float baseAngle = -1;
	public Vector3 baseDirection;
	public Animation swayAnimation;
	private bool disableColliders;
	public Vector3 oldBulbPos;
	public GameObject bulb;

	void Start ()
	{
		//pulseCreator = GameObject.Find("Globals");
		oldBulbPos = bulb.transform.position;
	}

	// Update is called once per frame
	void Update () 
	{
		if (disableColliders)
		{
			Collider[] colliders = GetComponentsInChildren<Collider>();
			for (int i = 0; i < colliders.Length; i++)
			{
				colliders[i].enabled = false;
			}
			disableColliders = false;
		}

		if (passed && moving)
		{
			Vector3 direction = target - transform.position;
			if (direction.sqrMagnitude > Mathf.Pow(moveSpeed * Time.deltaTime, 2))
			{
				direction.z = 0;
				float distance = direction.magnitude;

				float decelerationFactor = distance / 1.5f;

				float speed = moveSpeed * decelerationFactor;

				//transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
				transform.Rotate(rotationSpeed*Time.deltaTime,0.0f,0.0f);

				Vector3 moveVector = direction.normalized * Time.deltaTime * speed;
				transform.position += moveVector;
			}
			else
			{
				moving = false;

				RaycastHit attachInfo;
				if (Physics.Raycast(transform.position, Vector3.forward, out attachInfo, Mathf.Infinity))
				{
					//Debug.Log(attachInfo.collider.gameObject.name);
					//transform.parent = attachInfo.collider.transform;
				}
				transform.rotation = Quaternion.Euler(270, 0, 0);

				if (swayAnimation != null)
				{
					swayAnimation.enabled = true;
				}
			}
		}
	}

	public void ReadyForPass()
	{
		trail.gameObject.SetActive(true);
		Collider[] colliders = GetComponentsInChildren<Collider>();
		for (int i = 0; i < colliders.Length; i++)
		{
			colliders[i].enabled = true;
		}
		passed = true;
		moving = true;
		baseAngle = -1;
	}

	public void EndPass()
	{
		trail.gameObject.SetActive(false);
		disableColliders = true;
		passed = false;
		moving = false;
		creator = null;
		volleyTarget = null;
		volleys = 0;
		capacity = 0;
		volleyPartner = null;
	}

	void OnTriggerEnter(Collider collide)
	{
		if (!collide.isTrigger && collide.gameObject.tag == "Converser")
		{
			collide.gameObject.GetComponent<PartnerLink>().AttachFluff(this);
		}

		/*if (passed && moving)
		{
			if (collide.gameObject.tag == "Pulse")
			{
				MovePulse otherPulse = collide.GetComponent<MovePulse>();
				if (otherPulse.passed && otherPulse.moving)
				{
					if (otherPulse != null && otherPulse.creator != creator)
					{
						if (creator != null && creator.name == "Player 1")
						{
							PulseCombo pulseCombo = pulseCreator.GetComponent<PulseCombo>();
							pulseCombo.pulseOne = true;
							pulseCombo.pulseOnePos = transform.position;
							pulseCombo.p1Cap = capacity;
							pulseCombo.p1Quat = Quaternion.identity;
							pulseCombo.p1For = Vector3.forward;
							pulseCombo.p1scale = transform.localScale;
							pulseCombo.p1Targ = target;
						}
						if (creator != null && creator.name == "Player 2")
						{
							PulseCombo pulseCombo = pulseCreator.GetComponent<PulseCombo>();
							pulseCombo.pulseTwo = true;
							pulseCombo.pulseTwoPos = transform.position;
							pulseCombo.p2Cap = capacity;
							pulseCombo.p2Quat = Quaternion.identity;
							pulseCombo.p2For = Vector3.forward;
							pulseCombo.p2scale = transform.localScale;
							pulseCombo.p2Targ = target;
						}

						Destroy(gameObject);
					}
				}
			}
		}*/
	}
}
