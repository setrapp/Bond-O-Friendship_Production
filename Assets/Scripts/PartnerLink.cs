using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartnerLink : MonoBehaviour {
	public bool isPlayer = false;
	public Renderer headRenderer;
	public Renderer fillRenderer;
	public TrailRenderer trail;
	public PulseShot pulseShot;
	public float partnerLineSize = 0.25f;
	[HideInInspector]
	public SimpleMover mover;
	[HideInInspector]
	public Tracer tracer;
	public GameObject connectionPrefab;
	[SerializeField]
	public List<SimpleConnection> connections;
	[HideInInspector]
	public float fillScale = 1;
	public bool empty;
	public float minScale;
	public float maxScale;
	public float normalScale;
	public float preChargeScale;
	public float scaleRestoreRate;
	public float endChargeRestoreRate;
	public bool absorbing = false;
	public int volleysToConnect = 2;
	private List<MovePulse> fluffsToAdd;

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
		if (pulseShot == null)
		{
			pulseShot = GetComponent<PulseShot>();
		}

		fillRenderer.material.color = headRenderer.material.color;
	}
	
	void Update()
	{
		if (fluffsToAdd != null)
		{
			//Debug.Log(pulseShot.fluffSpawn.fluffs.Count);
			for (int i = 0; i < fluffsToAdd.Count; i++)
			{
				fluffsToAdd[i].EndPass();
				Vector3 fluffRotation = pulseShot.fluffSpawn.FindOpenFluffAngle();
				fluffsToAdd[i].transform.localEulerAngles = fluffRotation;
				fluffsToAdd[i].baseAngle = fluffRotation.z;
				fluffsToAdd[i].baseDirection = Quaternion.Euler(0, 0, fluffsToAdd[i].baseAngle) * Vector3.up;
				Vector3 worldBaseDirection = transform.InverseTransformDirection(fluffsToAdd[i].baseDirection);
				worldBaseDirection.x *= -1;
				worldBaseDirection.y = worldBaseDirection.z;
				worldBaseDirection.z = 0;
				fluffsToAdd[i].transform.up = worldBaseDirection;
				//Debug.Log(fluffsToAdd[i].transform.up);
				fluffsToAdd[i].transform.position = pulseShot.fluffSpawn.fluffContainer.transform.position + fluffsToAdd[i].transform.up * pulseShot.fluffSpawn.spawnOffset;
				if (fluffsToAdd[i].swayAnimation != null)
				{
					fluffsToAdd[i].swayAnimation["Fluff_Sway"].time = 0;
					fluffsToAdd[i].swayAnimation.enabled = false;
					Vector3 rotation = fluffsToAdd[i].swayAnimation.transform.localEulerAngles;
					rotation.z = 0;
					fluffsToAdd[i].swayAnimation.transform.localEulerAngles = rotation;
				}
				pulseShot.fluffSpawn.fluffs.Add(fluffsToAdd[i]);
				fluffsToAdd[i].transform.parent = pulseShot.fluffSpawn.fluffContainer.transform;
			}
			fluffsToAdd.Clear();
			fluffsToAdd = null;
		}

		fillRenderer.transform.localScale = new Vector3(fillScale, fillScale, fillScale);

		trail.startWidth = transform.localScale.x;
	}

	public void AttachFluff(MovePulse pulse)
	{
		//Debug.Log(pulse.gameObject.name);
		if (pulse != null && (absorbing || pulse.moving))// && (fluffsToAdd == null || !fluffsToAdd.Contains(pulse)))
		{
			//transform.localScale += new Vector3(pulse.capacity, pulse.capacity, pulse.capacity);
			if (pulse.creator != pulseShot)
			{
				pulseShot.volleys = 1;
				if (pulse.volleyPartner != null && pulse.volleyPartner == pulseShot)
				{
					pulseShot.volleys = pulse.volleys;
				}
				if (pulseShot.volleys >= volleysToConnect)
				{
					bool connectionAlreadyMade = false;
					for (int i = 0; i < connections.Count && !connectionAlreadyMade; i++)
					{
						if ((connections[i].attachment1.partner == this && connections[i].attachment2.partner == pulse.creator.partnerLink) || (connections[i].attachment2.partner == this && connections[i].attachment1.partner == pulse.creator.partnerLink))
						{
							connectionAlreadyMade = true;
						}
					}
					if (!connectionAlreadyMade)
					{
						SimpleConnection newConnection = ((GameObject)Instantiate(connectionPrefab, Vector3.zero, Quaternion.identity)).GetComponent<SimpleConnection>();
						connections.Add(newConnection);
						pulse.creator.partnerLink.connections.Add(newConnection);
						newConnection.AttachPartners(pulse.creator.partnerLink, this);
					}
				}
			}
			pulseShot.lastPulseAccepted = pulse.creator;

			if (fluffsToAdd == null)
			{
				fluffsToAdd = new List<MovePulse>();
			}
			fluffsToAdd.Add(pulse);
		}	
	}
}
