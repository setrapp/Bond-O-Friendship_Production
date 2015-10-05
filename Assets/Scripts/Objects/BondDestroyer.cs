using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BondDestroyer : MonoBehaviour {
	public bool destroyFluffs = true;
	public bool destroyBonds = true;
	private List<Fluff> toPop = null;
	public float crossAlpha = 0.5f;
	private float restAlpha;
	public float fadeTime = 1;
	public Renderer targetRenderer;
	public BondStrain bondStrain;
	public bool defaultStrainerToSelf = true;
	[Header("Destroy Pulse")]
	public float initialDelay = 0;
	public float pulseDelay = -1;
	private float lastPulseTime = 0;
	public PulseStats pulseStats;
	private BondDestroyerPulse destroyerPulse;
	public RingPulse DangerPulse;

	void Awake()
	{
		if (targetRenderer == null)
		{
			targetRenderer = GetComponent<MeshRenderer>();
		}

		restAlpha = targetRenderer.material.color.a;

		if (bondStrain.strainer == null && defaultStrainerToSelf)
		{
			bondStrain.strainer = gameObject;
		}
	}

	void Update()
	{
		if (targetRenderer.material.color.a > restAlpha)
		{
			Color fadeColor = targetRenderer.material.color;
			if (fadeTime <= 0)
			{
				fadeColor.a = restAlpha;
			}
			else
			{
				fadeColor.a -= Time.deltaTime / fadeTime;
				if (fadeColor.a < restAlpha)
				{
					fadeColor.a = restAlpha;
				}
			}
			targetRenderer.material.color = fadeColor;
			if (destroyerPulse != null && destroyerPulse.renderer != null)
			{
				destroyerPulse.pulseBase.alpha = fadeColor.a;
				destroyerPulse.renderer.material.color = fadeColor;
			}
		}

		// Only allow one pulse to exist at a time.
		if (pulseDelay >= 0 && pulseDelay < pulseStats.lifeTime)
		{
			pulseDelay = pulseStats.lifeTime;
		}

		if (pulseDelay >= 0 && Time.time - lastPulseTime >= pulseDelay + Mathf.Max(initialDelay, 0))
		{
			initialDelay = 0;

			RingPulse shotPulse = Helper.FirePulse(transform.position, pulseStats, DangerPulse);
			shotPulse.transform.parent = transform.parent;
			shotPulse.gameObject.name = "Destroyer Pulse";
			shotPulse.gameObject.layer = gameObject.layer;
			Renderer pulseRenderer = shotPulse.renderer;
			if (pulseRenderer != null)
			{
				Color pulseColor = targetRenderer.material.color;
				pulseColor.a = restAlpha;
				shotPulse.mycolor = pulseColor;
				pulseRenderer.material.color = pulseColor;
			}
			destroyerPulse = shotPulse.gameObject.AddComponent<BondDestroyerPulse>();
			destroyerPulse.creator = this;
			destroyerPulse.renderer = pulseRenderer;
			destroyerPulse.pulseBase = shotPulse;

			lastPulseTime = Time.time;
		}

		if (toPop != null)
		{
			for (int i = toPop.Count - 1; i >= 0; i--)
			{
				Fluff fluff = toPop[i];
				if (fluff != null)
				{
					fluff.PopFluff();
				}
			}
			toPop.Clear();
			toPop = null;
		}
	}

	void OnCollisionEnter(Collision col)
	{
		AttemptDestoy(col.collider);
	}

	void OnCollisionStay(Collision col)
	{
		AttemptDestoy(col.collider);
	}

	void OnTriggerEnter(Collider col)
	{
		AttemptDestoy(col);
	}

	void OnTriggerStay(Collider col)
	{
		AttemptDestoy(col);
	}

	public void AttemptDestoy(Collider other, BondDestroyerPulse collidedPulse = null)
	{
		bool crossed = false;
		if (other.gameObject.tag == "Fluff" && destroyFluffs)
		{
			Fluff fluff = other.GetComponent<Fluff>();
			if (fluff != null)// && (fluff.attachee == null || !fluff.attachee.possessive))
			{
				if (toPop == null)
				{
					toPop = new List<Fluff>();
				}
				toPop.Add(fluff);
			}
			crossed = true;
		}
		else if (other.gameObject.layer == LayerMask.NameToLayer("Bond") && other.gameObject.transform.parent != null && destroyBonds)
		{
			Bond bond = null;
			BondLink bondLink = other.transform.parent.GetComponent<BondLink>();
			if (bondLink != null)
			{
				bond = bondLink.bond;
			}
			
			if (bond != null)
			{
				//bond.BreakBond();
				bond.AddBondStrain(bondStrain);
			}
			crossed = true;
		}
        else if (other.gameObject.tag == "Bud")
        {
            GameObject Blossom = other.gameObject;
            Blossom.GetComponent<BudFadeOut>().fadeNow = true;
            crossed = true;
        }

		if (crossed)
		{
			Color crossColor = targetRenderer.material.color;
			crossColor.a = crossAlpha;
			targetRenderer.material.color = crossColor;

			/*if (collidedPulse != null && collidedPulse.renderer != null)
			{
				Color pulseCrossColor = crossColor;
				collidedPulse.pulseBase.alpha = pulseCrossColor.a;
				collidedPulse.renderer.material.color = crossColor;
			}*/
			if (destroyerPulse != null && destroyerPulse.renderer != null)
			{
				destroyerPulse.pulseBase.alpha = crossColor.a;
				destroyerPulse.renderer.material.color = crossColor;
			}
		}
	}
}
