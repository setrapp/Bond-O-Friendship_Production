using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PulseShot : MonoBehaviour {
	public PartnerLink partnerLink;
	public GameObject pulsePrefab;
	private GameObject pulse;
	public ParticleSystem pulseParticlePrefab;
	private ParticleSystem pulseParticle;
	private float pulseScale;
	public float basePulseSize = 0.25f;
	public PulseShot lastPulseAccepted;
	public bool volleyOnlyFirst = true;
	public int volleys;
	public PulseShot volleyPartner;
	public FloatMoving floatMove;
	public float floatPushBack;
	public FluffSpawn fluffSpawn;
	public int minShotCount;
	public int maxShotCount;
	public float shotSpread;
	public float minShotFactor;

	void Start()
	{
		if (partnerLink == null)
		{
			partnerLink = GetComponent<PartnerLink>();
		}
		if (floatMove == null)
		{
			floatMove = GetComponent<FloatMoving>();
		}
		if (fluffSpawn == null)
		{
			fluffSpawn = GetComponent<FluffSpawn>();
		}
	}

	public void Shoot(Vector3 pulseTarget, float pulseCapacity)
	{
		int passFluffCount = Mathf.Min(Random.Range(minShotCount, maxShotCount), fluffSpawn.fluffs.Count);


		if (fluffSpawn == null || fluffSpawn.fluffs.Count < 1 || passFluffCount < 1)
		{
			return;
		}

		Vector3 passDir = (pulseTarget - transform.position).normalized;
		List<int> passFluffIndices = new List<int>();
		List<GameObject> passFluffs = new List<GameObject>();
		List<float> maxFluffDotPasses = new List<float>();
		for (int i = 0; i < fluffSpawn.fluffs.Count; i++)
		{
			float fluffDotPass = Vector3.Dot(fluffSpawn.fluffs[i].transform.up, passDir);
			if ((maxFluffDotPasses.Count < passFluffCount || fluffDotPass > maxFluffDotPasses[passFluffCount - 1]) && fluffSpawn.fluffs[i].gameObject != fluffSpawn.spawnedFluff)
			{
				maxFluffDotPasses.Add(fluffDotPass);
				passFluffs.Add(fluffSpawn.fluffs[i].gameObject);
				passFluffIndices.Add(i);
				if (maxFluffDotPasses.Count > passFluffCount)
				{
					float minMaxFluffDotPass = maxFluffDotPasses[0];
					int minMaxFluffIndex = 0;
					for (int j = 1; j < maxFluffDotPasses.Count; j++)
					{
						float maxFluffDotPass = maxFluffDotPasses[j];
						if (maxFluffDotPass < minMaxFluffDotPass)
						{
							minMaxFluffDotPass = maxFluffDotPasses[j];
							minMaxFluffIndex = j;
						}
					}
				}
				
			}

			if (maxFluffDotPasses.Count > passFluffCount)
			{
				maxFluffDotPasses.RemoveAt(passFluffCount);
				passFluffs.RemoveAt(passFluffCount);
				passFluffIndices.RemoveAt(passFluffCount);
			}
		}

		
		float shotAngle = -shotSpread / 2;
		if (passFluffs.Count == 1)
		{
			shotAngle = 0;
		}
		float shotDist = Vector3.Distance(pulseTarget, transform.position);

		for (int i = passFluffs.Count - 1; i >= 0; i--)
		{
			fluffSpawn.fluffs.RemoveAt(passFluffIndices[i]);

			Vector3 rotatedPassDir = Quaternion.Euler(0, 0, shotAngle) * passDir;

			MovePulse movePulse = passFluffs[i].GetComponent<MovePulse>();
			movePulse.transform.position = transform.position;
			movePulse.transform.rotation = Quaternion.LookRotation(rotatedPassDir, Vector3.Cross(rotatedPassDir, -Vector3.forward));
			movePulse.transform.parent = transform.parent;
			movePulse.ReadyForPass();
			movePulse.creator = this;
			movePulse.capacity = pulseCapacity;
			movePulse.volleys = volleys + 1;
			movePulse.volleyPartner = lastPulseAccepted;
			shotAngle += shotSpread / passFluffCount;

			// Set target and determine if block within first frame of movement.
			movePulse.target = transform.position + (rotatedPassDir * shotDist * Random.RandomRange(minShotFactor, 1));
			Vector3 fluffToTarget = (movePulse.target - movePulse.transform.position).normalized;
			int fluffLayer = (int)Mathf.Pow(2, gameObject.layer);
			RaycastHit[] hits = Physics.RaycastAll(movePulse.transform.position, fluffToTarget, movePulse.moveSpeed * Time.deltaTime, ~fluffLayer);
			bool blocked = false;
			for (int j = 0; j < hits.Length && !blocked; j++)
			{
				if (hits[j].collider.gameObject != gameObject)
				{
					blocked = hits[j].collider.gameObject != gameObject && !Physics.GetIgnoreLayerCollision(movePulse.gameObject.layer, hits[j].collider.gameObject.layer);
					movePulse.target = hits[j].point;
				}
			}
		}
		

		// If only the first pulse can be volleyed to create a connection, ignore last pulse accepted for future shots.
		if (volleyOnlyFirst)
		{
			lastPulseAccepted = null;
		}

		// If floating propel away from pulse.
		if (floatMove.Floating && passFluffs.Count > 0)
		{
			Vector3 pulseForce = (((transform.position - pulseTarget).normalized * floatPushBack));
			//GetComponent<Rigidbody>().AddForce(pulseForce, ForceMode.VelocityChange);
			partnerLink.mover.velocity += pulseForce * Time.deltaTime;
		}
	}
}
