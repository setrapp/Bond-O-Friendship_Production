using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class ShapingPoint : MonoBehaviour {
	public Rigidbody body;
	public float shapingForce = -1;
}
