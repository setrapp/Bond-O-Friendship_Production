using UnityEngine;
using System.Collections;

public class TriggerFade : MonoBehaviour {

	public FadeToBeContinued fadeTarget = null;

	private void SendFade()
	{
		fadeTarget.gameObject.SetActive(true);
		fadeTarget.StartFade();
	}

    void OnTriggerEnter(Collider collide)
    {
        if (collide.gameObject.name == "Player 1" || collide.gameObject.name == "Player 2")
            {
                if(!fadeTarget.gameObject.activeSelf)
                    SendFade();
            }
    }

}
