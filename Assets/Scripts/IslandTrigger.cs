using UnityEngine;
using System.Collections;

public class IslandTrigger : MonoBehaviour {
	public Island targetIsland;
	public bool toggleLandingOn = true;

	void OnTriggerEnter(Collider other)
	{
		if (targetIsland != null && (other.gameObject == Globals.Instance.player1.gameObject || other.gameObject == Globals.Instance.player2.gameObject))
		{
			if (toggleLandingOn)
			{
				if (targetIsland.container != null)
				{
					targetIsland.container.SendMessage("IslandLanded", other.gameObject, SendMessageOptions.DontRequireReceiver);
				}
				else if (targetIsland.levelHelper != null && targetIsland.levelHelper.landingEnabledObjects != null)
				{
					targetIsland.levelHelper.landingEnabledObjects.ToggleObjects(true);
				}
			}
			else
			{
				if (targetIsland.container != null)
				{
					targetIsland.container.SendMessage("IslandUnlanded", other.gameObject, SendMessageOptions.DontRequireReceiver);
				}
				else if (targetIsland.levelHelper != null && targetIsland.levelHelper.landingEnabledObjects != null)
				{
					targetIsland.levelHelper.landingEnabledObjects.ToggleObjects(false);
				}
			}
		}
	}
}
