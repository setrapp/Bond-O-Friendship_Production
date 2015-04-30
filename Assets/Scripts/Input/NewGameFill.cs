using UnityEngine;
using System.Collections;

public class NewGameFill : MonoBehaviour {


    public GameObject player1Fill;
    public GameObject player2Fill;
    public MenuControl menuControl;

    private float player1F;
    private float player2F;

    private bool player1Filling = false;
    private bool player2Filling = false;

    private float duration = 2f;

    private Vector3 emptyFillp1 = new Vector3(0f, .99f, 1f);
    private Vector3 emptyFillp2 = new Vector3(0f, .99f, 1f);

    private Vector3 fullFillp1 = new Vector3(.59f, .99f, 1f);
    private Vector3 fullFillp2 = new Vector3(.4f, .99f, 1f);
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        float t = Time.deltaTime / duration;

        player1F += player1Filling ? t : -t;
        player2F += player2Filling ? t : -t;

        player1F = Mathf.Clamp(player1F, 0, 1f);
        player2F = Mathf.Clamp(player2F, 0, 1f);

        if (player1Fill.activeInHierarchy)
            player1Fill.transform.localScale = Vector3.Lerp(emptyFillp1, fullFillp1, player1F);
        if (player2Fill.activeInHierarchy)
            player2Fill.transform.localScale = Vector3.Lerp(emptyFillp2, fullFillp2, player2F);

        if (player1F == 1.0f)
            menuControl.player1Ready = true;
        else
            menuControl.player1Ready = false;

        if (player2F == 1.0f)
            menuControl.player2Ready = true;
        else
            menuControl.player2Ready = false;
	
	}

    void OnTriggerEnter(Collider collide)
    {
        if (collide.gameObject.name == "Player 1")
        {
            player1Filling = true;
        }
        if (collide.gameObject.name == "Player 2")
        {
            player2Filling = true;
        }
    }

    void OnTriggerExit(Collider collide)
    {
        if (!menuControl.player1Ready || !menuControl.player2Ready)
        {
            if (collide.gameObject.name == "Player 1")
            {
                player1Filling = false;
            }
            if (collide.gameObject.name == "Player 2")
            {
                player2Filling = false;
            }
        }

    }
}
