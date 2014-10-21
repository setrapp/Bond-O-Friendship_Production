using UnityEngine;
using System.Collections;

public class MovePulse : MonoBehaviour {

	public Vector3 target;
	private float moveSpeed = 2;
	public Vector3 moveVector;
	
	// Update is called once per frame
	void Update () {

			Vector3 direction = target - transform.position;
			direction.z = 0;
			
			float distance = direction.magnitude;
			
			float decelerationFactor = distance / 1.5f;
			
			float speed = moveSpeed * decelerationFactor;
			
			//transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);

			moveVector = direction.normalized * Time.deltaTime * speed;
			transform.position += moveVector;

	
	}
}
