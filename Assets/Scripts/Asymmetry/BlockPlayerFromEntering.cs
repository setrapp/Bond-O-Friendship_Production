using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockPlayerFromEntering : MonoBehaviour {

    public bool activated = false;
	public ClusterNodePuzzle activatingPuzzle;
	public List <BlockPlayerFromEntering> followingBlockers;
    public GameObject playerToAllow;
    public GameObject partnerBlocker;

    [HideInInspector] public bool activateThisUpdate;
    [HideInInspector] public bool deactivateThisUpdate;
    //[HideInInspector] public GameObject playerToBlock;
    [HideInInspector] SpringJoint springJointScript;
    public GameObject primary;
    public GameObject secondary;
    [HideInInspector] public GameObject player1;
    [HideInInspector] public GameObject player2;
    
	// Use this for initialization
	void Start () {
        springJointScript = GetComponent<SpringJoint>();
        //make trigger
        springJointScript.GetComponent<BoxCollider>().isTrigger = true;
        //disable collision with spring joint partner
        springJointScript.enableCollision = false;

        player1 = Globals.Instance.Player1.gameObject;
        player2 = Globals.Instance.Player2.gameObject;

		Color startColor = GetComponent<MeshRenderer> ().material.color;
		startColor.a = 0;
		GetComponent<MeshRenderer> ().material.color = startColor;
	}
	
	// Update is called once per frame
	void Update () {

		if (primary != null)
			playerToAllow = primary;

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
            ActivateAndAllow(playerToAllow);
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
		gameObject.GetComponent<MeshRenderer>().material.color = player.GetComponent<CharacterColors>().baseColor;

		//activate followers if any:
		if (followingBlockers.Count > 0) {
			int count = followingBlockers.Count;
			for (int i=0; i<count; i++) {
				SpringJoint followerSpringJointScript = followingBlockers [i].GetComponent<SpringJoint> ();
				//Set same variables as this one:
				followingBlockers[i].activated = true;
				followingBlockers[i].primary = primary;
				followingBlockers[i].secondary = secondary;
				followingBlockers[i].playerToAllow = playerToAllow;
				followerSpringJointScript.connectedBody = player.GetComponent<Rigidbody> ();
				followerSpringJointScript.GetComponent<BoxCollider> ().isTrigger = false;
				 
				//Change Material
				followingBlockers[i].gameObject.GetComponent<MeshRenderer>().material.color = player.GetComponent<CharacterColors>().baseColor;
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
		//only check if it isn't already activated
			if (other.gameObject.tag == "Character") {
				//nobody has entered yet, or both have exited
				if (primary == null && secondary == null) {
					primary = other.gameObject;
				}
			//one's inside and one's outside
            else if (primary != null && primary != other.gameObject && secondary == null)
					secondary = other.gameObject;
			}
    }

    void OnTriggerExit(Collider other)
    {
			if (other.gameObject.tag == "Character") {
				//one's already outside and the other's exiting now
				if (other.gameObject == primary && secondary == null) {
					primary = null;
				}
				//primary's exiting while secondary is inside, make latter primary
				if (other.gameObject == primary && secondary != null) {
					//set partner blocker to allow the player that just exited
					//if (other == player1)
					//	partnerBlocker.GetComponent<BlockPlayerFromEntering> ().primary = player1;
					//else
					//	partnerBlocker.GetComponent<BlockPlayerFromEntering> ().primary = player2;
					//set player still within to become primary
					primary = secondary;
					secondary = null;
				} 
				//if primary is inside and secondary exits
				else if (other.gameObject == secondary) {
					secondary = null;
				}
			}
    }

}
