using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StreamCollisionTrigger : MonoBehaviour {

	public Rigidbody collisionBody;
	public float oscillateAmount = 0.1f;
	private Vector3 oscillateDirection = new Vector3(0, 0, 1);
	private int streamLayer;
	public List<StreamReaction> reactions;
	public bool blockStream = true;
	public List<Stream> streamsBeingBlocked;
	public bool warnForBody = true;

	void Start()
	{
		streamLayer = LayerMask.NameToLayer("Water");

		if (collisionBody == null)
		{
			collisionBody = GetComponent<Rigidbody>();

			if (collisionBody == null)
			{
				Debug.LogError("Stream Collision Trigger on " + gameObject.name + " is not attached to a non-kinematic rigidbody. A non-kinematic rigidbody is required for collision detection with streams.");
			}
		}
	}

	void Update()
	{
		// Keep body moving to ensure that collisions are listened for.
		transform.position += oscillateDirection * oscillateAmount * Time.deltaTime;
		oscillateDirection *= -1;

		// If no longer blocking streams, unblock currently blocked streams.
		if (!blockStream && streamsBeingBlocked.Count > 0)
		{
			while(streamsBeingBlocked.Count > 0)
			{
				RemoveStreamBlockee(0);
			}
		}
	}

	// If blocking is desired, block newly colliding streams.
	void OnCollisionEnter(Collision col)
	{
		if (blockStream && col.gameObject.layer == streamLayer)
		{
			AddStreamBlockee(col.collider.GetComponent<Stream>());
		}
	}
	void OnTriggerEnter(Collider col)
	{
		if (blockStream && col.gameObject.layer == streamLayer)
		{
			AddStreamBlockee(col.collider.GetComponent<Stream>());
		}
	}

	// React to colliding streams.
	void OnCollisionStay(Collision col)
	{
		if (col.gameObject.layer == streamLayer)
		{
			Stream stream = col.collider.GetComponent<Stream>();
			if (stream != null)
			{
				CallStreamAction(stream);
			}
		}
	}
	void OnTriggerStay(Collider col)
	{
		if (col.gameObject.layer == streamLayer)
		{
			Stream stream = col.collider.GetComponent<Stream>();
			if (stream != null)
			{
				CallStreamAction(stream);
			}
		}
	}


	// Stop blocking streams that are no colliding.
	void OnCollisionExit(Collision col)
	{
		if (col.gameObject.layer == streamLayer)
		{
			RemoveStreamBlockee(col.collider.GetComponent<Stream>());
		}
	}
	void OnTriggerExit(Collider col)
	{
		if (col.gameObject.layer == streamLayer)
		{
			RemoveStreamBlockee(col.collider.GetComponent<Stream>());
		}
	}

	void OnDestroy()
	{
		while (streamsBeingBlocked.Count > 0)
		{
			RemoveStreamBlockee(0);
		}
	}

	private void AddStreamBlockee(Stream stream)
	{
		if (!streamsBeingBlocked.Contains(stream))
		{
			stream.streamBlockers++;
			streamsBeingBlocked.Add(stream);
		}
	}

	private void RemoveStreamBlockee(Stream stream)
	{
		RemoveStreamBlockee(streamsBeingBlocked.IndexOf(stream));
	}

	private void RemoveStreamBlockee(int index)
	{
		if (index >= 0 && index < streamsBeingBlocked.Count)
		{
			streamsBeingBlocked[index].streamBlockers--;
			streamsBeingBlocked.RemoveAt(index);
		}
	}

	void CallStreamAction(Stream stream)
	{
		if (stream != null)
		{
			for (int i = 0; i < reactions.Count; i++)
			{
				if (reactions[i] != null)
				{
					stream.ProvokeReaction(reactions[i]);
				}
			}
		}
	}
}
