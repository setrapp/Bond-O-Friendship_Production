using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SeasonObjectReaction : SeasonReaction {

	public Rigidbody body;
	public DestroyInSpace destroyInSpace;
	public CrumpleMesh crumpleMesh;
	private float baseDrag;
	public float[] seasonDragFactors = new float[3];
	public Renderer reactionRenderer;
	public Color[] seasonColors = new Color[3];
	private Vector3 normalScale = new Vector3(1, 1, 1);
	public Transform wetScaleTarget = null;
	public Vector3 wetScale = new Vector3(1, 1, 1);

	protected override void Start()
	{
		base.Start();

		if (body == null)
		{
			body = GetComponent<Rigidbody>();
		}
		if (destroyInSpace == null)
		{
			destroyInSpace = GetComponent<DestroyInSpace>();
		}
		if (crumpleMesh == null)
		{
			crumpleMesh = GetComponent<CrumpleMesh>();
		}
		if (reactionRenderer == null)
		{
			reactionRenderer = GetComponent<Renderer>();
		}

		if (body != null)
		{
			baseDrag = body.drag;
		}

		if (seasonColors.Length > 0 && reactionRenderer != null)
		{
			seasonColors[0] = reactionRenderer.material.color;
		}

		normalScale = transform.localScale;

		ApplySeasonChanges();
	}

	override protected void ApplySeasonChanges()
	{
		base.ApplySeasonChanges();

		// Drag.
		if (body != null && seasonDragFactors.Length >= 3)
		{
			body.drag = baseDrag * seasonDragFactors[(int)season];
		}

		// Color.
		if (seasonColors.Length >= 3 && reactionRenderer != null)
		{
			reactionRenderer.material.color = seasonColors[(int)season];
		}

		// Allow existence in space if in cold season.
		if (destroyInSpace != null)
		{
			if (season == SeasonManager.ActiveSeason.COLD)
			{
				destroyInSpace.enabled = false;
			}
			else
			{
				destroyInSpace.enabled = true;
			}
		}

		// Stop jiggling in cold season.
		if (crumpleMesh != null)
		{
			if (season == SeasonManager.ActiveSeason.COLD)
			{
				crumpleMesh.enabled = false;
			}
			else
			{
				crumpleMesh.enabled = true;
			}
		}

		// Alter Scale if in wet season
		if (season == SeasonManager.ActiveSeason.WET)
		{
			if(wetScaleTarget != null)
			{
				normalScale = wetScaleTarget.localScale;
				wetScaleTarget.localScale = wetScale;
			}
		}
		else
		{
			wetScaleTarget.localScale = normalScale;
		}
		
	}
}
