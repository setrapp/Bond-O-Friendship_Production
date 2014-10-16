using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Waypoint : MonoBehaviour {
	public bool isStart = false;
	public Waypoint loopBackTo;
	public int maxLoopBacks;
	public int loopBacks;
	[SerializeField]
	public List<PointSpawn> pointSpawns;
}

[System.Serializable]
public class PointSpawn
{
	public GameObject pointPrefab;
	public Vector3 offset;
	public float informationFactor;
	//public bool setInformationFactor;
	public bool requirePartner = true;
}