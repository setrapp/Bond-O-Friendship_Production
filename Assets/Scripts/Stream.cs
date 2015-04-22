using UnityEngine;
using System.Collections;

public class Stream : MonoBehaviour {

	public StreamSpawner spawner;
	public StreamChannel targetChannel;
	public SimpleMover mover;
	public Renderer streamRenderer;
	public Tracer tracer;
	public bool startAtTarget = true;
	public bool showTarget = false;
	//public bool autoMove = true;
	public  StreamChannel oldChannel;
	[HideInInspector]
	public bool ending;
	public Vector3 seekOffset;
	public float actionRate = 1;
	public LayerMask ignoreReactionLayers;
	public Material lineMaterial;
	public ParticleSystem diffusionParticles;
	public Stream streamSplittingPrefab;
	public int streamBlockers = 0;
	public float blockingTime = 0;
	public bool drawStreamLine = true;

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
		if (streamRenderer == null)
		{
			streamRenderer = GetComponent<Renderer>();
		}
	}

	void Start()
	{
		if (targetChannel != null && startAtTarget)
		{
			transform.position = targetChannel.transform.position + seekOffset;
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
		UpdateMovement();
	}

	void Update()
	{
		if (streamBlockers <= 0)
		{
			if (!ending)
			{
				if (diffusionParticles != null && diffusionParticles.gameObject.activeSelf)
				{
					diffusionParticles.gameObject.SetActive(false);
				}

				Vector3 streamBedCenter = oldChannel.transform.position + Helper.ProjectVector(oldChannel.transform.forward, transform.position - oldChannel.transform.position);
				Vector3 fromBedCenter = transform.position - streamBedCenter;
				float maxDistFromCenter = oldChannel.bed.transform.localScale.x / 2;
				if (fromBedCenter.sqrMagnitude > Mathf.Pow(maxDistFromCenter, 2))
				{
					transform.position = streamBedCenter + (fromBedCenter.normalized * maxDistFromCenter);
				}

				Vector3 oldToTarget = targetChannel.transform.position - oldChannel.transform.position;
				Vector3 toTarget = (targetChannel.transform.position + seekOffset) - transform.position;

				// TODO: Should the stream be able to change z-depth?
				oldToTarget.z = toTarget.z = 0;

				if (Vector3.Dot(oldToTarget, toTarget) < 0)
				{
					Vector3 toBank1 = Helper.ProjectVector(targetChannel.transform.right, targetChannel.bank1.transform.position - transform.position);
					Vector3 toBank2 = Helper.ProjectVector(targetChannel.transform.right, targetChannel.bank2.transform.position - transform.position);
					//if (Vector3.Dot(toBank1, toBank2) < 0)
					{
						SeekNextChannel();
						toTarget = (targetChannel.transform.position + seekOffset) - transform.position;
					}
				}

				
				mover.AccelerateWithoutHandling(toTarget);
			}
			else
			{
				Destroy(gameObject);
				/*if (mover.velocity.sqrMagnitude > 0)
				{
					mover.Stop();
					transform.up = targetChannel.transform.forward;
				}
				if (diffusionParticles != null && !diffusionParticles.gameObject.activeSelf)
				{
					diffusionParticles.gameObject.SetActive(true);
				}
				SeekNextChannel();*/
			}

			streamBlockers = 0;
			blockingTime = 0;
		}
		else
		{
			if (mover.velocity.sqrMagnitude > 0)
			{
				mover.Stop();
			}

			if (spawner != null && spawner.spawnTime >= 0 && spawner.destroyTimeFactor >= 0)
			{
				blockingTime += Time.deltaTime;
				if (blockingTime >= spawner.spawnTime * spawner.destroyTimeFactor)
				{
					spawner.StopTrackingStream(this);
					spawner = null;
					// TODO make the streams fade before destroying, or just remove their ability to act.
					Destroy(gameObject);
				}
			}
			
		}

		if (drawStreamLine)
		{
			tracer.AddVertex(transform.position);
		}
	}

	public void UpdateMovement()
	{
		Vector3 toTarget = (targetChannel.transform.position + seekOffset) - transform.position;
		
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
					splitStream.seekOffset = seekOffset;
					if (spawner != null)
					{
						spawner.TrackStream(splitStream);
					}
				}
			}
		}
		else
		{
			ending = true;
		}
	}

	public void ProvokeReaction(StreamReaction reaction)
	{
		if (reaction != null)
		{
			reaction.React(actionRate * Time.deltaTime);
		}
	}

	void OnDrawGizmos()
	{
		if (showTarget && targetChannel != null)
		{
			Gizmos.DrawLine(transform.position, targetChannel.transform.position);
		}
	}
}
