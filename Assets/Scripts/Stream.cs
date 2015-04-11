﻿using UnityEngine;
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
	public Material lineMaterial;
	public ParticleSystem diffusionParticles;

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
				SeekNextChannel();
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
		StreamChannel[] nextChannels = targetChannel.parentSeries.GetNextChannels(targetChannel);
		if (nextChannels != null && nextChannels.Length > 0)
		{
			oldChannel = targetChannel;
			targetChannel = nextChannels[0];
			ending = false;

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
		if (reaction != null)
		{
			reaction.React(actionRate * Time.deltaTime);
		}

		StreamReactionDelegate reactionDelegate = reactionObject.GetComponent<StreamReactionDelegate>();
		if (reactionDelegate != null)
		{
			for (int i = 0; i < reactionDelegate.reactions.Count; i++)
			{
				reactionDelegate.reactions[i].React(actionRate * Time.deltaTime);
			}
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
