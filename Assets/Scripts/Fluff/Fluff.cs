using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SimpleMover))]
public class Fluff : MonoBehaviour {
	[HideInInspector]
	public SimpleMover mover;
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
	public MeshRenderer innerBulb;
	//public MeshRenderer stalk;
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
	public GameObject soleAttractor = null;
	public float fluffFill;
	public float minFill = 0;
	public float maxFill = 1;

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

		if (attachee != null && attachee.attachInfo != null)
		{
			Attach(attachee.attachInfo, true);
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

		fluffFill = maxFill;
	}

	void FixedUpdate()
	{
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
	}

	void Update() 
	{
		if(forgetCreator)
		{
			creator = null;
			forgetCreator = false;
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
				PopFluff();
				trail.gameObject.SetActive(false);
			}
			else
			{
				if (attachee != null && attachee.attachInfo != null && attachee.attachInfo.stuckFluff == this)
				{
					attachee.attachInfo.stuckFluff = null;
				}
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
					if (attacheeStick == null || attacheeStick.root == null || attacheeStick.root.allowSway)
					{
						ToggleSwayAnimation(true);
					}
				}
			}
		}

		innerBulb.transform.forward = Vector3.forward;
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
			Attach(attemptPassHit.collider.GetComponent<FluffStick>(), true);
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
		bool blocked = false;// TestForBlocking(toPuller, toPullerDist, out attemptPullHit, false, puller);
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
		if (attachee != null && attachee.attachInfo != null && attachee.attachInfo.root != null && !attachee.attachInfo.root.fluffsDetachable)
		{
			attachee.attachInfo.AddPullForce(pullForce, transform.position);
		}
		else
		{
			if (attachee != null && attachee.attachInfo != null)
			{
				attachee.attachInfo.FluffDetached(this);
			}

			if (body != null)
			{
				body.isKinematic = false;
			}
			mover.Accelerate(pullForce, true, false, true);
		}
	}

	public void Attach(FluffStick attacheeStick, bool sway = true, bool sprouting = false)
	{
		if (Globals.Instance == null)
		{
			return;
		}

		// If no potential attachee is given, disregard.
		if (attacheeStick == null)
		{
			return;
		}

		GameObject attacheeObject = attacheeStick.gameObject;

		// If already attached to a possessive attachee, do not attempt to attach.
		if (attachee != null && attachee.possessive)
		{
			return;
		}

		Vector3 position = attacheeStick.transform.TransformPoint(attacheeStick.stickOffset);
		Vector3 standDirection = attacheeStick.transform.TransformDirection(attacheeStick.stickDirection);

		// Position and orient.
		transform.position = position;
		transform.up = standDirection;

		// Actaully attach to target and record relationship to attachee.
		Vector3 attachPoint = attacheeObject.transform.InverseTransformDirection(position - attacheeObject.transform.position);
		attachee = new Attachee(attacheeObject, attacheeStick, attachPoint, false, false);
		if (attacheeStick.root == null || attacheeStick.root.trackStuckFluffs)
		{
			attacheeStick.stuckFluff = this;
		}
		baseDirection = attacheeStick.stickDirection;
		ignoreCollider = attacheeObject;
		if (Globals.Instance.fluffsThrowable)
		{
			nonAttractTime = 0;
			attractable = true;
		}

		// Notify the potential attachee that fluff has been attached.
		attacheeObject.SendMessage("AttachFluff", this, SendMessageOptions.DontRequireReceiver);

		// If fluffs are not throwable and the attachee is not controlling, embed the fluff to sprout out.
		if (sprouting && attachee != null && !attachee.controlling)
		{
			geometry.transform.position -= standDirection.normalized * Globals.Instance.fluffLeaveEmbed;
		}
		// If desired, start swaying. 
		if ((attacheeStick.root == null || attacheeStick.root.allowSway) && !sprouting)
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

		PlayFluffAudio();
	}

	public void PlayFluffAudio(CharacterComponents attachingCharacter = null)
	{
		Vector3 fromNearPlayer = Vector3.zero;

		if (attachingCharacter == null)
		{
			Vector3 fromPlayer1 = transform.position - Globals.Instance.Player1.transform.position;
			Vector3 fromPlayer2 = transform.position - Globals.Instance.Player2.transform.position;
			fromNearPlayer = fromPlayer1;
			if (fromPlayer2.sqrMagnitude < fromPlayer1.sqrMagnitude)
			{
				fromNearPlayer = fromPlayer2;
			}
		}


		AudioSource attachAudio = Globals.Instance.fluffAudio;
		if (attachAudio == null || fromNearPlayer.sqrMagnitude > Mathf.Pow(attachAudio.maxDistance, 2))
		{
			return;
		}

		attachAudio.transform.position = CameraSplitter.Instance.audioListener.transform.position + fromNearPlayer;
		attachAudio.Play();
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

	public void Inflate(float inflation)
	{
		fluffFill = Mathf.Min(fluffFill + inflation, maxFill);
		bulb.transform.localScale = new Vector3(fluffFill, fluffFill, fluffFill);
	}

	public void InflateToFull()
	{
		bulb.transform.localScale = new Vector3(maxFill, maxFill, maxFill);
	}

	public void Deflate(float deflation)
	{
		fluffFill = Mathf.Max(fluffFill - deflation, minFill);
		bulb.transform.localScale = new Vector3(fluffFill, fluffFill, fluffFill);
		if (fluffFill <= minFill)
		{
			PopFluff();
		}
	}

	// Accessible function that does not require coroutine call.
	public void PopFluff(float secondsDelay = 0, float slowMultiplier = -1, bool fakeDestroy = false, bool deflateBeforePop = false)
	{
		if (!gameObject.activeInHierarchy)
		{
			return;
		}

		if (slowMultiplier >= 0)
		{
			StartCoroutine(SlowBeforePop(slowMultiplier, secondsDelay));
		}

		StartCoroutine(PopAndDestroy(secondsDelay, fakeDestroy, deflateBeforePop));
	}

	private IEnumerator PopAndDestroy(float secondsDelay = 0, bool fakeDestroy = false, bool deflateBeforePop = false)
	{
		if (secondsDelay > 0)
		{
			if (deflateBeforePop)
			{
				while(secondsDelay > 0)
				{
					Deflate(Time.deltaTime / secondsDelay);
					secondsDelay -= Time.deltaTime;
					yield return null;
				}
			}
			else
			{
				yield return new WaitForSeconds(secondsDelay);
			}
		}

		if (popAnimation != null)
		{
			if (!popAnimation.isPlaying)
			{
				popAnimation.Play();
				if (fakeDestroy)
				{
					StartCoroutine(HideOnPop(bulb.transform.localScale, popAnimation.clip.length));
				}
				else
				{
					Destroy(gameObject, popAnimation.clip.length);
				}
			}
		}
		else
		{
			if (fakeDestroy)
			{
				StartCoroutine(HideOnPop(bulb.transform.localScale));
			}
			else
			{
				Destroy(gameObject);
			}
		}
	}

	private IEnumerator HideOnPop (Vector3 bulbScale, float secondsDelay = 0)
	{
		yield return new WaitForSeconds(secondsDelay);
		bulb.transform.localScale = bulbScale;
		gameObject.SetActive(false);
	}

	private IEnumerator SlowBeforePop(float endSpeedMultiplier, float timeUntilPop)
	{
		float startMultiplier = mover.externalSpeedMultiplier;
		float normalCutSpeed = mover.cutSpeedThreshold;
		Vector3 normalVelocity = mover.velocity;
		float slowingTime = 0;
		float slowingProgress = 0;

		mover.cutSpeedThreshold = 0;

		while(slowingProgress < 1)
		{
			yield return null;
			slowingTime += Time.deltaTime;
			if (timeUntilPop <= 0) { slowingProgress = 1; }
			else { slowingProgress = slowingTime / timeUntilPop; }
			slowingProgress = Mathf.Min(slowingProgress, 1);

			mover.externalSpeedMultiplier = (startMultiplier * (1 - slowingProgress)) + (endSpeedMultiplier * slowingProgress);
		}

		mover.externalSpeedMultiplier = startMultiplier;
		mover.velocity = normalVelocity;
		mover.cutSpeedThreshold = normalCutSpeed;
	}

	public void StopMoving()
	{
		if (mover != null)
		{
			mover.Stop();
		}
	}

	void OnCollisionEnter(Collision col)
	{
		AttemptColliderAttach(col.collider);
	}

	void OnTriggerEnter(Collider col)
	{
		AttemptColliderAttach(col);
	}
	
	/*void OnTriggerEnter(Collider other)
	{
		if ((attachee == null || attachee.gameObject != other.gameObject) && (attachee == null || !attachee.possessive) && ignoreCollider != other.gameObject)
		{
			other.SendMessage("AttachFluff", this, SendMessageOptions.DontRequireReceiver);
		}
	}*/

	void OnTriggerExit(Collider other)
	{
		if (ignoreCollider == other.gameObject)
		{
			ignoreCollider = null;
		}
	}

	private void AttemptColliderAttach(Collider col)
	{
		bool sameLayer = (col.gameObject.layer == gameObject.layer);
		bool alreadyAttachee = (attachee != null && GetComponent<Collider>().gameObject == attachee.gameObject);
		bool shouldIgnore = col.gameObject == ignoreCollider;
		bool nonDetachable = (attachee != null && attachee.attachInfo != null && attachee.attachInfo.root != null && !attachee.attachInfo.root.fluffsDetachable);
		if ((attachee != null && attachee.possessive) || nonDetachable || sameLayer || alreadyAttachee || shouldIgnore)
		{
			return;
		}

		FluffStick fluffStick = col.GetComponent<FluffStick>();
		if (fluffStick == null || !fluffStick.CanStick())
		{
			return;
		}
		
		if (fluffStick.root == null || fluffStick.root.trackStuckFluffs)
		{
			fluffStick.stuckFluff = this;
		}

		GameObject newAttachee = GetComponent<Collider>().gameObject;
		FluffAttachDelgator fluffAttachDelegator = newAttachee.GetComponent<FluffAttachDelgator>();
		while (fluffAttachDelegator != null && fluffAttachDelegator.attachDelegatee != null && fluffAttachDelegator.attachDelegatee != newAttachee)
		{
			newAttachee = fluffAttachDelegator.attachDelegatee;
			fluffAttachDelegator = newAttachee.GetComponent<FluffAttachDelgator>();
		}

		Attach(fluffStick);
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

			if (attachee.attachInfo != null && attachee.attachInfo.stuckFluff == this)
			{
				attachee.attachInfo.FluffDetached(this);
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
