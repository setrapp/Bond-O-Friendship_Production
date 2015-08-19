using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockPlayerFromEntering : MonoBehaviour {

    public bool activated = false;
	public ClusterNodePuzzle activatingPuzzle;
	public List <BlockPlayerFromEntering> followingBlockers;
    public GameObject playerToAllow;
    public GameObject partnerBlocker;
    public Material p1Material;
	public Material p2Material;

    [HideInInspector] public bool activateThisUpdate;
    [HideInInspector] public bool deactivateThisUpdate;
    [HideInInspector] public GameObject playerToBlock;
    [HideInInspector] SpringJoint springJointScript;
    [HideInInspector] public GameObject primary;
    [HideInInspector] public GameObject secondary;
    [HideInInspector] public GameObject player1;
    [HideInInspector] public GameObject player2;
    

	// Use this for initialization
	void Start () {

        springJointScript = GetComponent<SpringJoint>();
        //make trigger
        springJointScript.GetComponent<BoxCollider>().isTrigger = true;
        //disable collision with spring joint partner
        springJointScript.enableCollision = false;

        player1 = Globals.Instance.player1.gameObject;
        player2 = Globals.Instance.player2.gameObject;
	
	}
	
	// Update is called once per frame
	void Update () {

		//if responding to puzzle
		if (activatingPuzzle != null) {
			//If they solved the puzzle, activate
			if (activatingPuzzle.solved) {
				activateThisUpdate = true;
			}
		}

        //activating this update
        if (activateThisUpdate && !activated)
        {
			//activate self and following blockers
            ActivateAndAllow(primary);
        }

        if (deactivateThisUpdate && activated)
        {
            Deactivate();
        }
	}

    void ActivateAndAllow(GameObject player)
    {
        activated = true;
        activateThisUpdate = false;
        springJointScript.connectedBody = player.GetComponent<Rigidbody>();
        springJointScript.GetComponent<BoxCollider>().isTrigger = false;
		//gameObject.GetComponent<MeshRenderer>().materials[0] = player.GetComponent<MeshRenderer> ().material;

		//activate followers if any:
		if (followingBlockers.Count > 0) {
			int count = followingBlockers.Count;
			for (int i=0; i<count; i++) {
				SpringJoint followerSpringJointScript = followingBlockers [i].GetComponent<SpringJoint> ();
				//Set same variables as this one:
				followingBlockers[i].activated = true;
				followerSpringJointScript.connectedBody = player.GetComponent<Rigidbody> ();
				followerSpringJointScript.GetComponent<BoxCollider> ().isTrigger = false;
				//Change Material
				//followingBlockers[i].gameObject.GetComponent<MeshRenderer>().materials[0] = player.GetComponent<MeshRenderer> ().material;
			}
		}
        //Do more visuals stuff here
    }

    void Deactivate()
    {
        activated = false;
        deactivateThisUpdate = false;
        springJointScript.GetComponent<BoxCollider>().isTrigger = true;

        //Do more visuals stuff here
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Character")
        {
            if (primary == null)
            {
                primary = other.gameObject;
                //set second blocker to allow other player
                if (primary == player1)
                    partnerBlocker.GetComponent<BlockPlayerFromEntering>().primary = player2;
                else
                    partnerBlocker.GetComponent<BlockPlayerFromEntering>().primary = player1;
            }
            else
                secondary = other.gameObject;
        }
    }

    void onTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Character")
        {
            if (other.gameObject == primary)
            {
                //set partner blocker to allow the player that just exited
                if (other == player1)
                    partnerBlocker.GetComponent<BlockPlayerFromEntering>().primary = player1;
                else
                    partnerBlocker.GetComponent<BlockPlayerFromEntering>().primary = player2;
                //set self to allow the player still inside
                primary = secondary;
                secondary = null;
            }
            else if (other.gameObject == secondary)
            {
                secondary = null;
            }
        }
    }

}
