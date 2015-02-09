using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Island : MonoBehaviour {
	public IslandID islandId;
	public IslandContainer container;
}

public enum IslandID
{
	NONE = 0,
	TUTORIAL,
	HARMONY_A,
	INTIMACY_A,
	ASYMMETRY_A
};