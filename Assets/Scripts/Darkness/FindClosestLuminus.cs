using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FindClosestLuminus : MonoBehaviour {

	public Luminus[] luminusArray;
	public Luminus l1, l2;   //Closest to P1, P2
	//[HideInInspector] public GameObject p1, p2;   //Players
	[HideInInspector] public SetShaderData_DarkAlphaMasker darkMaskScript;    //on the alpha masking plane
	[HideInInspector] public float l1p1_sqMag, l2p2_sqMag;    //shortest distances (useful in shader)

	// Use this for initialization
	void Start () 
	{
		//p1 = Globals.Instance.Player1.gameObject;
		//p2 = Globals.Instance.Player2.gameObject;

		//FindAllLumini();

		//darkMaskScript = GameObject.Find("DarkMask").GetComponent<SetShaderData_DarkAlphaMasker>();
		darkMaskScript = Globals.Instance.darknessMask;
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

	public void FindAllLumini()
	{
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

		// Store the actual luminus components.
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

	void FindClosestLumini()
	{
		if (luminusArray.Length < 1)
		{
			return;
		}

		if (luminusArray[0] == null)
		{
			FindAllLumini();
			if (luminusArray.Length < 1)
			{
				return;
			}
		}

		Luminus newL1, newL2, l2Alt;
		newL1 = newL2 = l2Alt = luminusArray[0];
		Vector3 l1p1 = Globals.Instance.Player1.transform.position - newL1.transform.position;
		Vector3 l2p2 = Globals.Instance.Player2.transform.position - newL2.transform.position;
		Vector3 l2Altp2 = Globals.Instance.Player1.transform.position - l2Alt.transform.position;
		Vector3 temp;

		l1p1_sqMag = l1p1.sqrMagnitude;
		l2p2_sqMag = l2p2.sqrMagnitude;
		float l2Altp2_sqMag = l2p2.sqrMagnitude;

		for (int i = 1; i < luminusArray.Length; i++)
		{
			//Closest to p1
			temp = Globals.Instance.Player1.transform.position - luminusArray[i].transform.position;
			//if a different luminus is closer
			if (temp.sqrMagnitude < l1p1_sqMag)
			{
				newL1 = luminusArray[i];
				l1p1_sqMag = temp.sqrMagnitude;
			}

			//Closest to P2
			temp = Globals.Instance.Player2.transform.position - luminusArray[i].transform.position;
			//if a different luminus is closer
			if (temp.sqrMagnitude < l2p2_sqMag)
			{
				// Store the previous nearest luminus, in case the nearest is shared with player one.
				if ((!l2Alt.isOn || l2Alt == newL1 || l2p2_sqMag < l2Altp2_sqMag) && newL1 != newL2)
				{
					l2Alt = newL2;
					l2Altp2_sqMag = l2p2_sqMag;
				}

				newL2 = luminusArray[i];
				l2p2_sqMag = temp.sqrMagnitude;
			}
			// Store the second nearest luminus, in case the nearest is shared with player one.
			else if ((l2Alt == newL1 || temp.sqrMagnitude < l2Altp2_sqMag) && newL1 != luminusArray[i])
			{
				l2Alt = luminusArray[i];
				l2Altp2_sqMag = temp.sqrMagnitude;
			}
		}

		// If player two is nearest to the same luminus as player one, use the alternate luminus.
		if (newL2 == newL1)
		{
			newL2 = l2Alt;
		}

		AttemptLuminusChange(newL1, newL2);
	}

	private void AttemptLuminusChange(Luminus newL1, Luminus newL2)
	{
		if (l1 == null)
		{
			l1 = newL1;
		}
		if (l2 == null)
		{
			l2 = newL2;
		}

		// If a new luminus matches the existing luminus targeted by the opposite player, swap targets to avoid fading a luminus still in use.
		if (newL2 == l1)
		{
			l1 = l2;
			l2 = newL2;
		}
		else if (newL1 == l2)
		{
			l2 = l1;
			l1 = newL1;
		}

		// Fade out current lumini and fade in new ones over time. Only switch a luminus when the old one is faded out.
		if (l1 != newL1)
		{
			l1.fadingIn = false;
			if (l1.intensity <= 0)
			{
				l1 = newL1;
				l1.fadingIn = true;
			}
		}
		if (l2 != newL2)
		{
			l2.fadingIn = false;
			if (l2.intensity <= 0)
			{
				l2 = newL2;
				l2.fadingIn = true;
			}
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
