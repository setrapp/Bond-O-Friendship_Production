using UnityEngine;
using System.Collections;

public class ObjectBondReact : MonoBehaviour {

	public GameObject reactingObject;
	public bool activeOnBond = true;
	private bool wasBonded = false;

	void Start()
	{
		wasBonded = !Globals.Instance.playersBonded;
		CheckBond();
	}

	void Update()
	{
		CheckBond();
	}

	private void CheckBond()
	{
		if (wasBonded != Globals.Instance.playersBonded && reactingObject != null)
		{
			if (activeOnBond)
			{
				reactingObject.SetActive(Globals.Instance.playersBonded);
			}
			else
			{
				reactingObject.SetActive(!Globals.Instance.playersBonded);
			}
			wasBonded = Globals.Instance.playersBonded;
		}
	}
}
