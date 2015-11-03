using UnityEngine;
using System.Collections;

public class BondLink : MonoBehaviour {
	public Bond bond;
	public BoxCollider toPreviousCollider = null;
	public BoxCollider toNextCollider = null;
	public Rigidbody body = null;
	public BondLink linkNext = null;
	public BondLink linkPrevious = null;
	public SpringJoint jointToNeighbor = null;
	public SpringJoint jointToAttachment = null;
	public int orderLevel = 0;
	public bool broken = false;

	public void AddJointToAttachment()
	{
		if (jointToAttachment == null)
		{
			jointToAttachment = gameObject.AddComponent<SpringJoint> ();
			jointToAttachment.autoConfigureConnectedAnchor = false;
			jointToAttachment.anchor = Vector3.zero;
			jointToAttachment.connectedAnchor = Vector3.zero;
			jointToAttachment.spring = 0;
			jointToAttachment.damper = 0;
		}
	}

	public void RemoveJointToAttachment()
	{
		if (jointToAttachment != null)
		{
			Destroy(jointToAttachment);
			jointToAttachment = null;
		}
	}

	void OnDestroy()
	{
		broken = true;
	}
}

public class BondLinkContainer
{
	public BondLink link = null;
}
