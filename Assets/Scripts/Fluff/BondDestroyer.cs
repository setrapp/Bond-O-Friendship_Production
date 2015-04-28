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

	void AttemptDestoy(Collider other)
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

		if (crossed)
		{
			Color crossColor = targetRenderer.material.color;
			crossColor.a = crossAlpha;
			targetRenderer.material.color = crossColor;
		}
	}
}
