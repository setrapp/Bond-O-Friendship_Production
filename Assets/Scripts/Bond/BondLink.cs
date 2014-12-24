using UnityEngine;
using System.Collections;

public class BondLink : MonoBehaviour {
	public BoxCollider toPreviousCollider = null;
	public BoxCollider toNextCollider = null;
	public Rigidbody body = null;
	public SpringJoint jointPrevious = null;
	public SpringJoint jointNext = null;
	public SpringJoint jointToAttachment = null;
	public int orderLevel = 0;
}
