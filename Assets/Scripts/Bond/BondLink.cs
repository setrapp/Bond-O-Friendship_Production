using UnityEngine;
using System.Collections;

public class BondLink : MonoBehaviour {
	public BoxCollider toPreviousCollider = null;
	public BoxCollider toNextCollider = null;
	public Rigidbody body = null;
	public BondLink linkNext = null;
	public BondLink linkPrevious = null;
	public SpringJoint jointToNeighbor = null;
	public SpringJoint jointToAttachment = null;
	public int orderLevel = 0;
	public bool broken = false;

	void OnDestroy()
	{
		broken = true;
	}
}

public class BondLinkContainer
{
	public BondLink link = null;
}
