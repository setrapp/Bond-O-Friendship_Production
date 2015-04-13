using UnityEngine;
using System.Collections;

public class Stream : MonoBehaviour {

	public StreamChannel targetChannel;
	public SimpleMover mover;
	public Tracer tracer;
	public bool startAtTarget = true;
	public bool showTarget = false;
	private StreamChannel oldChannel;
	private bool ending;
	public float actionRate = 1;
	public LayerMask ignoreReactionLayers;
	public Material lineMaterial;
	public ParticleSystem diffusionParticles;
	public Stream streamSplittingPrefab;

	/*TODO handle streams merging back together*/

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
	}

	void Start()
	{
		if (targetChannel != null && startAtTarget)
		{
			transform.position = targetChannel.transform.position;
		}

		tracer.CreateLineMaker(true);
		if (lineMaterial != null)
		{
			if (tracer.lineRenderer != null)
			{
				tracer.lineRenderer.material = lineMaterial;
			}

			if (diffusionParticles != null)
			{
				diffusionParticles.renderer.material.color = lineMaterial.color;
				diffusionParticles.gameObject.SetActive(false);
			}
		}

		SeekNextChannel();
	}

	void Update()
	{
		if (!ending)
		{
			if (diffusionParticles != null && diffusionParticles.gameObject.activeSelf)
			{
				diffusionParticles.gameObject.SetActive(false);
			}

			Vector3 oldToTarget = targetChannel.transform.position - oldChannel.transform.position;
			Vector3 toTarget = targetChannel.transform.position - transform.position;

			if (Vector3.Dot(oldToTarget, toTarget) < 0)
			{
				Vector3 toBank1 = Helper.ProjectVector(targetChannel.transform.right, targetChannel.bank1.transform.position - transform.position);
				Vector3 toBank2 = Helper.ProjectVector(targetChannel.transform.right, targetChannel.bank2.transform.position - transform.position);
				//if (Vector3.Dot(toBank1, toBank2) < 0)
				{
					SeekNextChannel();
				}
			}

			mover.velocity = mover.rigidbody.velocity;
			mover.Accelerate(toTarget);
		}
		else
		{
			if (mover.velocity.sqrMagnitude > 0)
			{
				mover.Stop();
				mover.body.angularVelocity = Vector3.zero;
				transform.up = targetChannel.transform.forward;
			}
			if (diffusionParticles != null && !diffusionParticles.gameObject.activeSelf)
			{
				diffusionParticles.gameObject.SetActive(true);
			}
			SeekNextChannel();
		}

		tracer.AddVertex(transform.position);
	}

	private void SeekNextChannel()
	{
		if (targetChannel == null)
		{
			return;
		}

		StreamChannel[] nextChannels = targetChannel.parentSeries.GetNextChannels(targetChannel);
		if (nextChannels != null && nextChannels.Length > 0)
		{
			oldChannel = targetChannel;
			targetChannel = nextChannels[0];
			ending = false;

			if (streamSplittingPrefab != null)
			{
				for (int i = 1; i < nextChannels.Length; i++)
				{
					Stream splitStream = ((GameObject)Instantiate(streamSplittingPrefab.gameObject, transform.position, transform.rotation)).GetComponent<Stream>();
					splitStream.targetChannel = nextChannels[i];
					splitStream.startAtTarget = false;
					splitStream.transform.parent = transform.parent;
					splitStream.mover.maxSpeed = mover.maxSpeed;
					//splitStream.transform.up = targetChannel.transform.position - splitStream.transform.position;
					//splitStream.mover.Move(splitStream.transform.up, splitStream.mover.velocity.magnitude);
				}
			}
		}
		else
		{
			ending = true;
		}
	}

	void OnCollisionStay(Collision col)
	{
		int layer = (int)Mathf.Pow(2, col.collider.gameObject.layer);
		if ((layer & ignoreReactionLayers.value) != layer)
		{
			Rigidbody body = col.rigidbody;
			if (body != null)
			{
				ProvokeReaction(body.gameObject);
			}
			else
			{
				ProvokeReaction(col.collider.gameObject);
			}
		}
	}

	void OnTriggerStay(Collider col)
	{
		int layer = (int)Mathf.Pow(2, col.collider.gameObject.layer);
		if ((layer & ignoreReactionLayers.value) != layer)
		{
			Rigidbody body = col.GetComponent<Rigidbody>();
			if (body != null)
			{
				ProvokeReaction(body.gameObject);
			}
			else
			{
				ProvokeReaction(col.gameObject);
			}
		}
	}

	private void ProvokeReaction(GameObject reactionObject)
	{
		float minAlterSpeed = -1;

		StreamReaction reaction = reactionObject.GetComponent<StreamReaction>();
		if (reaction != null)
		{
			bool reacted = reaction.React(actionRate * Time.deltaTime);
			if (!reacted && reaction.streamAlterSpeed >= 0 && (reaction.streamAlterSpeed < minAlterSpeed || minAlterSpeed < 0))
			{
				minAlterSpeed = reaction.streamAlterSpeed;
			}
		}

		StreamReactionDelegate reactionDelegate = reactionObject.GetComponent<StreamReactionDelegate>();
		if (reactionDelegate != null)
		{
			for (int i = 0; i < reactionDelegate.reactions.Count; i++)
			{
				bool reacted = reactionDelegate.reactions[i].React(actionRate * Time.deltaTime);
				if (!reacted && reactionDelegate.reactions[i].streamAlterSpeed >= 0 && (reactionDelegate.reactions[i].streamAlterSpeed < minAlterSpeed || minAlterSpeed < 0))
				{
					minAlterSpeed = reactionDelegate.reactions[i].streamAlterSpeed;
				}
			}
		}

		if (minAlterSpeed >= 0)
		{
			mover.maxSpeed = minAlterSpeed;
		}
	}

	void OnDrawGizmos()
	{
		if (showTarget)
		{
			Gizmos.DrawLine(transform.position, targetChannel.transform.position);
		}
	}
}
