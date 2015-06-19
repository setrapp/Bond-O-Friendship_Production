using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClusterNodeBit : MonoBehaviour {

	public ClusterNode targetNode;

	void Start()
	{
		if (targetNode == null && transform.parent != null)
		{
			targetNode = transform.parent.GetComponent<ClusterNode>();
		}

		if (targetNode == null)
		{
			Debug.LogError("ClusterNodeBit " + gameObject.name + " is not targetting a cluster node.");
		}
	}

    virtual protected void OnCollisionEnter(Collision col)
	{
		if (targetNode != null)
		{
			targetNode.CheckCollision(col.collider);
		}
	}

    virtual protected void OnTriggerEnter(Collider col)
	{
		if (targetNode != null)
		{
			targetNode.CheckCollision(col);
		}
	}


}
