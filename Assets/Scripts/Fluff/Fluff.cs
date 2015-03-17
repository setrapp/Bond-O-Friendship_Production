using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SimpleMover))]
public class Fluff : MonoBehaviour {
	[HideInInspector]
	private SimpleMover mover;
	public BondAttachable creator;
	public float rotationSpeed = 50.0f;
	public TrailRenderer trail;
	public bool moving = false;
	public float baseAngle = -1;
	public Vector3 baseDirection;
	public Animation swayAnimation;
	private bool disableColliders;
	public Vector3 oldBulbPos;
	public MeshRenderer bulb;
	public MeshRenderer stalk;
	public GameObject geometry;
	public LineRenderer lineToAttractor;
	[HideInInspector]
	public CapsuleCollider hull;
	[HideInInspector]
	public Rigidbody body;
	public float attacheePullRate = 1;
	public Attachee attachee;
	private Vector3 attachPoint;
	public GameObject ignoreCollider;
	public float nonAttractTime;
	public bool attractable = true;
	private bool forgetCreator;
	public Animation popAnimation;
	public Vector3 pullForce;
	public float pullDistance;

	void Awake()
	{
		oldBulbPos = bulb.transform.position;
		if (hull == null)
		{
			hull = GetComponent<CapsuleCollider>();
		}
		if (body == null)
		{
			body = GetComponent<Rigidbody>();
		}
		mover = GetComponent<SimpleMover>();

		if (attachee != null && attachee.gameObject != null)
		{
			Attach(attachee.gameObject, transform.position, transform.up, true);
		}
		else
		{
			attachee = null;
		}

		// Keep track of this fluff for easy retrieval.
		if (Globals.Instance != null && Globals.Instance.allFluffs != null)
		{
			Globals.Instance.allFluffs.Add(this);
		}

		//TODO This fixes a unity tag changing bug that was fixed in a newer version of unity 5
		gameObject.tag = "Fluff";

		if (lineToAttractor != null)
		{
			lineToAttractor.SetVertexCount(0);
		}
	}

	// Update is called once per frame
	void Update() 
	{
		if(forgetCreator)
		{
			creator = null;
			forgetCreator = false;
		}

		if (pullForce.sqrMagnitude > 0)
		{
			ApplyPullForce();
			pullForce = Vector3.zero;
			pullDistance = 0;
		}
		else
		{
			if (lineToAttractor != null)
			{
				lineToAttractor.SetVertexCount(0);
			}
		}

		if (!attractable && nonAttractTime <= 0)
		{
			attractable = true;
			nonAttractTime = 0;
		}
		else if (nonAttractTime > 0)
		{
			nonAttractTime -= Time.deltaTime;
			attractable = false;
		}

		// If attachee is not controlling movement, reposition and reorient to stay constant in relation to it.
		if (attachee != null && !attachee.controlling)
		{
			if (attachee.gameObject == null)
			{
				Destroy(gameObject);
			}
			else
			{
				transform.position = attachee.gameObject.transform.position + attachee.gameObject.transform.TransformDirection(attachee.attachPoint);
				transform.up = attachee.gameObject.transform.TransformDirection(baseDirection);
			}
		}

		if (moving != mover.Moving)
		{
			if (!mover.Moving)
			{
				RaycastHit attachInfo;
				if (Physics.Raycast(transform.position, Vector3.forward, out attachInfo, Mathf.Infinity))
				{
					Attach(attachInfo.collider.gameObject, transform.position, -Vector3.forward);
				}
				else
				{
					PopFluff();
				}
				trail.gameObject.SetActive(false);
			}
			else
			{
				attachee = null;

				// If fluff is pointing more in the z direction than the other directions, rotate into the correct plane.
				if(Mathf.Pow(transform.up.z, 2) > new Vector2(transform.up.x, transform.up.y).sqrMagnitude)
				{
					transform.up = -mover.velocity;
				}
				ToggleSwayAnimation(false);
				trail.gameObject.SetActive(true);
				baseAngle = -1;
			}

			moving = mover.Moving;
		}

		if (moving)
		{
			if (ignoreCollider == null && hull.isTrigger)
			{
				hull.isTrigger = false;
			}

			transform.Rotate(0.0f, 0.0f, rotationSpeed * Time.deltaTime);
		}
		else
		{
			// Sprouting.
			if (!Globals.Instance.fluffsThrowable && Vector3.Dot(geometry.transform.localPosition, Vector3.up) < 0)
			{
				geometry.transform.localPosition += Vector3.up * Globals.Instance.fluffLeaveEmbed / Globals.Instance.fluffLeaveAttractWait * Time.deltaTime;
				if (Vector3.Dot(geometry.transform.localPosition, Vector3.up) >= 0)
				{
					geometry.transform.localPosition = Vector3.zero;
					FluffStick attacheeStick = null;
					if (attachee != null && attachee.gameObject != null)
					{
						attacheeStick = attachee.gameObject.GetComponent<FluffStick>();
					}
					if (attacheeStick == null || attacheeStick.allowSway)
					{
						ToggleSwayAnimation(true);
					}
				}
			}
		}

		
	}

	public void Pass(Vector3 passForce, GameObject ignoreColliderTemporary = null, float preventAttractTime = 0)
	{
		attachee = null;
		nonAttractTime = preventAttractTime;
		attractable = false;

		// If something attachable is already in reach, attach without moving.
		RaycastHit attemptPassHit;
		float blockingTestDistance = Mathf.Max(hull.height, hull.radius);
		ignoreCollider = ignoreColliderTemporary;
		bool blocked = TestForBlocking(passForce, blockingTestDistance, out attemptPassHit);
		if (blocked)
		{
			moving = true;
			Attach(attemptPassHit.collider.gameObject, attemptPassHit.point, attemptPassHit.normal, true);
			return;
		}

		// Allow fluff to act on physical objects.
		if (body != null)
		{
			body.isKinematic = false;
		}

		float passForceMag = passForce.magnitude;
		mover.Move(passForce / passForceMag, passForceMag * Time.deltaTime, false);
	}

	public void Pull(GameObject puller, Vector3 pullOffset, float pullMagnitude)
	{
		Pull(puller, pullOffset, pullMagnitude, Color.white);
	}

	public void Pull(GameObject puller, Vector3 pullOffset, float pullMagnitude, Color pullColor)
	{
		// If something is blocking the path to the puller, do not move.
		RaycastHit attemptPullHit;
		Vector3 toPuller = (puller.transform.position + pullOffset) - transform.position;
		float toPullerDist = toPuller.magnitude;
		toPuller /= toPullerDist;
		bool blocked = TestForBlocking(toPuller, toPullerDist, out attemptPullHit, false, puller);
		if (blocked)
		{
			return;
		}

		// Compute new pull force to be compared to existing pull force.
		Vector3 newPullForce = toPuller * pullMagnitude;
		float newPullDistance = toPullerDist + pullOffset.magnitude;

		// If the new pull force over distance is greater than the existing pull force over distance, use the new.
		float distEpsilon = 0.00001f;
		if ((newPullForce.magnitude / (newPullDistance + distEpsilon)) > (pullForce.magnitude / (pullDistance + distEpsilon)))
		{
			pullForce = newPullForce;
			pullDistance = newPullDistance;

			// Draw line from attractor to fluff.
			if (lineToAttractor != null)
			{
				lineToAttractor.SetVertexCount(2);
				lineToAttractor.SetPosition(0, puller.transform.position + pullOffset);
				lineToAttractor.SetPosition(1, transform.position);
				lineToAttractor.material.color = pullColor;
			}
		}
	}

	private void ApplyPullForce()
	{
		if (attachee != null && attachee.attachInfo != null && attachee.attachInfo.pullableBody != null)
		{
			attachee.attachInfo.AddPullForce(pullForce, transform.position);
		}
		else
		{
			if (body != null)
			{
				body.isKinematic = false;
			}
			mover.Accelerate(pullForce, true, false, true);
		}
	}

	public void Attach(GameObject attacheeObject, Vector3 position, Vector3 standDirection, bool sway = true)
	{
		if (Globals.Instance == null)
		{
			return;
		}

		// If no potential attachee is given, disregard.
		if (attacheeObject == null)
		{
			return;
		}

		// If already attached to a possessive attachee, do not attempt to attach.
		if (attachee != null && attachee.possessive)
		{
			return;
		}

		FluffStick attacheeStick = attacheeObject.GetComponent<FluffStick>();

		// Position and orient.
		transform.position = position;
		transform.up = standDirection;

		// Actaully attach to target and record relationship to attachee.
		Vector3 attachPoint = attacheeObject.transform.InverseTransformDirection(position - attacheeObject.transform.position);
		attachee = new Attachee(attacheeObject, attacheeStick, attachPoint, false, false);
		baseDirection = attacheeObject.transform.InverseTransformDirection(standDirection);
		ignoreCollider = attacheeObject;
		if (Globals.Instance.fluffsThrowable)
		{
			nonAttractTime = 0;
			attractable = true;
		}

		// Notify the potential attachee that fluff has been attached.
		attacheeObject.SendMessage("AttachFluff", this, SendMessageOptions.DontRequireReceiver);

		// If fluffs are not throwable and the attachee is not controlling, embed the fluff to sprout out.
		bool sprouting = false;
		if (!Globals.Instance.fluffsThrowable && attachee != null && !attachee.controlling)
		{
			geometry.transform.position -= standDirection.normalized * Globals.Instance.fluffLeaveEmbed;
			sprouting = true;
		}
		// If desired, start swaying. 
		if ((attacheeStick == null || attacheeStick.allowSway) && !sprouting)
		{
			ToggleSwayAnimation(sway);
		}

		// Stop moving.
		mover.Stop();
		moving = false;

		// Halt physical interactions.
		if (body != null)
		{
			body.isKinematic = true;
		}
		hull.isTrigger = true;

		forgetCreator = true;
	}

	public void ToggleSwayAnimation(bool playSway)
	{
		if (swayAnimation != null)
		{
			swayAnimation.enabled = playSway;
			swayAnimation["Fluff_Sway"].time = 0;
		}
	}

	public bool TestForBlocking(Vector3 moveDirection, float testDistance, out RaycastHit blocker, bool ignoreIgnorable = true, GameObject whoWantsToKnow = null)
	{
		int fluffLayer = (int)Mathf.Pow(2, gameObject.layer);
		RaycastHit[] hits = Physics.RaycastAll((transform.position + bulb.transform.position) / 2, moveDirection, testDistance, ~fluffLayer);
		bool blocked = false;
		blocker = new RaycastHit();
		for (int j = 0; j < hits.Length && !blocked; j++)
		{
			bool hitIgnoredCollider = ignoreIgnorable && hits[j].collider.gameObject == ignoreCollider;
			bool hitTester = hits[j].collider.gameObject == whoWantsToKnow;
			bool layerIgnorable = Physics.GetIgnoreLayerCollision(gameObject.layer, hits[j].collider.gameObject.layer);
			blocked = !(hitIgnoredCollider || hitTester || layerIgnorable);
			if (blocked)
			{
				blocker = hits[j];
			}
		}
		return blocked;
	}

	// Accessible function that does not require coroutine call.
	public void PopFluff(float secondsDelay = 0)
	{
		StartCoroutine(PopAndDestroy(secondsDelay));
	}

	private IEnumerator PopAndDestroy(float secondsDelay = 0)
	{
		if (secondsDelay > 0)
		{
			yield return new WaitForSeconds(secondsDelay);
		}

		if (popAnimation != null)
		{
			if (!popAnimation.isPlaying)
			{
				popAnimation.Play();
				Destroy(gameObject, popAnimation.clip.length);
			}
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void StopMoving()
	{
		if (mover != null)
		{
			mover.Stop();
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		// Attempt to attach to collided object.
		bool sameLayer = (collision.collider.gameObject.layer == gameObject.layer);
		bool alreadyAttachee = (attachee != null && collision.collider.gameObject == attachee.gameObject);
		bool shouldIgnore = collision.collider.gameObject == ignoreCollider;
		if (!((attachee != null && attachee.possessive) || sameLayer || alreadyAttachee || shouldIgnore))
		{
			// Ensure the standing direction is the surface normal of the collided object.
			Vector3 standingDirection = collision.contacts[0].normal;
			RaycastHit[] hits = Physics.RaycastAll(collision.contacts[0].point + collision.contacts[0].normal, -collision.contacts[0].normal, Mathf.Infinity);
			bool hitFound = false;
			for (int i = 0; i < hits.Length && !hitFound; i++)
			{
				if (hits[i].collider == collision.collider)
				{
					standingDirection = hits[i].normal;
					hitFound = true;
				}
			}
			standingDirection.z = 0;

			GameObject newAttachee = collision.collider.gameObject;
			FluffAttachDelgator fluffAttachDelegator = newAttachee.GetComponent<FluffAttachDelgator>();
			while (fluffAttachDelegator != null && fluffAttachDelegator.attachDelegatee != null && fluffAttachDelegator.attachDelegatee != newAttachee)
			{
				newAttachee = fluffAttachDelegator.attachDelegatee;
				fluffAttachDelegator = newAttachee.GetComponent<FluffAttachDelgator>();
			}
			Attach(newAttachee, collision.contacts[0].point, standingDirection);
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		if ((attachee == null || attachee.gameObject != other.gameObject) && (attachee == null || !attachee.possessive) && ignoreCollider != other.gameObject)
		{
			other.SendMessage("AttachFluff", this, SendMessageOptions.DontRequireReceiver);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (ignoreCollider == other.gameObject)
		{
			ignoreCollider = null;
		}
	}

	void OnDestroy()
	{
		if (attachee != null && attachee.gameObject != null)
		{
			FluffHandler attacheeFluffContainer = attachee.gameObject.GetComponent<FluffHandler>();
			if (attacheeFluffContainer != null)
			{
				attacheeFluffContainer.fluffs.Remove(this);
			}
		}

		DepthMaskHandler depthMask = GetComponent<DepthMaskHandler>();
		if (depthMask != null)
		{
			Destroy(depthMask.depthMask);
		}

		// Keep track of this fluff for easy retrieval.
		if (Globals.Instance != null && Globals.Instance.allFluffs != null)
		{
			Globals.Instance.allFluffs.Remove(this);
		}
	}
}

[System.Serializable]
public class Attachee
{
	public GameObject gameObject;
	public FluffStick attachInfo;
	public Vector3 attachPoint;
	public bool possessive;
	public bool controlling;

	public Attachee(GameObject gameObject, FluffStick attachInfo, Vector3 attachPoint, bool possessive = false, bool controlling = false)
	{
		this.gameObject = gameObject;
		this.attachInfo = attachInfo;
		this.attachPoint = attachPoint;
		this.possessive = possessive;
		this.controlling = controlling;
	}
}
