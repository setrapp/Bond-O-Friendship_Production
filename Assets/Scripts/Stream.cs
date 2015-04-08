using UnityEngine;
using System.Collections;

public class Stream : MonoBehaviour {

	public StreamChannel targetChannel;
	public SimpleMover mover;
	public Tracer tracer;
	public bool showTarget = false;
	private StreamChannel oldChannel;
	private bool ending;
	public float actionRate = 1;
	public LayerMask ignoreReactionLayers;

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
		transform.position = targetChannel.transform.position;
		tracer.CreateLineMaker(true);

		SeekNextChannel();
	}

	void Update()
	{
		if (!ending)
		{
			Vector3 oldToTarget = targetChannel.transform.position - oldChannel.transform.position;
			Vector3 toTarget = targetChannel.transform.position - transform.position;

			if (Vector3.Dot(oldToTarget, toTarget) < 0)
			{
				SeekNextChannel();
			}

			mover.velocity = mover.rigidbody.velocity;
			mover.Accelerate(toTarget);
		}
		else if (mover.velocity.sqrMagnitude > 0)
		{
			mover.slowDown = true;
		}

		tracer.AddVertex(transform.position);
	}

	private void SeekNextChannel()
	{
		StreamChannel[] nextChannels = targetChannel.parentSeries.GetNextChannels(targetChannel);
		if (nextChannels != null && nextChannels.Length > 0)
		{
			oldChannel = targetChannel;
			targetChannel = nextChannels[0];

			/*TODO spawn new steram and reset it when a split is found*/
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
		StreamReaction reaction = reactionObject.GetComponent<StreamReaction>();
		if (reaction == null)
		{
			StreamReactionDelegate reactionDelegate = reactionObject.GetComponent<StreamReactionDelegate>();
			if (reactionDelegate != null)
			{
				reaction = reactionDelegate.reaction;
			}
		}

		if (reaction != null)
		{
			reaction.React(actionRate * Time.deltaTime);
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
