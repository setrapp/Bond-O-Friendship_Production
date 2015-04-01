using UnityEngine;
using System.Collections;

public class SquashStretchVelocity : MonoBehaviour {
	public Rigidbody body;
	public GameObject squashStretchTarget;
	public float squashStretchFactor;
	private Vector3 oldVelocity;
	public Vector3 directionRotation;
	public float squashDrag = 0.95f;

	void Start()
	{
		if (body == null)
		{
			body = GetComponent<Rigidbody>();
		}
		oldVelocity = body.velocity;
	}

	void Update()
	{
		Vector3 baseScale = squashStretchTarget.transform.localScale;
		Vector3 velocity = Quaternion.Euler(directionRotation) * transform.InverseTransformDirection(body.velocity);

		// Remove scaling from last frame.
		float oldStretchX = squashStretchFactor;
		float oldStretchZ = -squashStretchFactor;
		float oldVelocityStretch = oldVelocity.x;
		if (oldVelocity.x < oldVelocity.z)
		{
			oldStretchX *= -1;
			oldStretchZ *= -1;
			oldVelocityStretch = oldVelocity.z;
		}
		baseScale -= new Vector3(oldVelocityStretch * oldStretchX, 0, oldVelocityStretch * oldStretchZ);

		// Scale target based current velocity, stretching i nthe 
		float stretchX = squashStretchFactor;
		float stretchZ = -squashStretchFactor;
		float velocityStretch = velocity.x;
		if (velocity.x < velocity.z)
		{
			stretchX *= -1;
			stretchZ *= -1;
			velocityStretch = velocity.z;
		}

		Vector3 newScale = baseScale + new Vector3(velocityStretch * stretchX, 0, velocityStretch * stretchZ);
		squashStretchTarget.transform.localScale = newScale;

		oldVelocity = velocity;

		
		/* TODO scale based on change from last frame*/
		/* TODO maybe do a fake drag factor for old velocity that applies when velocity is falling*/

		/* TODO allow for arbitrary squash and stretch vectors*/
		/* TODO squash when velocity is decreasing, maybe handle on collision*/
	}

	void OnCollisionEnter(Collision col)
	{
		/*TODO squash the charcter on the side that is collided on ... probably a shader thing*/
	}
}
