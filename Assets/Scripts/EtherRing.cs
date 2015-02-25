using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EtherRing : MonoBehaviour {
	public MembraneShell ringAtmosphere;
	public bool forceAtmosphere = false;
	[SerializeField]
	public List<ExpressivePlaceholder> expressivePlaceholders;
	private List<GameObject> expressiveClouds;

	void Start()
	{
		

		if (Globals.Instance != null)
		{
			Globals.Instance.existingEther = this;
		}

		if (ringAtmosphere != null && (!Application.isEditor || forceAtmosphere))
		{
			Debug.Log("hi");
			ringAtmosphere.CreateShell();
		}
		for (int i = 0; i < expressivePlaceholders.Count; i++)
		{
			expressivePlaceholders[i].placeholder.SetActive(false);
		}
		expressiveClouds = new List<GameObject>();

		// TODO REMOVE
		//LevelHandler.Instance.LoadEtherRing(this, null);
		//LoadExpressiveClouds();
	}

	public void LoadExpressiveClouds()
	{
		for (int i = 0; i < expressivePlaceholders.Count; i++)
		{
			if (expressivePlaceholders[i] != null && expressivePlaceholders[i].cloudPrefab != null && expressivePlaceholders[i].placeholder != null)
			{
				GameObject expressiveCloud = (GameObject)Instantiate(expressivePlaceholders[i].cloudPrefab, expressivePlaceholders[i].placeholder.transform.position, Quaternion.identity);
				expressiveCloud.transform.parent = expressivePlaceholders[i].placeholder.transform.parent;
				expressiveClouds.Add(expressiveCloud);
			}
		}
	}

	public void UnloadExpressiveClouds()
	{
		while(expressiveClouds.Count > 0)
		{
			Destroy(expressiveClouds[0]);
			expressiveClouds.RemoveAt(0);
		}
	}

	void OnDestroy()
	{
		if (Globals.Instance != null && Globals.Instance.existingEther == this)
		{
			Globals.Instance.existingEther = null;
		}
	}
}

[System.Serializable]
public class ExpressivePlaceholder
{
	//public string sceneName;
	public GameObject cloudPrefab;
	public GameObject placeholder;
}
