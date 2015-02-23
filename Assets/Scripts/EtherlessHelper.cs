using UnityEngine;
using System.Collections;

public class EtherlessHelper : MonoBehaviour {
	void Start()
	{
		// Only keep this object, and its children, if no ether ring exists (useful for boundaries).
		if (Globals.Instance != null && Globals.Instance.existingEther == null)
		{
			// TODO This is not useful because the object is already active or will not do anything.
			gameObject.SetActive(true);
		}
		else
		{
			Destroy(gameObject);
		}
	}
}
