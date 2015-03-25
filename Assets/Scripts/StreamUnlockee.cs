using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StreamUnlockee : MonoBehaviour {

	[SerializeField]
	public List<GameObject> unlockees;
	public bool forceNonKinematic = true;

	void OnTriggerEnter(Collider col)
	{
		if (LayerMask.LayerToName(col.gameObject.layer) == "Water")
		{
			for (int i = 0; i < unlockees.Count; i++)
			{
				unlockees[i].BroadcastMessage("StreamUnlock", col.GetComponent<Stream>(), SendMessageOptions.DontRequireReceiver);
				if (forceNonKinematic)
				{
					Rigidbody unlockBody = unlockees[i].GetComponent<Rigidbody>();
					if (unlockBody != null)
					{
						unlockBody.isKinematic = false;
					}
				}
			}
		}
	}
}
