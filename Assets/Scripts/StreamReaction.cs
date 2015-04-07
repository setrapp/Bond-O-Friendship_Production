using UnityEngine;
using System.Collections;

public abstract class StreamReaction : MonoBehaviour {

	public Collider reactionCollider;
	public Rigidbody reactionBody;

	public abstract void React(Stream stream);
}
