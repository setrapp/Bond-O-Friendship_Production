using UnityEngine;
using System.Collections;

public class ToggleEraser : MonoBehaviour {

    void OnTriggerExit(Collider collide)
    {
        if (collide.gameObject.name == "Player 1" || collide.gameObject.name == "Player 2")
        {
            collide.gameObject.GetComponent<Paint>().eraserOn = !collide.gameObject.GetComponent<Paint>().eraserOn;
        }
    }

   /* void OnTriggerExit(Collider collide)
    {
        if (collide.gameObject.name == "Player 1")
        {
            player1 = collide.gameObject;
            player1.GetComponent<Paint>().painting = false;
            //print ("Paintfalse");
        }
        if (collide.gameObject.name == "Player 2")
        {
            player2 = collide.gameObject;
            player2.GetComponent<Paint>().painting = false;
            //print ("Paint");
        }
    }*/
}
