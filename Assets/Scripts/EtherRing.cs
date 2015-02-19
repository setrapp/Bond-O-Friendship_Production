using UnityEngine;
using System.Collections;

public class EtherRing : MonoBehaviour {
	public MembraneShell ringAtmosphere;

	void Start()
	{
		if (Globals.Instance != null)
		{
			Globals.Instance.existingEther = this;
		}

		if (ringAtmosphere != null)
		{
			ringAtmosphere.CreateShell();
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
