using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FindClosestLuminus : MonoBehaviour {

	[HideInInspector] public Luminus[] luminusArray;
	public Luminus l1, l2;   //Closest to P1, P2
	public Luminus l2Alt;
	[HideInInspector] public GameObject p1, p2;   //Players
	[HideInInspector] public SetShaderData_DarkAlphaMasker darkMaskScript;    //on the alpha masking plane
	[HideInInspector] public float l1p1_sqMag, l2p2_sqMag;    //shortest distances (useful in shader)

	// Use this for initialization
	void Start () 
	{
		p1 = Globals.Instance.player1.gameObject;
		p2 = Globals.Instance.player2.gameObject;

		/*TODO make sure this doesn't break when moving between two levels with a darkness layer*/
		// Find all objects with the luminus tag, ignoring objects that do not have a luminus component.
		GameObject[] luminusObjects = GameObject.FindGameObjectsWithTag("Luminus");
		int luminusCount = luminusObjects.Length;
		for (int i = 0; i < luminusObjects.Length; i++)
		{
			if (luminusObjects[i].GetComponentInChildren<Luminus>() == null)
			{
				luminusObjects[i] = null;
				luminusCount--;
			}
		}

		// If any luminus are found store the actual luminus components.
		if (luminusCount > 0)
		{
			luminusArray = new Luminus[luminusCount];
			int luminusIndex = 0;
			for (int i = 0; i < luminusObjects.Length; i++)
			{
				if (luminusObjects[i] != null)
				{
					luminusArray[luminusIndex] = luminusObjects[i].GetComponentInChildren<Luminus>();
					luminusIndex++;
				}
			}
		}
		
		/* TODO What happens when none of the lumini are on??? */

		darkMaskScript = GameObject.Find("DarkMask").GetComponent<SetShaderData_DarkAlphaMasker>();
		//Find which ones are closest at the beginning
		FindClosestLumini();
		SendDataToDarkMask();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Update closest lumini every frame
		FindClosestLumini();
		SendDataToDarkMask();
	}

	void FindClosestLumini()
	{
		l1 = l2 = luminusArray[0];
		l2Alt = luminusArray[0];
		Vector3 l1p1 = p1.transform.position - l1.transform.position;
		Vector3 l2p2 = p2.transform.position - l2.transform.position;
		Vector3 l2Altp2 = p2.transform.position - l2Alt.transform.position;
		Vector3 temp;

		l1p1_sqMag = l1p1.sqrMagnitude;
		l2p2_sqMag = l2p2.sqrMagnitude;
		float l2Altp2_sqMag = l2p2.sqrMagnitude;

		for (int i = 1; i < luminusArray.Length; i++)
		{
			if (luminusArray[i].isOn)
			{
				//Closest to p1
				temp = p1.transform.position - luminusArray[i].transform.position;
				//if a different luminus is closer
				if (!l1.isOn || temp.sqrMagnitude < l1p1_sqMag)
				{
					l1 = luminusArray[i];
					l1p1_sqMag = temp.sqrMagnitude;
				}

				//Closest to P2
				temp = p2.transform.position - luminusArray[i].transform.position;
				//if a different luminus is closer
				if (!l2.isOn || temp.sqrMagnitude < l2p2_sqMag)
				{
					// Store the previous nearest luminus, in case the nearest is shared with player one.
					if ((!l2Alt.isOn || l2Alt == l1 || l2p2_sqMag < l2Altp2_sqMag) && l1 != l2)
					{
						l2Alt = l2;
						l2Altp2_sqMag = l2p2_sqMag;
					}

					l2 = luminusArray[i];
					l2p2_sqMag = temp.sqrMagnitude;
				}
				// Store the second nearest luminus, in case the nearest is shared with player one.
				else if ((!l2Alt.isOn || l2Alt == l1 || temp.sqrMagnitude < l2Altp2_sqMag) && l1 != luminusArray[i])
				{
					l2Alt = luminusArray[i];
					l2Altp2_sqMag = temp.sqrMagnitude;
				}
			}
		}

		// If player two is nearest to the same luminus as player one, use the alternate luminus.
		if (l2 == l1)
		{
			l2 = l2Alt;
		}
	}

	void SendDataToDarkMask()
	{
		darkMaskScript.l1 = l1;
		darkMaskScript.l2 = l2;
		darkMaskScript.l1p1_sqMag = l1p1_sqMag;
		darkMaskScript.l2p2_sqMag = l2p2_sqMag;
	}
}
