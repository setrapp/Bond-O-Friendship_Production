using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

    public GameObject player;

	// Use this for initialization
	void Start () {
        player = Globals.Instance.player1.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = player.transform.position;
	}
}
