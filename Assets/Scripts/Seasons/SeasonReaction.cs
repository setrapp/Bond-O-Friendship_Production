using UnityEngine;
using System.Collections;

public class SeasonReaction : MonoBehaviour {

	public string managerSearchTag = "Island";
	public SeasonManager manager = null;
	protected SeasonManager.ActiveSeason season = SeasonManager.ActiveSeason.DRY;


	protected virtual void Start()
	{
		FindManager();
	}

	protected virtual void Update()
	{
		if (manager != null && season != manager.activeSeason)
		{
			season = manager.activeSeason;
			ApplySeasonChanges();
		}
	}

	public void FindManager(bool resetPreCheck = false)
	{
		if (resetPreCheck)
		{
			manager = null;
		}

		// Find the season manager that controls this object by checking the transform parents.
		Transform parent = transform.parent;
		while (manager == null && parent != null)
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
			season = manager.activeSeason;
			enabled = true;
		}
		else
		{
			enabled = false;
		}
	}

	protected virtual void ApplySeasonChanges() { }
}
