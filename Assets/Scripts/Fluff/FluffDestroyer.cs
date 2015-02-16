using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FluffDestroyer : MonoBehaviour {
	public bool destroyFluffs = true;
	public bool destroyBonds = true;
	public int newNaturalFluff = -1;
	private List<GameObject> toDestroy = null;
	private List<FluffHandler> toEmpty = null;
	public float crossAlpha = 0.5f;
	private float restAlpha;
	public float fadeTime = 1;
	private MeshRenderer targetRenderer;
	

	void Awake()
	{
		targetRenderer = GetComponent<MeshRenderer>();
		restAlpha = targetRenderer.material.color.a;
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

		if (toDestroy != null)
		{

			for (int i = toDestroy.Count - 1; i >= 0; i--)
			{
				Fluff fluff = toDestroy[i].GetComponent<Fluff>();
				if (fluff != null)
				{
					fluff.StopMoving();
					fluff.PopFluff();
				}
				else
				{
					Destroy(toDestroy[i]);
				}
				toDestroy.RemoveAt(i);
			}
			toDestroy.Clear();
			toDestroy = null;
		}

		if (toEmpty != null)
		{
			for (int i = toEmpty.Count - 1; i >= 0; i--)
			{
				if (destroyFluffs)
				{
					toEmpty[i].DestroyAllFluffs();
				}
				if (newNaturalFluff >= 0)
				{
					toEmpty[i].naturalFluffCount = newNaturalFluff;
				}
				toEmpty.RemoveAt(i);
			}
			toEmpty.Clear();
			toEmpty = null;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		bool crossed = false;
		if (other.gameObject.tag == "Fluff" && destroyFluffs)
		{
			Fluff fluff = other.GetComponent<Fluff>();
			if (fluff != null && (fluff.attachee == null || !fluff.attachee.possessive))
			{
				if (toDestroy == null)
				{
					toDestroy = new List<GameObject>();
				}
				toDestroy.Add(other.gameObject);
			}
			crossed = true;
		}
		else if (other.gameObject.tag == "Character")
		{
			FluffHandler spawn = other.GetComponent<FluffHandler>();
			if (spawn != null)
			{
				if (toEmpty == null)
				{
					toEmpty = new List<FluffHandler>();
				}
				toEmpty.Add(spawn);
			}

			if (destroyBonds)
			{
				CharacterComponents character = other.GetComponent<CharacterComponents>();
				for (int i = 0; i < character.bondAttachable.bonds.Count; i++)
				{
					character.bondAttachable.bonds[i].BreakBond();
				}
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
