using UnityEngine;
using System.Collections;

public class FluffStickRelocater : MonoBehaviour {

	public FluffStick fluffStick;

	void OnCollisionEnter(Collision col)
	{
		if (fluffStick == null || !fluffStick.CanStick())
		{
			return;
		}

		if (col.collider.gameObject == Globals.Instance.Player1.gameObject || col.collider.gameObject == Globals.Instance.Player2.gameObject)
		{
			Debug.Log("HIa");
			if (col.contacts.Length > 0)
			{
				Debug.Log("HI");
				fluffStick.transform.position = col.contacts[0].point;
				fluffStick.transform.forward = col.contacts[0].normal;
			}
		}
	}
}
