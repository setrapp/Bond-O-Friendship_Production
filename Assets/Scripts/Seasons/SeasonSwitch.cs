using UnityEngine;
using System.Collections;

public class SeasonSwitch : MonoBehaviour {

	public string managerSearchTag = "Island";
	public SeasonManager manager;
	public SeasonManager.ActiveSeason targetSeason;
	private SeasonManager.ActiveSeason activeSeason;
	public Collider switchCollider = null;
	public Renderer switchRenderer = null;


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

		if (manager != null)
		{
			activeSeason = manager.activeSeason;
			CheckSeason();
		}
		else
		{
			enabled = false;
		}
	}

	void Update()
	{
		if (manager != null && activeSeason != manager.activeSeason)
		{
			CheckSeason();
			activeSeason = manager.activeSeason;
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
			switchRenderer.enabled = false;
		}
		else
		{
			switchCollider.enabled = true;
			switchRenderer.enabled = true;
		}
	}
}
