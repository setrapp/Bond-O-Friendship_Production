using UnityEngine;
using System.Collections;

public class LoopTag : MonoBehaviour {

	//Will the object move if it's on screen?
	public bool loopOnCamera = false;

	//Move the parent object or just the child?
	public bool moveRoot = true;

	// Keep object out of bounds if it already is?
	public bool stayOutsideBounds = true;

	// Allow object to pass through boundary;
	public bool passThrough = false;

	//Track This Object?
	public bool trackObject = false;
}
