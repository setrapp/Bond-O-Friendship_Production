using UnityEngine;
using System.Collections;

public class TrailRotator : MonoBehaviour {
	public float speed;

	void Update()
	{
		transform.Rotate(new Vector3(0, speed * Time.deltaTime, speed * Time.deltaTime));
	}
}
