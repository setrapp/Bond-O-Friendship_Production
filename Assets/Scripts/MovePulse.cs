using UnityEngine;
using System.Collections;

public class MovePulse : MonoBehaviour {

	public PulseShot creator;
	public PartnerLink volleyTarget;
	public int volleys;
	public float capacity;
	public Vector3 target;
	private float moveSpeed = 2;
	public GameObject pulseCreator;
	public PulseShot volleyPartner;

	void Start ()
	{
		pulseCreator = GameObject.Find("Globals");
	}

	// Update is called once per frame
	void Update () {

			Vector3 direction = target - transform.position;
			direction.z = 0;
			float distance = direction.magnitude;
			
			float decelerationFactor = distance / 1.5f;
			
			float speed = moveSpeed * decelerationFactor;
			
			//transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);

			Vector3 moveVector = direction.normalized * Time.deltaTime * speed;
			transform.position += moveVector;
	
	}
	void OnTriggerEnter(Collider collide)
	{
		if(collide.gameObject.tag == "Pulse")
		{
			if(creator != null && creator.name == "Player 1")
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
