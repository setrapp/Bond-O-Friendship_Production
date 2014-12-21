using UnityEngine;
using System.Collections;

public class BondLink : MonoBehaviour {
	public BoxCollider linkCollider = null;
	public Rigidbody body = null;
	public SpringJoint jointPrevious = null;
	public SpringJoint jointNext = null;
	public SpringJoint jointToAttachment = null;
	public int orderLevel = 0;
}
