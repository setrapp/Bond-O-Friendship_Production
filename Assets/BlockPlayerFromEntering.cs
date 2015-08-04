using UnityEngine;
using System.Collections;

public class BlockPlayerFromEntering : MonoBehaviour {

    public bool activated;
    public GameObject playerToAllow;
    [HideInInspector] public GameObject playerToBlock;
    public GameObject p1Blocker;
    public GameObject p2Blocker;
    [HideInInspector] public GameObject firstEntered;
    [HideInInspector] public GameObject secondEntered;
    [HideInInspector] public bool activateThisUpdate;
    [HideInInspector] public bool deactivateThisUpdate;

    [HideInInspector] public GameObject player1 = Globals.Instance.player1.gameObject;
    [HideInInspector] public GameObject player2 = Globals.Instance.player2.gameObject;
    public Material playerMaterial;

	// Use this for initialization
	void Start () {

        DeactivateBlockers();
	
	}
	
	// Update is called once per frame
	void Update () {

        if (!activated && activateThisUpdate)
        {
            activated = true;
            if (playerToAllow == player1)
            {
                BlockPlayers(false, true);
                //do more collision stuff here
            }

            else
            {
                BlockPlayers(true, false);
                //do more collision stuff here
            }
        }

        if (activated && deactivateThisUpdate)
        {
            activated = false;
            DeactivateBlockers();
            //do more collision stuff here
        }
	}

    void BlockPlayers(bool blockP1, bool blockP2)
    {
        p1Blocker.SetActive(blockP1);
        p2Blocker.SetActive(blockP2);
    }

    void DeactivateBlockers()
    {
        BlockPlayers(false, false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (firstEntered != null && other.gameObject.tag == "Character")
            firstEntered = other.gameObject;
        else if (other.gameObject.tag == "Character")
            secondEntered = other.gameObject;
    }

    void onTriggerExit(Collider other)
    {
        if (other.gameObject == firstEntered)
        {
            firstEntered = secondEntered;
            secondEntered = null;
        }
        else if (other.gameObject == secondEntered)
        {
            secondEntered = null;
        }
    }

}
