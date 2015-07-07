using UnityEngine;
using System.Collections;

public class SeasonObjectReaction : SeasonReaction {

	public Rigidbody body;
	public DestroyInSpace destroyInSpace;
	private float baseDrag;
	public float[] seasonDragFactors = new float[3];
	/*private float dryDrag;
	public float wetDrag;
	public float coldDrag;*/

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

		if (body != null)
		{
			baseDrag = body.drag;
		}
		ApplySeasonDrag();
	}

	override protected void Update()
	{
		base.Update();
		
		if (seasonChanged)
		{
			ApplySeasonDrag();
		}
	}

	private void ApplySeasonDrag()
	{
		if (body != null)
		{
			body.drag = baseDrag * seasonDragFactors[(int)season];
		}

		if (destroyInSpace)
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
		
	}
}
