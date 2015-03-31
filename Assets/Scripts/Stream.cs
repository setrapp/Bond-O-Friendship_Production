using UnityEngine;
using System.Collections;

public class Stream : MonoBehaviour {

	public StreamChannel targetChannel;
	public SimpleMover mover;
	public Tracer tracer;
	public bool showTarget = false;
	private StreamChannel oldChannel;
	private bool ending;

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

	void OnDrawGizmos()
	{
		if (showTarget)
		{
			Gizmos.DrawLine(transform.position, targetChannel.transform.position);
		}
	}
}
