using UnityEngine;
using System.Collections;

public class IslandTrigger : MonoBehaviour {
	public Island targetIsland;
	public bool toggleLandingOn = true;

	void OnTriggerEnter(Collider other)
	{
		if (targetIsland != null && targetIsland.container != null && (other.gameObject == Globals.Instance.player1.gameObject || other.gameObject == Globals.Instance.player2.gameObject))
		{
			if (toggleLandingOn)
			{
				targetIsland.container.SendMessage("IslandLanded", other.gameObject, SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				targetIsland.container.SendMessage("IslandUnlanded", other.gameObject, SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}
