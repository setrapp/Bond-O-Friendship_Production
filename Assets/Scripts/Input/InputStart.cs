using UnityEngine;
using System.Collections;

public class InputStart : MonoBehaviour {

    private bool player1Ready = false;
    private bool player2Ready = false;
	
	// Update is called once per frame
	void Update () 
    {
        if (player1Ready && player2Ready)
            Debug.Log("Put Start Here. Also Toggle SPlit SCreen");
	
	}

    void OnTriggerEnter(Collider collide)
    {
        if (collide.gameObject.name == "Player 1")
        {
            player1Ready = true;
        }
        if (collide.gameObject.name == "Player 2")
        {
            player2Ready = true;
        }
    }

    void OnTriggerExit(Collider collide)
    {
        if (collide.gameObject.name == "Player 1")
        {
            player1Ready = false;
        }
        if (collide.gameObject.name == "Player 2")
        {
            player2Ready = false;
        }

    }
}
