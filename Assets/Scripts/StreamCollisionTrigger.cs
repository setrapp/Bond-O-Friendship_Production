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
	private bool wasBlocking = true;
	public List<StreamBody> streamsTouched;
	public bool warnForBody = true;
	public Collider ignoreCollider;
	public bool fakeStreamEnd = false;
	
	//public bool debug = false;

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

		wasBlocking = blockStream;
	}

	void FixedUpdate()
	{
		// Keep body moving to ensure that collisions are listened for.
		transform.position += oscillateDirection * oscillateAmount * Time.deltaTime;
		oscillateDirection *= -1;

		// If no longer blocking streams, unblock currently blocked streams.
		if (wasBlocking && !blockStream)
		{
			for (int i = 0; i < streamsTouched.Count; i++)
			{
				StopBlockingStream(streamsTouched[i] as Stream);
			}
		}
		else if (!wasBlocking && blockStream)
		{
			for (int i = 0; i < streamsTouched.Count; i++)
			{
				StartBlockingStream(streamsTouched[i] as Stream);
			}
		}
		wasBlocking = blockStream;

		float oldStreamTouchCount = streamsTouched.Count;
		for (int i = 0; i < streamsTouched.Count; i++)
		{
			if (streamsTouched[i] == null)
			{
				streamsTouched.RemoveAt(i);
				i--;
			}
		}
		if (oldStreamTouchCount != streamsTouched.Count)
		{
			SetReactionsStreamTouches();
		}
	}

	// If blocking is desired, block newly colliding streams.
	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.layer == streamLayer && col.collider != ignoreCollider)
		{
			StreamBody stream = col.collider.GetComponent<StreamBody>();
			if (stream != null)
			{
				CallStreamAction(stream);
				AddStreamTouched(stream);
			}
		}
	}
	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.layer == streamLayer && col.GetComponent<Collider>() != ignoreCollider)
		{
			StreamBody stream = col.GetComponent<Collider>().GetComponent<StreamBody>();
			if (stream != null)
			{
				CallStreamAction(stream);
				AddStreamTouched(stream);
			}
		}
	}

	// React to colliding streams.
	void OnCollisionStay(Collision col)
	{
		if (col.gameObject.layer == streamLayer && col.collider != ignoreCollider)
		{
			StreamBody stream = col.collider.GetComponent<StreamBody>();
			if (stream != null)
			{
				CallStreamAction(stream);
			}
		}
	}
	void OnTriggerStay(Collider col)
	{
		if (col.gameObject.layer == streamLayer && col.GetComponent<Collider>() != ignoreCollider)
		{
			StreamBody stream = col.GetComponent<Collider>().GetComponent<StreamBody>();
			if (stream != null)
			{
				CallStreamAction(stream);
			}
		}
	}


	// Stop blocking streams that are no colliding.
	void OnCollisionExit(Collision col)
	{
		if (col.gameObject.layer == streamLayer && col.collider != ignoreCollider)
		{
			RemoveStreamTouched(col.collider.GetComponent<StreamBody>());
		}
	}
	void OnTriggerExit(Collider col)
	{
		if (col.gameObject.layer == streamLayer && col.GetComponent<Collider>() != ignoreCollider)
		{
			RemoveStreamTouched(col.GetComponent<Collider>().GetComponent<StreamBody>());
		}
	}

	void OnDestroy()
	{
		//if (debug)
			//Debug.Log ("Destroyed" + streamsTouched.Count);
		while (streamsTouched.Count > 0)
		{
			RemoveStreamTouched(0);
		}
	}

	private void AddStreamTouched(StreamBody stream)
	{
		if (!streamsTouched.Contains(stream))
		{
			if (blockStream)
			{
				StartBlockingStream(stream as Stream);
			}
			streamsTouched.Add(stream);
			SetReactionsStreamTouches();

			// Hide stream particles, this should only be used if this collider will never let the stream pass.
			if (fakeStreamEnd)
			{
				Stream mobileStream = stream as Stream;
				if (mobileStream != null && mobileStream.particles != null)
				{
					mobileStream.particles.startLifetime = 0;
				}
			}
		}
	}

	private void RemoveStreamTouched(StreamBody stream)
	{
		RemoveStreamTouched(streamsTouched.IndexOf(stream));
	}

	private void RemoveStreamTouched(int index)
	{
		if (index >= 0 && index < streamsTouched.Count)
		{
			StopBlockingStream(streamsTouched[index] as Stream);
			streamsTouched.RemoveAt(index);
			SetReactionsStreamTouches();
		}
	}

	private void StartBlockingStream(Stream stream)
	{
		if (stream != null)
		{
			stream.streamBlockers++;
		}
	}

	private void StopBlockingStream(Stream stream)
	{
		if (stream != null)
		{
			stream.streamBlockers--;
		}
	}

	private void SetReactionsStreamTouches()
	{
		for (int i = 0; i < reactions.Count; i++)
		{
			if (reactions[i] != null)
			{
				reactions[i].SetTouchedStreams(streamsTouched.Count);
			}
		}
	}

	void CallStreamAction(StreamBody stream)
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
