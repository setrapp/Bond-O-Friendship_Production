using UnityEngine;
using System.Collections;

public class OrphanFluffHolder : MonoBehaviour {
	private static OrphanFluffHolder instance = null;
	public static OrphanFluffHolder Instance
	{
		get
		{
			if (instance == null && Globals.Instance != null)
			{

				GameObject orphanFluffHolderObj = (GameObject)Instantiate(Globals.Instance.orphanFluffHolderPrefab);
				if (orphanFluffHolderObj != null)
				{
					instance = orphanFluffHolderObj.GetComponent<OrphanFluffHolder>();
				}
			}
			return instance;
		}
	}
}
