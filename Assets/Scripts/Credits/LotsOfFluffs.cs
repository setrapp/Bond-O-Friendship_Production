using UnityEngine;
using System.Collections;

public class LotsOfFluffs : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Globals.Instance.Player1.character.fluffHandler.naturalFluffCount = 30;
		Globals.Instance.Player1.character.fluffHandler.spawnTime = 0.1f;
		Globals.Instance.Player2.character.fluffHandler.naturalFluffCount = 30;
		Globals.Instance.Player2.character.fluffHandler.spawnTime = 0.1f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
