using UnityEngine;
using System.Collections;

public class SeasonSwitch : MonoBehaviour {

	public string managerSearchTag = "Island";
	public SeasonManager manager;
	public SeasonManager.ActiveSeason targetSeason;
	private SeasonManager.ActiveSeason activeSeason;
	public Collider switchCollider = null;
	public Renderer switchRenderer = null;
	public StreamFillReaction fillReaction = null;
	public StreamScalingReaction scaleReaction = null;
	private bool initializedSeason = false;


	public void Start()
	{
		if (switchCollider == null)
		{
			switchCollider = GetComponent<Collider>();
		}
		if (switchRenderer == null)
		{
			switchRenderer = GetComponent<Renderer>();
		}
		if (fillReaction == null)
		{
			fillReaction = GetComponent<StreamFillReaction>();
		}
		if (scaleReaction == null)
		{
			scaleReaction = GetComponent<StreamScalingReaction>();
		}

		// Find the season manager that controls this object by checking the transform parents.
		Transform parent = transform.parent;
		while(manager == null && parent != null)
		{
			if (parent.gameObject.tag == managerSearchTag)
			{
				manager = parent.GetComponent<SeasonManager>();
			}
			if (manager == null)
			{
				parent = parent.transform.parent;
			}
		}

		if (manager == null)
		{
			enabled = false;
		}
	}

	void Update()
	{
		if (!initializedSeason || (manager != null && activeSeason != manager.activeSeason))
		{
			CheckSeason();
			activeSeason = manager.activeSeason;
			initializedSeason = true;
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if (manager != null && (col.gameObject.tag == ("Character") || col.gameObject.layer == LayerMask.NameToLayer ("Bond")))
		{
			bool seasonChanged = manager.AttemptSeasonChange(targetSeason);
			if (seasonChanged)
			{
				Helper.FirePulse(transform.position, Globals.Instance.defaultPulseStats);
			}
		}
	}

	void CheckSeason()
	{
		if (manager.activeSeason == targetSeason)
		{
			switchCollider.enabled = false;
			RendererToSeason(false);
		}
		else
		{
			switchCollider.enabled = true;
			RendererToSeason(true);
		}
	}

	private void RendererToSeason(bool toUsable)
	{
		bool rendererFound = false;
		for (int i = 0; i < fillReaction.fillTargetRenderers.Count && !rendererFound; i++)
		{
			if (fillReaction.fillTargetRenderers[i] == switchRenderer)
			{
				rendererFound = true;
				if (toUsable)
				{
					switchRenderer.material.color = fillReaction.baseColors[i] * fillReaction.filledTint;
				}
				else
				{
					switchRenderer.material.color = fillReaction.baseColors[i] * fillReaction.unfilledTint;
				}
				
			}
		}

		rendererFound = false;
		for (int i = 0; i < scaleReaction.scalees.Count && !rendererFound; i++)
		{
			if (scaleReaction.scalees[i] != null && scaleReaction.scalees[i].scalee == switchRenderer.transform)
			{
				rendererFound = true;
				if (toUsable)
				{
					switchRenderer.transform.localScale = scaleReaction.scalees[i].scaled;
				}
				else
				{
					switchRenderer.transform.localScale = scaleReaction.scalees[i].unscaled;
				}

			}
		}
	}
}
